/* CheckboxOption.cs - Imported
 * 
 * Creation Date: 09/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *      [09/09/2023] - Initial implementation (C137)
 */
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheckboxOption : MonoBehaviour
{
    /// <summary>
    /// The id of the setting of this checkbox
    /// </summary>
    public string settingID;

    /// <summary>
    /// Image responsible to display the sprite
    /// </summary>
    public Image spriteShower;

    /// <summary>
    /// Sprite to use when the checkbox is checked
    /// </summary>
    public Sprite check;

    /// <summary>
    /// Sprite to use when the checkbox is unchecked
    /// </summary>
    public Sprite uncheck;

    /// <summary>
    /// The current state of the checkbox
    /// </summary>
    public bool state = false;


    public void Start()
    {
        state = PlayerPrefs.GetInt(settingID, 1) == 1;

        UpdateSprite();
    }

    /// <summary>
    /// Updates the state of the checkbox to the next one
    /// </summary>
    public void UpdateState()
    {
        state = !state;

        PlayerPrefs.SetInt(settingID, state == true ? 1 : 0);

        UpdateSprite();
    }

    /// <summary>
    /// Updates the sprite to reflect the change
    /// </summary>
    public void UpdateSprite()
    {
        spriteShower.sprite = state == true ? check : uncheck;
    }
}
