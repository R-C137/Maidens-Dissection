/*
 * AudioHandler.cs - Maiden's Dissection
 * 
 * Creation Date: 17/09/2023
 * Authors: C137
 * Original: C137
 * 
 * Changes: 
 *  [17/09/2023] - Initial Implementation (C137)
 *  
 */using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(singleton.voiceActingAudioSource.clip != null && singleton.voiceActingAudioSource.isPlaying)
        {
            if(voiceActingTween != -1)
                LeanTween.cancel(voiceActingTween);

            var tween = LeanTween.value(1, 0, .2f)
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

        PlayAudio(singleton.voiceActingAudioSource, clip);
    }
    
    /// <summary>
    /// Play background music related audio
    /// </summary>
    /// <param name="clip">The audio clip to play</param>
    /// <param name="loop">Whether the loop the clip</param>
    public static void PlayBackground(AudioClip clip, bool loop = true)
    {
        if (singleton.backgroundAudioSource.clip != null && singleton.backgroundAudioSource.isPlaying)
        {
            if (singleton.backgroundAudioSource.clip == clip)
                return;

            if (backgroundFadeOutTween != -1)
                LeanTween.cancel(backgroundFadeOutTween);

            var fadeOutTween = LeanTween.value(1, 0, 1.5f)
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

            var fadeInTween = LeanTween.value(0, 1, 1.5f).setOnUpdate(v => singleton.backgroundAudioSource.volume = v);

            singleton.backgroundAudioSource.loop = loop;

            backgroundFadeInTween = fadeInTween.uniqueId;
        }
    }
    static void PlayAudio(AudioSource source, AudioClip clip, bool setVolume = true)
    {
        if(setVolume)
            source.volume = 1;

        source.clip = clip;
        source.Play();
    }

    private void OnDisable()
    {
        LeanTween.cancel(backgroundFadeInTween);
        LeanTween.cancel(backgroundFadeOutTween);
    }
}
