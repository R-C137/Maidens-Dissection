/*
 * CharacterDescription.cs - Maiden's Dissection
 * 
 * Creation Date: 21/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [21/09/2023] - Initial Implementation (C137)
 *  [01/10/2023] - Remap MC's Name (C137)
 *  
 */
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterDescription : MonoBehaviour
{
    /// <summary>
    /// The text shower for the character's description
    /// </summary>
    public TextMeshProUGUI textShower;

    /// <summary>
    /// The descriptions for this character
    /// </summary>
    [TextArea]
    public string[] descriptions;

    /// <summary>
    /// The currently shown index of the description
    /// </summary>
    public int currentIndex = 0;

    /// <summary>
    /// Reference to the right button
    /// </summary>
    public GameObject rightButton;

    /// <summary>
    /// Reference to the right button
    /// </summary>
    public GameObject leftButton;

    /// <summary>
    /// Whether to update the index each frame
    /// </summary>
    public bool updateIndex = false;

    /// <summary>
    /// The color to replace the hexes with
    /// </summary>
    public string colorReplace = "#cc5a63";

    /// <summary>
    /// Displays the current index's description to the screen
    /// </summary>
    /// <param name="index">The index to display</param>
    public void ShowIndex(int index)
    {
        textShower.text = descriptions[index].Replace("MC", PlayerPrefs.GetString("general.mc-name", "MC")).Replace("#F5CECD", colorReplace);

        currentIndex = index;

        rightButton.SetActive(currentIndex < descriptions.Length - 1);

        leftButton.SetActive(currentIndex > 0);
    }

    public void Update()
    {
        if(updateIndex)
            ShowIndex(currentIndex);
    }

    /// <summary>
    /// Moves to the next index
    /// </summary>
    /// <param name="progressRight">Whether to progresss to the right or to the left</param>
    public void NextIndex(bool progressRight)
    {
        ShowIndex(currentIndex + (progressRight ? 1 : -1));
    }
}
