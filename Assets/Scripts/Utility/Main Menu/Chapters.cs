/*
 * Chapters.cs - Maiden's Dissection
 * 
 * Creation Date: 09/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [09/09/2023] - Initial Implementation (C137)
 *  [12/09/2023] - Added animation to back button (C137)
 *  [30/09/2023] - Improved characters page (C137)
 *  [01/10/2023] - Fix buttons handling with new button system (C13&)
 *  
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Chapters : MonoBehaviour
{
    /// <summary>
    /// Reference to the main menu section
    /// </summary>
    public GameObject mainMenu;

    /// <summary>
    /// Reference to the chapters section
    /// </summary>
    public GameObject chapters;

    /// <summary>
    /// Reference to the tutorial section
    /// </summary>
    public GameObject tutorial;

    /// <summary>
    /// Reference to the text of the characters page
    /// </summary>
    public TextMeshProUGUI charactersText;

    /// <summary>
    /// The button used to access the characters page
    /// </summary>
    public Button charactersButton;

    /// <summary>
    /// The amount of time the tutorial is shown for
    /// </summary>
    public float tutorialShowTime;

    /// <summary>
    /// A collection of the different acts
    /// </summary>
    public ImprovedButton[] acts;

    /// <summary>
    /// The current act reache
    /// </summary>
    public int currentAct;

    private void Start()
    {
        int unlockedActs = PlayerPrefs.GetInt("general.unlocked-acts", 0);

        if (acts.Any())
        {
            for (int i = unlockedActs + 1; i < acts.Length; i++)
            {
                acts[i].enabled = false;
                Destroy(acts[i].GetComponent<HoverAnimation>());
                acts[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.grey;
            }
            if(unlockedActs >= 2)
            {
                charactersText.text = "~ Characters ~";
                charactersButton.enabled = true;
            }
        }
    }

    /// <summary>
    /// Called when the resumed button is pressed
    /// </summary>
    public void Resume()
    {
        ShowTutorial(() =>
        {
            currentAct = PlayerPrefs.GetInt("general.acts", 0);

            if (currentAct > 1)
            {
                Utility.currentAct = 1;
                PlayerPrefs.SetInt($"general.act1.scriptpos", -1);
            }

            Utility.singleton.LoadScene(1);

            PlayerPrefs.SetInt("genereal.first-start", 0);
        });
    }

    /// <summary>
    /// Shows the tutorial 
    /// </summary>
    /// <param name="onFinish">Callback to execute on finish</param>
    public void ShowTutorial(Action onFinish)
    {
        bool firstPlay = PlayerPrefs.GetInt("genereal.first-start", 1) == 1;

        if (firstPlay)
        {
            tutorial.SetActive(true);
            LeanTween.delayedCall(tutorialShowTime, () => onFinish?.Invoke());
        }
        else
            onFinish?.Invoke();
    }

    /// <summary>
    /// Plays the act by loading the scene
    /// </summary>
    /// <param name="act">Act to be played</param>
    public void PlayAct(int act)
    {
        ShowTutorial(() =>
        {
            Utility.singleton.LoadScene(1);

            Utility.currentAct = act;

            PlayerPrefs.SetInt($"general.act{act}.scriptpos", -1);

            PlayerPrefs.SetInt("genereal.first-start", 0);
        });
    }

    /// <summary>
    /// Called when the back button is pressed
    /// </summary>
    public void Back()
    {
        Utility.singleton.fadingImage.gameObject.SetActive(true);
        LeanTween.value(0, 1, .5f).setOnUpdate((v) =>
        {
            Utility.singleton.fadingImage.color = new Color(Utility.singleton.fadingImage.color.r, Utility.singleton.fadingImage.color.g, Utility.singleton.fadingImage.color.b, v);
        }).setOnComplete(() =>
        {
            mainMenu.SetActive(true);
            chapters.SetActive(false);

            LeanTween.value(1, 0, .5f).setOnUpdate((v) =>
            {
                Utility.singleton.fadingImage.color = new Color(Utility.singleton.fadingImage.color.r, Utility.singleton.fadingImage.color.g, Utility.singleton.fadingImage.color.b, v);
            }).setOnComplete(() => Utility.singleton.fadingImage.gameObject.SetActive(false));
        });
    }
}
