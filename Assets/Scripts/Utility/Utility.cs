using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utility : Singleton<Utility>
{
    /// <summary>
    /// Changes the scene to the main menu
    /// </summary>
    public void TitleScreen()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
