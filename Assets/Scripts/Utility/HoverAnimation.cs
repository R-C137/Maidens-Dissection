/*
 * GameManager.cs - Maiden's Dissection
 * 
 * Creation Date: 10/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [10/09/2023] - Initial Implementation (C137)
 *  [19/09/2023] - Change colour on hover (C137)
 *  
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// By how much should the text size increase on hover
    /// </summary>
    public float sizeMultiplier = 1.2f;

    /// <summary>
    /// Whether to change colour from white to black on hover
    /// </summary>
    public bool changeColour = true;

    /// <summary>
    /// The text to animate
    /// </summary>
    public TextMeshProUGUI text;

    /// <summary>
    /// Image to show when selected
    /// </summary>
    public Image selectionImage;

    /// <summary>
    /// How long it takes for the selection image to fully fade in
    /// </summary>
    public float selectionShowTime = .5f;

    /// <summary>
    /// Font size before animation. Internal use only
    /// </summary>
    [HideInInspector]
    public float size;

    /// <summary>
    /// Whether the mouse is currently hovering over the ui
    /// </summary>
    [HideInInspector]
    public bool hovering = false;

    private void Start()
    {
        text = text == null ? transform.GetChild(0).GetComponent<TextMeshProUGUI>() : text;
        size = text.fontSize;
    }

    private void Update()
    {
        hovering = true;

        if (Input.GetKeyUp(KeyCode.Mouse0))
            OnPointerExit(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (changeColour)
            text.color = Color.black;
        if (text.enableAutoSizing)
        {
            text.enableAutoSizing = false;
            size = text.fontSize;
        }

        if (text.fontSize != size)
            return;

        text.fontSize = text.fontSize * sizeMultiplier;

        if (selectionImage == null)
            return;

        LeanTween.cancel(gameObject);

        selectionImage.gameObject.SetActive(true);

        LeanTween.value(0, 1, selectionShowTime).setOnUpdate(v => selectionImage.fillAmount = v);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (changeColour)
            text.color = Color.white;

        text.fontSize = size;
        text.enableAutoSizing = true;

        if (selectionImage == null)
            return;

        LeanTween.cancel(gameObject);
        selectionImage.gameObject.SetActive(false);
    }

}
