/*
 * Settings.cs - Maiden's Dissection
 * 
 * Creation Date: 09/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [09/09/2023] - Initial Implementation (C137)
 *  [31/10/2023] - Change the default value for the background audio (C137)
 *  
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    /// <summary>
    /// Reference to the main menu section
    /// </summary>
    public GameObject mainMenu;

    /// <summary>
    /// Reference to the settings section
    /// </summary>
    public GameObject settings;

    /// <summary>
    /// The slider of the voice setting
    /// </summary>
    public Slider voiceSlider;

    /// <summary>
    /// The slider of the sfx setting
    /// </summary>
    public Slider sfxSlider;

    /// <summary>
    /// Slider for the background music
    /// </summary>
    public Slider bgMusic;

    private void Start()
    {
        bgMusic.value = PlayerPrefs.GetFloat("settings.background-music-slider", .5f);

        voiceSlider.value = PlayerPrefs.GetFloat("settings.voice-slider", 1);

        sfxSlider.value = PlayerPrefs.GetFloat("settings.sfx-slider", 1);
    }

    /// <summary>
    /// Called when the back button is pressed
    /// </summary>
    public void Back()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
    }

    public void BackgroundMusicUpdated(float value)
    {
        PlayerPrefs.SetFloat("settings.background-music-slider", value);
    }

    public void VoiceUpdated(float value)
    {
        PlayerPrefs.SetFloat("settings.voice-slider", value);
    }

    public void SFXUpdated(float value)
    {
        PlayerPrefs.SetFloat("settings.sfx-slider", value);
    }
}
