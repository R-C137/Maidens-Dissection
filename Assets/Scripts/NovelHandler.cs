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
    /// The image of the different available character shower
    /// </summary>
    public Image[] characters;

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
        if(saveProgress)
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

        if (currentScript >= scripts.Length)
            return;

        if(saveProgress)
            PlayerPrefs.SetInt($"general.act{act}.scriptpos", currentScript);

        NovelScript script = scripts[currentScript];

        //Handle background
        background.sprite = script.background == null ? background.sprite : script.background;

        if(script.characters.Length != 0 && script.characters.Length <= characters.Length)
        {
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

        //Handle text showing
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
}
