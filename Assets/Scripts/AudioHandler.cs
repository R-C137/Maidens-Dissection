/*
 * AudioHandler.cs - Maiden's Dissection
 * 
 * Creation Date: 17/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [17/09/2023] - Initial Implementation (C137)
 *  [25/09/2023] - SFX support (C137)
 *  [26/09/2023] - SFX no longer carry over scripts + Added sfx audio slider (C137)
 *  [30/09/2023] - Added support for settings (C137)
 *  [01/09/2023] - Improved support for settings (C137)
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class AudioHandler : Singleton<AudioHandler>
{
    /// <summary>
    /// Reference to the audio source used for voice acting
    /// </summary>
    public AudioSource voiceActingAudioSource;

    /// <summary>
    /// Reference to the audio source used for background music
    /// </summary>
    public AudioSource backgroundAudioSource;

    /// <summary>
    /// Audio sources handling the sfx
    /// </summary>
    public List<AudioSource> sfxAudioSources;

    /// <summary>
    /// Parent for the dynamic sfx audio sources
    /// </summary>
    public Transform sfxAudioParent;

    /// <summary>
    /// The id of the tween for the voice acting
    /// </summary>
    public static int voiceActingTween = -1;

    /// <summary>
    /// The id of the fade out tween for the background music
    /// </summary>
    public static int backgroundFadeOutTween = -1;

    /// <summary>
    /// The id of the fade int tween for the background music
    /// </summary>
    public static int backgroundFadeInTween = -1;

    /// <summary>
    /// Plays voice acting related audio
    /// </summary>
    /// <param name="clip">The audio clip to play</param>
    public static void PlayVoiceActing(AudioClip clip)
    {
        if (PlayerPrefs.GetInt("settings.voice-over", 1) == 0)
            return;

        if (singleton.voiceActingAudioSource.clip != null && singleton.voiceActingAudioSource.isPlaying)
        {
            if(voiceActingTween != -1)
                LeanTween.cancel(voiceActingTween);

            var tween = LeanTween.value(1 * PlayerPrefs.GetFloat("settings.voice-slider", 1), 0, .2f)
                .setOnUpdate((v) => singleton.voiceActingAudioSource.volume = v)
                .setOnComplete(() => 
                {
                    singleton.voiceActingAudioSource.Stop();
                    if(clip != null)
                        PlayAudio(singleton.voiceActingAudioSource, clip);
                    else
                        singleton.voiceActingAudioSource.clip = null;
                });

            voiceActingTween = tween.uniqueId;
            return;
        }

        PlayAudio(singleton.voiceActingAudioSource, clip, true, 1 * PlayerPrefs.GetFloat("settings.voice-slider", 1));
    }
    
    /// <summary>
    /// Play background music related audio
    /// </summary>
    /// <param name="clip">The audio clip to play</param>
    /// <param name="loop">Whether the loop the clip</param>
    public static void PlayBackground(AudioClip clip, bool loop = true)
    {
        if (PlayerPrefs.GetInt("settings.bg-music", 1) == 0)
            return;

        if (singleton.backgroundAudioSource.clip != null && singleton.backgroundAudioSource.isPlaying)
        {
            if (singleton.backgroundAudioSource.clip == clip)
                return;

            if (backgroundFadeOutTween != -1)
                LeanTween.cancel(backgroundFadeOutTween);

            var fadeOutTween = LeanTween.value(1 * PlayerPrefs.GetFloat("settings.background-music-slider", 1), 0, 1.5f)
                .setOnUpdate(v => singleton.backgroundAudioSource.volume = v)
                .setOnComplete(() =>
                {
                    singleton.backgroundAudioSource.Stop();
                    if (clip != null)
                    {
                        PlayAudio(singleton.backgroundAudioSource, clip);
                        DoFadeIn();
                    }
                    else
                        singleton.backgroundAudioSource.clip = null;
                });

            backgroundFadeOutTween = fadeOutTween.uniqueId;
            return;
        }

        PlayAudio(singleton.backgroundAudioSource, clip, false);
        DoFadeIn();

        void DoFadeIn()
        {
            if (backgroundFadeInTween != -1)
                LeanTween.cancel(backgroundFadeInTween);

            var fadeInTween = LeanTween.value(0, 1 * PlayerPrefs.GetFloat("settings.background-music-slider", 1), 1.5f).setOnUpdate(v => singleton.backgroundAudioSource.volume = v);

            singleton.backgroundAudioSource.loop = loop;

            backgroundFadeInTween = fadeInTween.uniqueId;
        }
    }

    public static void PlaySFX(SFXAudio[] sfx)
    {
        if (PlayerPrefs.GetInt("settings.sfx", 1) == 0)
            return;

        if (!sfx.Any())
        {
            foreach(var audioSource in singleton.sfxAudioSources)
            {
                audioSource.Stop();
                audioSource.clip = null;
            }
            return;
        }

        if(sfx.Length > singleton.sfxAudioSources.Count)
        {
            AddSFXSources(singleton.sfxAudioSources.Count - sfx.Length);
        }


        for (int i = 0; i < sfx.Length; i++)
        {
            PlayAudio(singleton.sfxAudioSources[i], sfx[i].clip, true, sfx[i].volume * PlayerPrefs.GetFloat("settings.sfx-slider", 1));
        }
    }

    public static void AddSFXSources(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            singleton.sfxAudioSources.Add(singleton.sfxAudioParent.AddComponent<AudioSource>());
        }
    }

    static void PlayAudio(AudioSource source, AudioClip clip, bool setVolume = true, float volume = 1)
    {
        if(setVolume)
            source.volume = volume;

        source.clip = clip;
        source.Play();
    }

    private void OnDisable()
    {
        LeanTween.cancel(backgroundFadeInTween);
        LeanTween.cancel(backgroundFadeOutTween);
    }
}
