/*
 * NovelScript.cs - Maiden's Dissection
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
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NovelScript", menuName = "Novel/Script", order = 1)]
public class NovelScript : ScriptableObject
{
    /// <summary>
    /// The background to be used
    /// </summary>
    public Sprite background;

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
    /// The speed for the text writer to write the script
    /// </summary>
    public float writerSpeed = 0.02f;

    /// <summary>
    /// The delay before the writer starts writing
    /// </summary>
    public float writerDelay;
}
