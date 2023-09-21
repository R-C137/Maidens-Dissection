/*
 * CharacterHandler.cs - Maiden's Dissection
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
using UnityEngine;
using UnityEngine.UI;

public class CharacterHandler : Singleton<CharacterHandler>
{
    /// <summary>
    /// The images to be used for the showing of characters
    /// </summary>
    public Image[] characterImages;

    /// <summary>
    /// Size multiplier for the speaker sprite
    /// </summary>
    public float speakerSizeMultiplier;

    /// <summary>
    /// The id of the tween used for the fading in of characters
    /// </summary>
    public static int characterFadeInTweenID;

    /// <summary>
    /// The id of the tween used for the fading out of characters
    /// </summary>
    public static int characterFadeOutTweenID;

    /// <summary>
    /// Hides all of the characters' sprite
    /// </summary>
    public static void HideAll()
    {
        foreach(var character in singleton.characterImages)
        {
            character.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Handling the showing, hiding and animation of the characters sprite
    /// </summary>
    /// <param name="script">The script to base the animations on</param>
    public static void ShowCharacters(NovelScript script)
    {
        LeanTween.cancel(characterFadeInTweenID);
        LeanTween.cancel(characterFadeOutTweenID);

        characterFadeInTweenID = LeanTween.value(0, 1, .5f).setOnUpdate(v =>
        {
            try
            {
                for (var i = 0; i < script.characters.Length; i++)
                {
                    if (singleton.characterImages[i].color.a == 1 || script.characters[i].poseChange)
                        continue;

                    singleton.characterImages[i].color = new(singleton.characterImages[i].color.r, singleton.characterImages[i].color.g, singleton.characterImages[i].color.b, v);
                }
            }
            catch (Exception)
            {
                return;
            }
        }).setOnStart(() =>
        {
            for (var i = 0; i < script.characters.Length; i++)
            {
                (singleton.characterImages[i].transform as RectTransform).localScale = 
                    script.characters[i].speaker ?
                        new Vector3(singleton.speakerSizeMultiplier, singleton.speakerSizeMultiplier, 1)
                            : Vector3.one;

                singleton.characterImages[i].gameObject.SetActive(true);
                singleton.characterImages[i].sprite = script.characters[i].sprite;
                singleton.characterImages[i].color = GetColour(script.characters[i], singleton.characterImages[i].color.a); ;
            }
        }).uniqueId;

        characterFadeOutTweenID = LeanTween.value(1, 0, .2f).setOnUpdate(v =>
        {
            try
            {
                for (var i = script.characters.Length; i < singleton.characterImages.Length; i++)
                {
                    singleton.characterImages[i].color = new(singleton.characterImages[i].color.r, singleton.characterImages[i].color.g, singleton.characterImages[i].color.b, v);
                }
            }
            catch (Exception)
            {
                return;
            }
        }).setOnComplete(() =>
        {
            try
            {
                for (var i = script.characters.Length; i < singleton.characterImages.Length; i++)
                {
                    singleton.characterImages[i].gameObject.SetActive(false);
                    singleton.characterImages[i].sprite = null;
                }
            }
            catch (Exception)
            {
                return;
            }
        }).uniqueId;
    }

    
    /// <summary>
    /// Returns the color to tint a character's sprite with
    /// </summary>
    /// <param name="character">The chracter to base the tinting on</param>
    /// <param name="alpha">The alpha of the returned colour</param>
    /// <returns>The colour to tint the character with</returns>
    public static Color GetColour(Character character, float alpha = 0)
    {
        if (character.useCustomColor)
            return character.customColor;
        switch (character.feeling)
        {
            case FeelingColour.Neutral:
                return new Color(1, 1, 1, alpha);

            case FeelingColour.Angry:
                return Color.red;

            case FeelingColour.Embarassed:
                return new Color(0.5019607843f, 0, 0.5019607843f, alpha); //purple

            case FeelingColour.Afraid:
                return new Color(0, 0, 1, alpha);

            case FeelingColour.Sad:
                return new Color(0, 0, 1, alpha);

            default:
                return new Color(1, 1, 1, alpha);
        }
    }
}
