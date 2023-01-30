using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BgmSystem : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<string, AudioClip> audioDatas = new Dictionary<string, AudioClip>();
    [SerializeField] private AudioSource audioSource;

    AudioClip previousClip;
    float previousTime;
    float localVolume;

    #region MVC
    
    private MyApplication app;

    protected MyApplication App
    {
        get
        {
            if (app == null)
            {
                app = FindObjectOfType<MyApplication>();
            }

            return app;
        }
    }

    #endregion

    public void Init()
    {
        App.model.settings.OnBgmVolumeChange += OnBgmVolumeChange;
        // Play("Lobby");
    }

    private void SetVolume(float volume)
    {
        audioSource.volume = volume;
        localVolume = volume;
    }

    public void Play(string audioName, bool trackPreviousTime = false)
    {
        AudioClip clip = audioDatas[audioName];
        if (clip == audioSource.clip) return;
        
        if (audioSource.isPlaying)
        {
            previousClip = audioSource.clip;
            if (trackPreviousTime) previousTime = audioSource.time;
            audioSource.Stop();
        }
        
        if (!audioDatas.ContainsKey(audioName))
        {
            Debug.LogError("Audio didn't found.");
            return;
        }

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayPrevious()
    {
        if (previousClip == null)
            return;

        audioSource.clip = previousClip;
        audioSource.time = previousTime;
        audioSource.Play();

        previousClip = null;
        previousTime = 0f;
    }

    public BgmSystem FadeOut()
    {
        audioSource.DOFade(0, 1f);
        return this;
    }

    public BgmSystem FadeIn()
    {
        audioSource.DOFade(localVolume, 1f);
        return this;
    }

    #region ValueChange

    private void OnBgmVolumeChange(object value)
    {
        float volume = Convert.ToSingle(value);
        SetVolume(volume);
    }

    #endregion
}
