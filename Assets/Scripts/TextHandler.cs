/*
 * TextHandler.cs - Maiden's Dissection
 * 
 * Creation Date: 21/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [21/09/2023] - Initial Implementation (C137)
 *  [22/09/2023] - Added default name remap for MC (C137)
 *  [23/09/2023] - Added custom font support (C137)
 *  [26/09/2023] - Custom color utility support (C137)
 *  
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextHandler : Singleton<TextHandler>
{
    /// <summary>
    /// Reference to the writer, to set proper values based on the script
    /// </summary>
    public TextWriter storyWriter;

    /// <summary>
    /// The current script being handled
    /// </summary>
    public NovelScript currentScript;

    /// <summary>
    /// The text shower for the speaker
    /// </summary>
    public TextMeshProUGUI speakerShower;

    /// <summary>
    /// The image for the text shower of the speaker
    /// </summary>
    public Image speakerImage;

    /// <summary>
    /// The unique id of the tween used for the fading out of the speaker
    /// </summary>
    public static int speakerTweenFadeOut;

    /// <summary>
    /// The unique id of the tween used for the fading in of the speaker
    /// </summary>
    public static int speakerTweenFadeIn;

    /// <summary>
    /// Used for remapping names if any
    /// </summary>
    public Dictionary<string, string> nameRemap = new()
    {
        {"NARRATION", "Narration"}
    };

    private void Start()
    {
        if (PlayerPrefs.HasKey("general.mc-name"))
            nameRemap.Add("MC", PlayerPrefs.GetString("general.mc-name"));
        else
            nameRemap.Add("MC", "MC");
    }

    private void Update()
    {
        if(currentScript != null)
            HandleColourChanges(currentScript);
    }

    /// <summary>
    /// Handles the showing of the text of the story telling and speakers
    /// </summary>
    /// <param name="script">The script to base the text handling on</param>
    public static void HandleText(NovelScript script)
    {
        singleton.currentScript = script;

        HandleSpeaker(script);

        singleton.storyWriter.textShower.fontStyle = script.fontStyle;

        if(script.fontAsset != null)
            singleton.storyWriter.textShower.font = script.fontAsset;

        singleton.storyWriter.text = script.script;

        if (singleton.storyWriter.textShower != null)
            singleton.storyWriter.textShower.text = string.Empty;

        singleton.storyWriter.speed = script.writerSpeed;
        singleton.storyWriter.delay = script.writerDelay;

        singleton.storyWriter.Write();


    }

    public static void HandleColourChanges(NovelScript script)
    {
        singleton.storyWriter.textShower.color = script.dialogueColour;

        singleton.storyWriter.textShower.ForceMeshUpdate();

        foreach (TMP_LinkInfo link in singleton.storyWriter.textShower.textInfo.linkInfo)
        {
            ColourChange colourChange;

            var tmp = script.colourChanges.Where(c => link.GetLinkID() == c.linkID);
            if (tmp.Any())
                colourChange = tmp.First();
            else
                continue;

            for (int i = link.linkTextfirstCharacterIndex; i < link.linkTextfirstCharacterIndex + link.linkTextLength; i++)
            {
                TMP_CharacterInfo charInfo = singleton.storyWriter.textShower.textInfo.characterInfo[i]; // Gets info on the current character
                int materialIndex = charInfo.materialReferenceIndex; // Gets the index of the current character material

                Color32[] newColors = singleton.storyWriter.textShower.textInfo.meshInfo[materialIndex].colors32;

                // Loop all vertexes of the current characters
                for (int j = 0; j < 4; j++)
                {
                    if (charInfo.character == ' ') continue; // Skips spaces
                    int vertexIndex = charInfo.vertexIndex + j;

                    // Sets the new effects
                    newColors[vertexIndex] = colourChange.color;
                }
            }
        }
        singleton.storyWriter.textShower.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }

    /// <summary>
    /// Handles animations related to the speaker
    /// </summary>
    /// <param name="script">The script to base the animations</param>
    public static void HandleSpeaker(NovelScript script)
    {
        string remappedName = RemapName(script.speaker);

        if (script.speaker == string.Empty || script.speaker == null)
        {
            if (!singleton.speakerImage.gameObject.activeSelf)
                return;

            if (speakerTweenFadeOut != -1)
                LeanTween.cancel(speakerTweenFadeOut);

            speakerTweenFadeOut = LeanTween.value(1, 0, .5f)
                .setOnUpdate(v =>
                {
                    try
                    {
                        singleton.speakerImage.fillAmount = v;
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }).setOnComplete(() => { try { singleton.speakerShower.transform.parent.gameObject.SetActive(false); } catch (Exception) { return; } }).uniqueId;
        }
        else
        {
            if (singleton.speakerShower.text == remappedName && singleton.speakerImage.gameObject.activeSelf)
                return;

            if (speakerTweenFadeIn != -1)
                LeanTween.cancel(speakerTweenFadeIn);

            speakerTweenFadeIn = LeanTween.value(0, 1, .5f)
                .setOnUpdate(v => { try { singleton.speakerImage.fillAmount = v; } catch (Exception) { return; } }).uniqueId;

            singleton.speakerShower.transform.parent.gameObject.SetActive(true);
            singleton.speakerShower.text = remappedName;
        }
    }

    /// <summary>
    /// Remaps speakers' name where appropriate
    /// </summary>
    /// <param name="name">The name to be remapped</param>
    /// <returns>The remapped name</returns>
    static string RemapName(string name)
    {
        if(singleton.nameRemap.ContainsKey(name))
            return singleton.nameRemap[name];

        return name;
    }
}
