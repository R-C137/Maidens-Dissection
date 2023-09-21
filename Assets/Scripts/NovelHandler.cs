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
 *  [17/09/2023] - Re order variables + Implemented audio system (C137)
 *  [20/09/2023] - Remap speaker names + Fix progress saving + Speaker name flower animation + Error handling + Improved character fading(C137)
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
    /// Background indexes that have been shown
    /// </summary>
    [HideInInspector]
    public List<int> shownBackgrounds = new();

    /// <summary>
    /// The current script being played. Used for choice system
    /// </summary>
    [HideInInspector]
    public NovelScript currentScript;

    /// <summary>
    /// The text writer for the background title
    /// </summary>
    [Header("Writers")]
    public TextWriter backgroundWriter;

    /// <summary>
    /// Reference to the writer, to set proper values based on the script
    /// </summary>
    public TextWriter storyWriter;

    /// <summary>
    /// Reference to the back button
    /// </summary>
    [Header("General References")]
    public GameObject backButton;

    /// <summary>
    /// The parent of the main story ui
    /// </summary>
    public GameObject mainStory;

    /// <summary>
    /// Reference to the background image, to change based on the script
    /// </summary>
    public Image background;

    /// <summary>
    /// The text shower for the speaker
    /// </summary>
    public TextMeshProUGUI speakerShower;

    /// <summary>
    /// The image for the text shower of the speaker
    /// </summary>
    public Image speakerImage;

    /// <summary>
    /// Whether to save the current progress
    /// </summary>
    public bool saveProgress = true;

    [Header("Indexes")]
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

    [Space(10)]
    /// <summary>
    /// The current act, used for internal purposes
    /// </summary>
    public int act = 1;

    /// <summary>
    /// Tween used for the fading out of the speaker text
    /// </summary>
    int speakerTweenFadeOut = -1;

    /// <summary>
    /// Tween used for the fading in of the speaker text
    /// </summary>
    int speakerTweenFadeIn = -1;

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
            if (!storyWriter.writing)
                ProgressScript();
            else
                storyWriter.Skip();
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

        //Handle audio
        HandleAudio();

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
                if (!speakerImage.gameObject.activeSelf)
                    goto NormalFlow;

                if(speakerTweenFadeOut != -1)
                    LeanTween.cancel(speakerTweenFadeOut);

                speakerTweenFadeOut = LeanTween.value(1, 0, .5f)
                    .setOnUpdate(v => 
                    { 
                        try 
                        { 
                            speakerImage.fillAmount = v; 
                        } 
                        catch (Exception) 
                        { 
                            return; 
                        } 
                    }).setOnComplete(() => { try { speakerShower.transform.parent.gameObject.SetActive(false); } catch (Exception) { return; } }).uniqueId;
            }
            else
            {
                if (speakerShower.text == RemapName(script.speaker) && speakerImage.gameObject.activeSelf)
                    goto NormalFlow;

                if (speakerTweenFadeIn != -1)
                    LeanTween.cancel(speakerTweenFadeIn);

                speakerTweenFadeIn = LeanTween.value(0, 1, .5f)
                    .setOnUpdate(v =>
                    {
                        try
                        {
                            speakerImage.fillAmount = v;
                        }
                        catch (Exception)
                        {
                            return;
                        }
                    }).uniqueId;

                speakerShower.transform.parent.gameObject.SetActive(true);
                speakerShower.text = RemapName(script.speaker);
            }

            NormalFlow:
            storyWriter.textShower.fontStyle = script.fontStyle;

            storyWriter.text = script.script;
            if(storyWriter.textShower != null)
                storyWriter.textShower.text = string.Empty;

            storyWriter.speed = script.writerSpeed;
            storyWriter.delay = script.writerDelay;

            storyWriter.Write();

            //Remaps the names of the speakers
            string RemapName(string name)
            {
                switch (name)
                {
                    case "NARRATION":
                        return "Narration";

                    case "MC":
                        return "MC";

                    default:
                        return name;
                }
            }
        }

        void HandleCharacters()
        {
            //if (script.characters == null || !script.characters.Any())
            //    return;

            CharacterHandler.ShowCharacters(script);

            //LeanTween.cancel(gameObject);
            //for (int i = 0; i < characters.Length; i++)
            //{
            //    if (i < script.characters.Length)
            //    {
            //        if (characters[i].sprite == script.characters[i] && characters[i].gameObject.activeSelf)
            //            continue;

            //        characters[i].gameObject.SetActive(true);
            //        characters[i].sprite = script.characters[i];

            //        int index = i;
            //        LeanTween.value(0, 1, .5f).setOnUpdate(v =>
            //        {
            //            try
            //            {
            //                characters[index].color = new(characters[index].color.r, characters[index].color.g, characters[index].color.b, v);
            //            }
            //            catch (Exception)
            //            {
            //                return;
            //            }
            //        });

            //        continue;
            //    }
            //    int index2 = i;
            //    LeanTween.value(1, 0, .5f).setOnUpdate(v =>
            //    {
            //        try
            //        {
            //            characters[index2].color = new(characters[index2].color.r, characters[index2].color.g, characters[index2].color.b, v);
            //        }
            //        catch (Exception)
            //        {
            //            return;
            //        }
            //    }).setOnComplete(() =>
            //    {
            //        try
            //        {
            //            characters[index2].gameObject.SetActive(false);
            //            characters[index2].color = new(characters[index2].color.r, characters[index2].color.g, characters[index2].color.b, 1);
            //        }catch(Exception)
            //        { 
            //            return; 
            //        }
            //    });
            //}
        }

        void HandleAudio()
        {
            //Voice acting
            AudioHandler.PlayVoiceActing(script.audio.voiceActing);

            //Background music
            AudioHandler.PlayBackground(script.audio.background, script.audio.loopBackground);
        }

        bool HandleBackground()
        {
            background.sprite = script.backgroundSprite == null ? background.sprite : script.backgroundSprite;

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

    private void OnDisable()
    {
        LeanTween.cancel(gameObject);
    }
}
