/*
 * Settings.cs - Maiden's Dissection
 * 
 * Creation Date: 09/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [09/09/2023] - Initial Implementation (C137)
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
    /// The checkbox of the sounds setting
    /// </summary>
    public Slider soundsSlider;

    /// <summary>
    /// Slider for the master volume
    /// </summary>
    public Slider masterVolumeSlider;

    private void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("settings.mastervolume", 1);

        voiceSlider.value = PlayerPrefs.GetFloat("settings.voice", 1);

        soundsSlider.value = PlayerPrefs.GetFloat("settings.sounds", 1);
    }

    /// <summary>
    /// Called when the back button is pressed
    /// </summary>
    public void Back()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
    }

    public void MasterAudioUpdated(float value)
    {
        PlayerPrefs.SetFloat("settings.mastervolume", value);
    }

    public void VoiceUpdated(float value)
    {
        PlayerPrefs.SetFloat("settings.voice", value);
    }

    public void SoundsUpdated(float value)
    {
        PlayerPrefs.SetFloat("settings.sounds", value);
    }
}
