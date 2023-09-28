/*
 * MainMenu.cs - Maiden's Dissection
 * 
 * Creation Date: 28/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [28/09/2023] - Initial Implementation (C137)
 *  
 */using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiImageFade : MonoBehaviour
{
    /// <summary>
    /// The images to apply the fade on
    /// </summary>
    public List<Graphic> graphics;

    /// <summary>
    /// Sets all the images to a color
    /// </summary>
    /// <param name="color">The color to be set to</param>
    public void SetColor(Color color)
    {
        graphics.ForEach(x => x.color = color);
    }

    /// <summary>
    /// Changes the opacity of all the images
    /// </summary>
    /// <param name="opacity">The opacity to be set</param>
    public void SetOpacity(float opacity)
    {
        graphics.ForEach(x => x.color = new(x.color.r, x.color.g, x.color.b, opacity));
    }
}
