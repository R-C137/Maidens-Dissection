/* Checkbox.cs - Maiden's Dissection
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
using UnityEngine;
using UnityEngine.UI;

public class Checkbox : MonoBehaviour
{
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

    /// <summary>
    /// Callback for when the state changes
    /// </summary>
    public Action<bool> callback;

    private void Start()
    {
        UpdateSprite();
    }

    /// <summary>
    /// Updates the state of the checkbox to the next one
    /// </summary>
    public void UpdateState()
    {
        state = !state;

        UpdateSprite();

        callback?.Invoke(state);
    }

    /// <summary>
    /// Sets the state of the check box
    /// </summary>
    /// <param name="state">The state to be set to</param>
    public void SetState(bool state)
    {
        this.state = state;

        UpdateSprite();

        callback?.Invoke(state);
    }

    /// <summary>
    /// Updates the sprite to reflect the change
    /// </summary>
    public void UpdateSprite()
    {
        spriteShower.sprite = state == true ? check : uncheck;
    }
}
