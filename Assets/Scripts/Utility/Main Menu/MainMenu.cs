/*
 * MainMenu.cs - Maiden's Dissection
 * 
 * Creation Date: 09/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [09/09/2023] - Initial Implementation (C137)
 *  [12/09/2023] - Added animation to play button (C137)
 *  [21/09/2023] - Added character name selection (C137)
 *  
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
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
    /// Reference to the settings section
    /// </summary>
    public GameObject settings;

    /// <summary>
    /// Reference to the character name selection section
    /// </summary>
    public GameObject characterNameSelection;

    /// <summary>
    /// Reference to the character name input field
    /// </summary>
    public TMP_InputField characterNameInputField;

    /// <summary>
    /// Called when the play button is pressed
    /// </summary>
    public void Play()
    {
        if (!PlayerPrefs.HasKey("general.mc-name"))
        {
            characterNameSelection.SetActive(true);
            return;
        }

        Utility.singleton.fadingImage.gameObject.SetActive(true);
        LeanTween.value(0, 1, .5f).setOnUpdate((v) =>
        {
            Utility.singleton.fadingImage.color = new Color(Utility.singleton.fadingImage.color.r, Utility.singleton.fadingImage.color.g, Utility.singleton.fadingImage.color.b, v);
        }).setOnComplete(() =>
        {
            mainMenu.SetActive(false);
            characterNameSelection.SetActive(false);
            chapters.SetActive(true);

            LeanTween.value(1, 0, .5f).setOnUpdate((v) =>
            {
                Utility.singleton.fadingImage.color = new Color(Utility.singleton.fadingImage.color.r, Utility.singleton.fadingImage.color.g, Utility.singleton.fadingImage.color.b, v);
            }).setOnComplete(() => Utility.singleton.fadingImage.gameObject.SetActive(false));
        });
    }

    /// <summary>
    /// Updates the main character's name
    /// </summary>
    /// <param name="name">The name to be updated with</param>
    public void UpdateName()
    {
        if (characterNameInputField.text == "")
            return;

        PlayerPrefs.SetString("general.mc-name", characterNameInputField.text);
        Play();
    }

    /// <summary>
    /// Called when the options button is pressed
    /// </summary>
    public void Options()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    /// <summary>
    /// Called when the exit button is pressed
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Called when the discord button(logo) is pressed
    /// </summary>
    public void Discord()
    {
        Application.OpenURL("https://discord.gg/4ZDVgcy9F");
    }
}
