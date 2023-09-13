/*
 * NovelHandler.cs - Maiden's Dissection
 * 
 * Creation Date: 12/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [12/09/2023] - Initial Implementation (C137)
 *  [13/09/2023] - Choice based script progress (C137)
 */
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
    /// The text writer for the background title
    /// </summary>
    public TextWriter backgroundWriter;

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
    public int choiceMadeIndex = 0;

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
    /// Whether the player is in a background animation phase
    /// </summary>
    public bool backgroundAnimation = false;

    /// <summary>
    /// Reference to the writer, to set proper values based on the script
    /// </summary>
    public TextWriter writer;

    private void Awake()
    {
        if(saveProgress)
            currentScriptIndex = PlayerPrefs.GetInt($"general.act{act}.scriptpos", -1);
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
            return;

        NovelScript script = scripts[currentScriptIndex];

        ShowScript(script, choice);
    }

    void ShowScript(NovelScript script, bool skipChoice = false)
    {
        //Handle background
        if (!HandleBackground())
            return;

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
            writer.speed = script.writerSpeed;
            writer.delay = script.writerDelay;

            writer.Write();
        }

        void HandleCharacters()
        {
            if (script.characters == null || !script.characters.Any())
                return;

            for (int i = 0; i < characters.Length; i++)
            {
                if (i < script.characters.Length)
                {
                    characters[i].gameObject.SetActive(true);
                    characters[i].sprite = script.characters[i];
                    continue;
                }

                characters[i].gameObject.SetActive(false);
            }
        }

        bool HandleBackground()
        {
            background.sprite = script.background == null ? background.sprite : script.background;

            if (script.backgroundTitle != null && script.backgroundTitle != string.Empty && !backgroundWriter.transform.parent.gameObject.activeSelf)
            {
                backgroundWriter.text = script.backgroundTitle;
                backgroundWriter.transform.parent.gameObject.SetActive(true);
                backgroundWriter.Write();

                mainStory.SetActive(false);

                currentScriptIndex--;
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
        currentChoiceIndex++;

        if (currentChoiceIndex >= currentScript.choices[currentChoiceIndex].followup.Length)
        {
            currentChoiceIndex = -1;
            return true;
        }

        ShowScript(currentScript.choices[choiceMadeIndex].followup[currentChoiceIndex]);

        return false;
    }

    void ChoiceMade(int choiceIndex)
    {
        choices[0].transform.parent.gameObject.SetActive(false);
        mainStory.SetActive(true);

        choiceMadeIndex = choiceIndex;

        HandleChoiceProgress();
    }
}
