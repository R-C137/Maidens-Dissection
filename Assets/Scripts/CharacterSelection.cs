/*
 * CharacterSelection.cs - Maiden's Dissection
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

public class CharacterSelection : Singleton<CharacterSelection>
{
    /// <summary>
    /// Reference to the showers of the selected bookmark sprite
    /// </summary>
    public Image[] selected;

    /// <summary>
    /// Reference to the showers of the normal bookmark sprite
    /// </summary>
    public Image[] normal;

    /// <summary>
    /// Reference to the handler of the characters' descriptions
    /// </summary>
    public CharacterDescription[] characterDescriptions;

    /// <summary>
    /// Reference to the character selection game object
    /// </summary>
    public GameObject characterSelection;

    /// <summary>
    /// Reference to the chapter menu game object
    /// </summary>
    public GameObject chapterMenu;

    private void Start()
    {
        Select(0);
    }

    /// <summary>
    /// Select the bookmark
    /// </summary>
    /// <param name="index">The index of the selected bookmark</param>
    public void Select(int index)
    {
        for (int i = 0; i < selected.Length; i++)
        {
            selected[i].gameObject.SetActive(false);
            normal[i].gameObject.SetActive(true);
            characterDescriptions[i].gameObject.SetActive(false);
        }

        characterDescriptions[index].gameObject.SetActive(true);
        characterDescriptions[index].ShowIndex(0);

        selected[index].gameObject.SetActive(true);
        normal[index].gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when the back button is pressed
    /// </summary>
    public void Back()
    {
        chapterMenu.SetActive(true);
        characterSelection.SetActive(false);
    }
}
