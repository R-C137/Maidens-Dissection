/*
 * NovelHandler.cs - Maiden's Dissection
 * 
 * Creation Date: 12/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [12/09/2023] - Initial Implementation (C137)
 *  
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NovelHandler : MonoBehaviour
{
    /// <summary>
    /// The different scripts for this act
    /// </summary>
    public NovelScript[] scripts;

    /// <summary>
    /// The current script the player has reached
    /// </summary>
    public int currentScript = -1;

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

    public bool dosaving = false;

    /// <summary>
    /// Reference to the writer, to set proper values based on the script
    /// </summary>
    public TextWriter writer;

    private void Awake()
    {
        if(dosaving)
        currentScript = PlayerPrefs.GetInt($"general.act{act}.scriptpos", -1);
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
        currentScript++;
        if(dosaving)
        PlayerPrefs.SetInt($"general.act{act}.scriptpos", currentScript);

        NovelScript script = scripts[currentScript];

        background.sprite = script.background == null ? background.sprite : script.background;

        speakerShower.text = script.speaker;

        writer.speed = script.writerSpeed;
        writer.delay = script.writerDelay;

        writer.Write(script.script);
    }
}
