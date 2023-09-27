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
 *  [21/09/2023] - Moved text handler to its own script
 *  [22/09/2023] - Multi-act support (C137)
 *  [26/09/2023] - Background improvements + Progression buttons improvements + SFX improvements (C137)
 *  [27/09/2023] - Added an information window at the end of each act (C137)
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct Acts
{
    public NovelScript[] scripts;
}

public class NovelHandler : MonoBehaviour
{
    /// <summary>
    /// The different scripts for this act
    /// </summary>
    public Acts[] scripts;

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
    public int act = -1;

    /// <summary>
    /// The text shower for the information window
    /// </summary>
    public TextMeshProUGUI informationWindowTextShower;

    /// <summary>
    /// Whether the act has finished playing. Internal use only
    /// </summary>
    bool actFinished = false;

    /// <summary>
    /// Handles the saving and resuming of the novel's progress + The progressing of the script on start
    /// </summary>
    private void Start()
    {
#if UNITY_EDITOR
        saveProgress = false;
#else
        saveProgress = true;
#endif

        if (act == -1)
            act = Utility.currentAct;

        if (saveProgress)
        {
            currentScriptIndex = PlayerPrefs.GetInt($"general.act{act}.scriptpos", -1);

            if(currentScriptIndex >= 1 )
                currentScriptIndex--;
        }

        ProgressScript();
    }

    /// <summary>
    /// Called when the back button is pressed
    /// </summary>
    public void Back()
    {
        if (actFinished)
            return;

        if (shownBackgrounds.Contains(scripts[act].scripts[currentScriptIndex].GetInstanceID()))
            shownBackgrounds.Remove(scripts[act].scripts[currentScriptIndex].GetInstanceID());

        if(act == 0 && currentScriptIndex == 0)//Back buttons shows the bcakground title of the first novel script (act 1 only)
        {
            currentScriptIndex = -1;
            ProgressScript();
            return;
        }        

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

    /// <summary>
    /// Handles the progressing of the script and text writing skipping
    /// </summary>
    private void Update()
    {
        if (actFinished)
            return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            HandleProgression();
        }
    }

    /// <summary>
    /// Either skips the writing effect of progresses the script
    /// </summary>
    public void HandleProgression()
    {
        if (actFinished)
            return;

        if (!storyWriter.writing)
            ProgressScript();
        else
            storyWriter.Skip();
    }

    /// <summary>
    /// Progresses the script to the next one
    /// </summary>
    public void ProgressScript()
    {
        if (actFinished)
            return;

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

        if (currentScriptIndex >= scripts[act].scripts.Length)
        {
            int currentSavedAct = PlayerPrefs.GetInt("general.unlocked-acts", 0);

            if (act <= currentSavedAct)
            {
                PlayerPrefs.SetInt("general.unlocked-acts", act + 1);
                PlayerPrefs.SetInt("general.acts", act + 1);
            }

            ShowInformationWindow(act == 0 ? 
                "Act 2 has been unlocked and is available in the title screen"
                : "Bonus section has been unlocked and is available in the title screen", 2.5f, () => Utility.singleton.LoadScene(0));

            actFinished = true;
            return;
        }

        NovelScript script = scripts[act].scripts[currentScriptIndex];

        ShowScript(script, choice);

        if (act == 0)
            backButton.SetActive(currentScriptIndex > -1);
        else
            backButton.SetActive(currentScriptIndex > 0);
    }

    /// <summary>
    /// Shows the information window
    /// </summary>
    /// <param name="text">The information to display</param>
    /// <param name="showTime">How long to display it</param>
    /// <param name="onComplete">The callback once the window has been displayed</param>
    public void ShowInformationWindow(string text, float showTime, Action onComplete)
    {
        informationWindowTextShower.text = text;
        informationWindowTextShower.transform.parent.parent.gameObject.SetActive(true);
        LeanTween.delayedCall(showTime, onComplete);
    }

    /// <summary>
    /// Handles the showing of the script on screen
    /// </summary>
    /// <param name="script">The script to be shown</param>
    /// <param name="skipChoices">Whether to skip the handling of choices</param>
    /// <param name="normalScript">Whether this script is part of the normal timeline or a choice followup</param>
    void ShowScript(NovelScript script, bool skipChoices = false, bool normalScript = true)
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
        if (!skipChoices && !HandleChoices())
            return;

        //Handle audio
        HandleAudio();

        //Handle characters
        CharacterHandler.ShowCharacters(script);

        //Handle text showing
        TextHandler.HandleText(script);

        //Saves the progress
        if (saveProgress)
            PlayerPrefs.SetInt($"general.act{act}.scriptpos", currentScriptIndex);


        void HandleAudio()
        {
            //Voice acting
            AudioHandler.PlayVoiceActing(script.audio.voiceActing);

            //Background music
            AudioHandler.PlayBackground(script.audio.background, script.audio.loopBackground);

            //SFX
            AudioHandler.PlaySFX(script.audio.sfx);
        }

        bool HandleBackground()
        {
            background.sprite = script.backgroundSprite == null ? background.sprite : script.backgroundSprite;

            if (script.backgroundTitle != null && script.backgroundTitle != string.Empty && !shownBackgrounds.Contains(script.GetInstanceID()))
            {
                backgroundWriter.text = script.backgroundTitle;
                backgroundWriter.textShower.text = string.Empty;

                if (backgroundWriter.textShower != null)
                    backgroundWriter.textShower.text = string.Empty;
                backgroundWriter.transform.parent.gameObject.SetActive(true);
                backgroundWriter.Write();

                mainStory.SetActive(false);

                shownBackgrounds.Add(script.GetInstanceID());

                //backButton.SetActive(false);
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

    /// <summary>
    /// Handles the progressing of the choice follow ups
    /// </summary>
    /// <returns>Whether the script has returned to its normal timeline</returns>
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

    /// <summary>
    /// Called when the player makes a choice
    /// </summary>
    /// <param name="choiceIndex">The index of the choice made</param>
    void ChoiceMade(int choiceIndex)
    {
        choices[0].transform.parent.gameObject.SetActive(false);
        mainStory.SetActive(true);
        backButton.SetActive(false);

        choiceMadeIndex = choiceIndex;

        HandleChoiceProgress();
    }
}
