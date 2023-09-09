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
    /// The checkbox of the voiceover setting
    /// </summary>
    public Slider voiceOverSlider;

    /// <summary>
    /// Slider for the master volume
    /// </summary>
    public Slider masterVolumeSlider;

    private void Start()
    {
        voiceOverSlider.value = PlayerPrefs.GetFloat("settings.voiceover", 1);

        masterVolumeSlider.value = PlayerPrefs.GetFloat("settings.mastervolume", 1);
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

    public void VoiceOverUpdated(float value)
    {
        PlayerPrefs.SetFloat("settings.voiceover", value);
    }
}
