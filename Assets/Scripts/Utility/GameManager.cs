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
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Loads the title screen. Called when the title screen button is pressed
    /// </summary>
    public void TitleSceen()
    {
        Utility.singleton.LoadScene(0);
    }
}
