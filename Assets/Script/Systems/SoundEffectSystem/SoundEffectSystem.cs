using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectSystem : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<string, AudioClip> audioDatas = new Dictionary<string, AudioClip>();
    [SerializeField] private AudioSource audioSource;

    #region MVC

    public MyApplication App
    {
        get => FindObjectOfType<MyApplication>();
    }

    #endregion

    public void Init()
    {
        App.model.settings.OnSeVolumeChange += OnSeVolumeChange;
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void Play(string audioName)
    {
        if (audioSource.isPlaying) audioSource.Stop();

        if (!audioDatas.ContainsKey(audioName))
        {
            Debug.LogError("Audio didn't found.");
            return;
        }

        audioSource.clip = audioDatas[audioName];
        audioSource.Play();
    }

    private void OnSeVolumeChange(object value)
    {
        float volume = Convert.ToSingle(value);
        SetVolume(volume);
    }
}
