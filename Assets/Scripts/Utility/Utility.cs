/*
 * Utility.cs - Maiden's Dissection
 * 
 * Creation Date: 09/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [09/09/2023] - Initial Implementation (C137)
 *  [10/09/2023] - Handling of scene loading with transition (C137)
 *  [12/09/2023] - Show warning only once per start (C137)
 *  [20/09/2023] - Unity editor only screen shots (C137)
 *  [01/09/2023] - Background music support (C137)
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.HDROutputUtils;

public class Utility : Singleton<Utility>
{
    /// <summary>
    /// Image to be used for fading transition
    /// </summary>
    public Image fadingImage;

    /// <summary>
    /// Time for fading in and out
    /// </summary>
    public float fadeTime;

    /// <summary>
    /// Whether the warning at the start has been shown already
    /// </summary>
    public static bool shownWarning = false;

    /// <summary>
    /// The current act
    /// </summary>
    public static int currentAct = 0;

    /// <summary>
    /// Audio source for handling the background music
    /// </summary>
    public AudioSource backgroundAudioSource;

    /// <summary>
    /// Audio clips to play based on the unlocked acts
    /// </summary>
    public AudioClip[] audioClips;

    public int screenShotMultiplier = 1;


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);


        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {

        HandleAudio();
    }

    /// <summary>
    /// Handles all audio related actions for the main menu
    /// </summary>
    void HandleAudio()
    {
        if (backgroundAudioSource != null)
        {
            backgroundAudioSource.clip = PlayerPrefs.GetInt("general.unlocked-acts", 0) == 0 ? audioClips[0] : audioClips[1];
            backgroundAudioSource.Play();
        }
    }

    /// <summary>
    /// Loads the scene with a transition
    /// </summary>
    /// <param name="scene">Scene index to load</param>
    public void LoadScene(int scene)
    {
        fadingImage.gameObject.SetActive(true);

        LeanTween.value(0, 1, fadeTime)
            .setOnUpdate(v => fadingImage.color = new(fadingImage.color.r, fadingImage.color.g, fadingImage.color.b, v))
            .setOnComplete(() =>
            {
                SceneManager.LoadScene(scene);

                LeanTween.value(1, 0, fadeTime)
                    .setOnUpdate(v => fadingImage.color = new(fadingImage.color.r, fadingImage.color.g, fadingImage.color.b, v))
                    .setOnComplete(() => fadingImage.gameObject.SetActive(false));
            });
    }

    /// <summary>
    /// Sets the current act
    /// </summary>
    /// <param name="act">The act to be set</param>
    public void SetAct(int act)
    {

    }

    /// <summary>
    /// Changes the scene to the main menu
    /// </summary>
    public void TitleScreen()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.L))
            StartCoroutine(Screenshot());
#endif
        if(backgroundAudioSource != null)
            backgroundAudioSource.volume = PlayerPrefs.GetFloat("settings.background-music-slider", 1);
    }

    public IEnumerator Screenshot()
    {
        yield return new WaitForEndOfFrame();

        ScreenCapture.CaptureScreenshot("Screenshots/" + Guid.NewGuid().ToString() + ".png", screenShotMultiplier);
    }
}
