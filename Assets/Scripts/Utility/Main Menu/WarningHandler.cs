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
    /// The different text of the warning to be faded out
    /// </summary>
    public TextMeshProUGUI[] warningTexts;

    /// <summary>
    /// The background of the warning to be faded
    /// </summary>
    public Image warningBackground;

    /// <summary>
    /// The backdrop of the warning to be faded
    /// </summary>
    public Image warningBackdrop;

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
//#if UNITY_EDITOR
        //Destroy(gameObject);
//#else
        HandleFading();
//#endif
    }

    void HandleFading()
    {
        if (Utility.shownWarning)
            return;

        Utility.shownWarning = true;

        transform.GetChild(0).gameObject.SetActive(true);

        LeanTween.value(1, 0, fadeTime).setOnUpdate((v) =>
        {
            foreach (var text in warningTexts)
            {
                text.color = new(text.color.r, text.color.g, text.color.b, v);
                warningBackground.color = new(warningBackground.color.r, warningBackground.color.g, warningBackground.color.b, v);
            }
        }).setDelay(fadeDelay).setOnComplete(() => Destroy(gameObject));

        LeanTween.value(warningBackdrop.color.a, 0, fadeTime).setOnUpdate((v) =>
        {
            warningBackdrop.color = new(warningBackdrop.color.r, warningBackdrop.color.g, warningBackdrop.color.b, v);
        }).setDelay(fadeDelay);
    }
}
