/*
 * Utility.cs - Maiden's Dissection
 * 
 * Creation Date: 09/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [09/09/2023] - Initial Implementation (C137)
 *  [10/09/2023] - Handling of scene loading with transition
 *  
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.HDROutputUtils;

public class Utility : Singleton<Utility>
{
    /// <summary>
    /// Image to be used for fading transition
    /// </summary>
    public Image fadingImage;

    /// <summary>
    /// Time for fading in and out
    /// </summary>
    public float fadeTime;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// Loads the scene with a transition
    /// </summary>
    /// <param name="scene">Scene index to load</param>
    public void LoadScene(int scene)
    {
        fadingImage.gameObject.SetActive(true);

        LeanTween.value(0, 1, fadeTime)
            .setOnUpdate(v => fadingImage.color = new(fadingImage.color.r, fadingImage.color.g, fadingImage.color.b, v))
            .setOnComplete(() =>
            {
                SceneManager.LoadScene(scene);

                LeanTween.value(1, 0, fadeTime)
                    .setOnUpdate(v => fadingImage.color = new(fadingImage.color.r, fadingImage.color.g, fadingImage.color.b, v))
                    .setOnComplete(() => fadingImage.gameObject.SetActive(false));
            });
    }

    /// <summary>
    /// Changes the scene to the main menu
    /// </summary>
    public void TitleScreen()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

}
