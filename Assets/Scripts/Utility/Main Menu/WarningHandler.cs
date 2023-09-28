/*
 * GameManager.cs - Maiden's Dissection
 * 
 * Creation Date: 10/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [10/09/2023] - Initial Implementation (C137)
 *  [12/09/2023] - Show warning only in builds + Show warning only once (C137)
 *  [28/09/2023] - Improved fading handling (C137)
 *  
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarningHandler : MonoBehaviour
{
    /// <summary>
    /// Used to fade all the graphics
    /// </summary>
    public MultiImageFade fader;

    /// <summary>
    /// Reference to the backdrop for custom fading
    /// </summary>
    public Image backdrop;

    /// <summary>
    /// The time it takes for the fading of the foreground to occur
    /// </summary>
    public float fadeTime;

    /// <summary>
    /// The delay for the fading of the warning
    /// </summary>
    public float fadeDelay;

    private void Awake()
    {
#if UNITY_EDITOR
      Destroy(gameObject);
#else
      HandleFading();
#endif
    }

    void HandleFading()
    {
        if (Utility.shownWarning)
            return;

        Utility.shownWarning = true;

        transform.GetChild(0).gameObject.SetActive(true);

        LeanTween.value(1, 0, fadeTime).setOnUpdate((v) => fader.SetOpacity(v)).setDelay(fadeDelay).setOnComplete(() => Destroy(gameObject));

        LeanTween.value(backdrop.color.a, 0, fadeTime).setOnUpdate((v) => backdrop.color = new(backdrop.color.r, backdrop.color.g, backdrop.color.b, v)).setDelay(fadeDelay);
    }
}
