/*
 * GameManager.cs - Maiden's Dissection
 * 
 * Creation Date: 10/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [10/09/2023] - Initial Implementation (C137)
 *  
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// By how much should the text size increase on hover
    /// </summary>
    public float sizeMultiplier = 1.2f;

    /// <summary>
    /// The text to animate
    /// </summary>
    public TextMeshProUGUI text;

    private void Start()
    {
        text = text == null ? transform.GetChild(0).GetComponent<TextMeshProUGUI>() : text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontSize = text.fontSize * sizeMultiplier;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        text.fontSize = text.fontSize / sizeMultiplier;
    }

}
