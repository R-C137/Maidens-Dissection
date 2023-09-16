/*
 * NovelHandler.cs - Maiden's Dissection
 * 
 * Creation Date: 12/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [12/09/2023] - Initial Implementation (C137)
 *  [13/09/2023] - Choice based script progress + Reworked background (C137)
 *  [15/09/2023] - Code cleanup + Fixed choice system + Improved text writing (C137)
 *  [16/09/2023] - Save progress only in builds + Unlock new act on finish (C137)
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NovelHandler : MonoBehaviour
{
    /// <summary>
    /// The different scripts for this act
    /// </summary>
    public NovelScript[] scripts;

    /// <summary>
    /// The image of the different available character shower
    /// </summary>
    public Image[] characters;

    /// <summary>
    /// The buttons of the different choices
    /// </summary>
    public Button[] choices;

    /// <summary>
    /// The current script being played. Used for choice system
    /// </summary>
    public NovelScript currentScript;

    /// <summary>
    /// Reference to the back button
    /// </summary>
    public GameObject backButton;

    /// <summary>
    /// The text writer for the background title
    /// </summary>
    public TextWriter backgroundWriter;

    /// <summary>
    /// Background indexes that have been shown
    /// </summary>
    public List<int> shownBackgrounds = new();

    /// <summary>
    /// The parent of the main story ui
    /// </summary>
    public GameObject mainStory;

    /// <summary>
    /// The current script index the player has reached
    /// </summary>
    public int currentScriptIndex = -1;

    /// <summary>
    /// The current choice script index the player has reached
    /// </summary>
    public int currentChoiceIndex = -1;

    /// <summary>
    /// The index of the choice made
    /// </summary>
    public int choiceMadeIndex = -1;

    /// <summary>
    /// The current act, used for internal purposes
    /// </summary>
    public int act = 1;

    /// <summary>
    /// Reference to the background image, to change based on the script
    /// </summary>
    public Image background;

    /// <summary>
    /// The text shower for the speaker
    /// </summary>
    public TextMeshProUGUI speakerShower;

    /// <summary>
    /// Whether to save the current progress
    /// </summary>
    public bool saveProgress = true;

    /// <summary>
    /// Reference to the writer, to set proper values based on the script
    /// </summary>
    public TextWriter writer;

    private void Awake()
    {
#if UNITY_EDITOR
        saveProgress = false;
#else
        saveProgress = true;
#endif

        if (saveProgress)
        {
            currentScriptIndex = PlayerPrefs.GetInt($"general.act{act}.scriptpos", -1);

            if(currentScriptIndex >= 1 )
                currentScriptIndex--;
        }
    }

    /// <summary>
    /// Called when the back button is pressed
    /// </summary>
    public void Back()
    {
        currentScriptIndex -= 2;
        if(currentScript != null)
        {
            currentChoiceIndex = -1;
            choiceMadeIndex = -1;
            currentScript = null;
            choices[0].transform.parent.gameObject.SetActive(false);
        }
        ProgressScript();
    }

    private void Start()
    {
        ProgressScript();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!writer.writing)
                ProgressScript();
            else
                writer.Skip();
        }
    }

    /// <summary>
    /// Progresses the script to the next one
    /// </summary>
    void ProgressScript()
    {
        bool choice = false;

        if(currentScript != null)
        {
            if (!HandleChoiceProgress())
            {
                return;
            }
            currentScript = null;
            choice = true;
        }

        currentScriptIndex++;

        if (currentScriptIndex >= scripts.Length)
        {
            int currentSavedAct = PlayerPrefs.GetInt("general.acts", 0);

            if(act <= currentSavedAct)
                PlayerPrefs.SetInt("general.acts", act + 1 );

            Utility.singleton.LoadScene(0);
            return;
        }

        NovelScript script = scripts[currentScriptIndex];

        ShowScript(script, choice);

        backButton.SetActive(currentScriptIndex > 0);
    }

    void ShowScript(NovelScript script, bool skipChoice = false, bool normalScript = true)
    {
        //Handle background
        if (!HandleBackground())
        {
            if (!normalScript)
                currentChoiceIndex--;

            currentScriptIndex--;
            return;
        }

        ///Handle choices
        if (!skipChoice && !HandleChoices())
            return;

        //Handle characters
        HandleCharacters();

        //Handle text showing
        HandleText();

        if (saveProgress)
            PlayerPrefs.SetInt($"general.act{act}.scriptpos", currentScriptIndex);

        void HandleText()
        {
            if (script.speaker == string.Empty || script.speaker == null)
            {
                speakerShower.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                speakerShower.transform.parent.gameObject.SetActive(true);
                speakerShower.text = script.speaker;
            }

            writer.textShower.fontStyle = script.fontStyle;

            writer.text = script.script;
            if(writer.textShower != null)
                writer.textShower.text = string.Empty;
            writer.speed = script.writerSpeed;
            writer.delay = script.writerDelay;

            writer.Write();
        }

        void HandleCharacters()
        {
            //if (script.characters == null || !script.characters.Any())
            //    return;

            LeanTween.cancel(gameObject);
            for (int i = 0; i < characters.Length; i++)
            {
                if (i < script.characters.Length)
                {
                    characters[i].gameObject.SetActive(true);
                    characters[i].sprite = script.characters[i];

                    int index = i;
                    LeanTween.value(0, 1, .5f).setOnUpdate(v =>
                    {
                        characters[index].color = new(characters[index].color.r, characters[index].color.g, characters[index].color.b, v);
                    });

                    continue;
                }
                int index2 = i;
                LeanTween.value(1, 0, .5f).setOnUpdate(v =>
                {
                    characters[index2].color = new(characters[index2].color.r, characters[index2].color.g, characters[index2].color.b, v);
                }).setOnComplete(() => characters[index2].gameObject.SetActive(false));
            }
        }
        bool HandleBackground()
        {
            background.sprite = script.background == null ? background.sprite : script.background;

            if (script.backgroundTitle != null && script.backgroundTitle != string.Empty && !shownBackgrounds.Contains(script.GetInstanceID()))
            {
                backgroundWriter.text = script.backgroundTitle;
                if(backgroundWriter.textShower != null)
                    backgroundWriter.textShower.text = string.Empty;
                backgroundWriter.transform.parent.gameObject.SetActive(true);
                backgroundWriter.Write();

                mainStory.SetActive(false);

                shownBackgrounds.Add(script.GetInstanceID());

                backButton.SetActive(false);
                return false;
            }
            else
            {
                if (backgroundWriter.writing)
                    backgroundWriter.Skip();
                backgroundWriter.transform.parent.gameObject.SetActive(false);
                mainStory.SetActive(true);
            }
            return true;
        }

        bool HandleChoices()
        {
            if (currentScript != null)
            {
                return true;
            }

            if (script.choices == null || !script.choices.Any())
                return true;

            currentScript = script;

            choices[0].transform.parent.gameObject.SetActive(true);
            mainStory.SetActive(false);

            backButton.SetActive(true);

            for (int i = 0; i < choices.Length; i++)
            {
                if (i < currentScript.choices.Length)
                {
                    choices[i].gameObject.SetActive(true);
                    choices[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = script.choices[i].name;

                    choices[i].onClick.RemoveAllListeners();

                    int choiceIndex = i;

                    choices[i].onClick.AddListener(() => ChoiceMade(choiceIndex));

                    continue;
                }

                choices[i].gameObject.SetActive(false);
            }

            currentScriptIndex--;
            return false;
        }
    }

    bool HandleChoiceProgress()
    {
        if (choiceMadeIndex == -1)
            return false;

        currentChoiceIndex++;

        if (currentChoiceIndex >= currentScript.choices[choiceMadeIndex].followup.Length)
        {
            currentChoiceIndex = -1;
            choiceMadeIndex = -1;
            return true;
        }

        ShowScript(currentScript.choices[choiceMadeIndex].followup[currentChoiceIndex], normalScript: false);

        return false;
    }

    void ChoiceMade(int choiceIndex)
    {
        choices[0].transform.parent.gameObject.SetActive(false);
        mainStory.SetActive(true);
        backButton.SetActive(false);

        choiceMadeIndex = choiceIndex;

        HandleChoiceProgress();
    }
}
