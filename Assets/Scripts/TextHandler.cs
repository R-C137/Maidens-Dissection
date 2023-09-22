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
using System;
using System.Collections;
using System.Collections.Generic;
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
    public static Dictionary<string, string> nameRemap = new()
    {
        {"NARRATION", "Narration"}
    };

    private void Start()
    {
        nameRemap.Add("MC", PlayerPrefs.GetString("general.mc-name"));
    }

    /// <summary>
    /// Handles the showing of the text of the story telling and speakers
    /// </summary>
    /// <param name="script">The script to base the text handling on</param>
    public static void HandleText(NovelScript script)
    {
        HandleSpeaker(script);

        singleton.storyWriter.textShower.fontStyle = script.fontStyle;

        singleton.storyWriter.text = script.script;

        if (singleton.storyWriter.textShower != null)
            singleton.storyWriter.textShower.text = string.Empty;

        singleton.storyWriter.speed = script.writerSpeed;
        singleton.storyWriter.delay = script.writerDelay;

        singleton.storyWriter.Write();

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
        if(nameRemap.ContainsKey(name))
            return nameRemap[name];

        return name;
    }
}
