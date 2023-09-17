/*
 * NovelScript.cs - Maiden's Dissection
 * 
 * Creation Date: 12/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [12/09/2023] - Initial Implementation (C137)
 *  [13/09/2023] - Added choice system for choice based scripts (C137)
 *  [17/09/2023] - Implemented audio system (C137)
 */
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[Serializable]
public struct Choice
{
    /// <summary>
    /// Name of the choice
    /// </summary>
    public string name;

    /// <summary>
    /// Number of followup scripts
    /// </summary>
    public NovelScript[] followup;
}

[Serializable]
public struct Audio
{
    /// <summary>
    /// The voice acting audio to play
    /// </summary>
    public AudioClip voiceActing;

    /// <summary>
    /// The background audio clip to play
    /// </summary>
    public AudioClip background;

    /// <summary>
    /// Whether to loop the background audio
    /// </summary>
    public bool loopBackground;
}

[CreateAssetMenu(fileName = "NovelScript", menuName = "Novel/Script", order = 1)]
public class NovelScript : ScriptableObject
{
    [Header("Background")]
    /// <summary>
    /// The background to be used
    /// </summary>
    public Sprite backgroundSprite;

    /// <summary>
    /// Title to show for the background
    /// </summary>
    public string backgroundTitle;

    /// <summary>
    /// The audio to play if any
    /// </summary>
    public Audio audio;

    [Header("Foreground")]
    /// <summary>
    /// The different choices following this script
    /// </summary>
    public Choice[] choices;

    /// <summary>
    /// The characters to show
    /// </summary>
    public Sprite[] characters;

    [Header("Story Text")]
    /// <summary>
    /// The text to show as the script
    /// </summary>
    [TextArea]
    public string script;

    /// <summary>
    /// The name of the speaker
    /// </summary>
    public string speaker;

    /// <summary>
    /// The font style to use on the script
    /// </summary>
    public FontStyles fontStyle;

    /// <summary>
    /// The speed for the text writer to write the script
    /// </summary>
    public float writerSpeed = 0.02f;

    /// <summary>
    /// The delay before the writer starts writing
    /// </summary>
    public float writerDelay;
}
