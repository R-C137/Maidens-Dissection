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
 *  [23/09/2023] - Added custom font support (C137)
 *  [25/09/2023] - Added sfx audio support + Custom speaker size support (C137)
 *  [26/09/2023] - Custom color utility support + Speaker name colour support + Improved display in the inspector (C137)
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

    /// <summary>
    /// The sfx audios to play
    /// </summary>
    [Space(10)]
    public SFXAudio[] sfx;

}

//Made it a class rather than a struct to set default values
[Serializable]
public class SFXAudio
{
    /// <summary>
    /// The sfx clip to play
    /// </summary>
    public AudioClip clip;

    /// <summary>
    /// The volume of the sfx clip to play
    /// </summary>
    [Range(0f, 1f)]
    public float volume = 1f;
}

public enum FeelingColour
{
    Neutral,
    Angry,
    Embarassed,
    Afraid,
    Sad,
}

[Serializable]
public struct Character
{
    /// <summary>
    /// The sprite related to the character
    /// </summary>
    public Sprite sprite;

    /// <summary>
    /// The color to be applied to the sprite based on the feeling
    /// </summary>
    public FeelingColour feeling;

    /// <summary>
    /// The custom color of the feeling (if any)
    /// </summary>
    public Color customColor;

    /// <summary>
    /// Whether to use the custom color or not
    /// </summary>
    public bool useCustomColor;

    /// <summary>
    /// Whether it is only a pose change (Set to true for fading)
    /// </summary>
    public bool poseChange;

    /// <summary>
    /// Whether this is the sprite of the current speaker
    /// </summary>
    public bool speaker;

    /// <summary>
    /// Size multiplier for a speaker
    /// </summary>
    public float speakerSizeMultiplier;
}

[Serializable]
public struct ColourChange
{
    /// <summary>
    /// The name of the link
    /// </summary>
    public string linkID;

    /// <summary>
    /// The colour to change to
    /// </summary>
    public Color color;
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
    public Character[] characters;

    [Header("Story Text")]
    /// <summary>
    /// The text to show as the script
    /// </summary>
    [TextArea]
    public string script;

    /// <summary>
    /// The font style to use on the script
    /// </summary>
    public FontStyles fontStyle;

    /// <summary>
    /// To font to be used
    /// </summary>
    public TMP_FontAsset fontAsset;

    /// <summary>
    /// Changes the colour of the whole dialogue
    /// </summary>
    public Color dialogueColour = Color.white;

    /// <summary>
    /// The name of the speaker
    /// </summary>
    [Header("Speaker Settings")]
    public string speaker;

    /// <summary>
    /// The colour to use for the speaker text
    /// </summary>
    public Color speakerColour = Color.white;

    /// <summary>
    /// The speed for the text writer to write the script
    /// </summary>
    [Header("Writing Effect")]
    public float writerSpeed = 0.02f;

    /// <summary>
    /// The delay before the writer starts writing
    /// </summary>
    public float writerDelay;

    /// <summary>
    /// Handles the changing of colour for links;
    /// </summary>
    [Header("Editor Only")]
    public ColourChange[] colourChanges;
}
