/*
 * Chapters.cs - Maiden's Dissection
 * 
 * Creation Date: 09/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [09/09/2023] - Initial Implementation (C137)
 *  
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Chapters : MonoBehaviour
{
    /// <summary>
    /// Reference to the main menu section
    /// </summary>
    public GameObject mainMenu;

    /// <summary>
    /// Reference to the chapters section
    /// </summary>
    public GameObject chapters;

    /// <summary>
    /// A collection of the different acts
    /// </summary>
    public Button[] acts;

    /// <summary>
    /// The current act reached
    /// </summary>
    public int currentAct;

    private void Start()
    {
        currentAct = PlayerPrefs.GetInt("general.acts", 0);

        if(acts.Any())
            for (int i = currentAct + 1; i < acts.Length; i++)
            {
                acts[i].enabled = false;
                Destroy(acts[i].GetComponent<HoverAnimation>());
                acts[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.grey;
            }
    }

    /// <summary>
    /// Called when the resumed button is pressed
    /// </summary>
    public void Resume()
    {
        PlayAct(currentAct);
    }

    /// <summary>
    /// Plays the act by loading the scene
    /// </summary>
    /// <param name="act">Act to be played</param>
    public void PlayAct(int act)
    {
        Utility.singleton.LoadScene(act + 1);
    }

    /// <summary>
    /// Called when the back button is pressed
    /// </summary>
    public void Back()
    {
        mainMenu.SetActive(true);
        chapters.SetActive(false);
    }
}
