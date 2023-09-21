/*
 * TextHandler.cs - Maiden's Dissection
 * 
 * Creation Date: 21/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [21/09/2023] - Initial Implementation (C137)
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHandler : Singleton<TextHandler>
{
    /// <summary>
    /// Reference to the writer, to set proper values based on the script
    /// </summary>
    public TextWriter storyWriter;

    /// <summary>
    /// Used for remapping names if any
    /// </summary>
    public Dictionary<string, string> nameRemap = new();

    public static void HandleText(NovelScript script)
    {
        singleton.storyWriter.text = script.script;

        singleton.storyWriter.delay = script.writerDelay;
        singleton.storyWriter.speed = script.writerSpeed;

        singleton.storyWriter.Write();
    }
}
