/*
 * Chapters.cs - Maiden's Dissection
 * 
 * Creation Date: 09/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [09/09/2023] - Initial Implementation (C137)
 *  [12/09/2023] - Added animation to back button (C137)
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
        int unlockedActs = PlayerPrefs.GetInt("general.unlocked-acts", 0);

        if(acts.Any())
            for (int i = unlockedActs + 1; i < acts.Length; i++)
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
        currentAct = PlayerPrefs.GetInt("general.acts", 0);

        if(currentAct > 1)
        {
            Utility.currentAct = 1;
            PlayerPrefs.SetInt($"general.act1.scriptpos", -1);
        }

        Utility.singleton.LoadScene(1);
    }

    /// <summary>
    /// Plays the act by loading the scene
    /// </summary>
    /// <param name="act">Act to be played</param>
    public void PlayAct(int act)
    {
        Utility.singleton.LoadScene(1);

        Utility.currentAct = act;

        PlayerPrefs.SetInt($"general.act{act}.scriptpos", -1);
    }

    /// <summary>
    /// Called when the back button is pressed
    /// </summary>
    public void Back()
    {
        Utility.singleton.fadingImage.gameObject.SetActive(true);
        LeanTween.value(0, 1, .5f).setOnUpdate((v) =>
        {
            Utility.singleton.fadingImage.color = new Color(Utility.singleton.fadingImage.color.r, Utility.singleton.fadingImage.color.g, Utility.singleton.fadingImage.color.b, v);
        }).setOnComplete(() =>
        {
            mainMenu.SetActive(true);
            chapters.SetActive(false);

            LeanTween.value(1, 0, .5f).setOnUpdate((v) =>
            {
                Utility.singleton.fadingImage.color = new Color(Utility.singleton.fadingImage.color.r, Utility.singleton.fadingImage.color.g, Utility.singleton.fadingImage.color.b, v);
            }).setOnComplete(() => Utility.singleton.fadingImage.gameObject.SetActive(false));
        });
    }
}
