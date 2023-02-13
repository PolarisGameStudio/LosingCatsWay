using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectSystem : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<string, AudioClip> audioDatas = new Dictionary<string, AudioClip>();
    [SerializeField] private AudioSource[] _audioSources;

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

    public void Init()
    {
        App.model.settings.OnSeVolumeChange += OnSetVolumeChange;
    }

    private void SetVolume(float volume)
    {
        for (int i = 0; i < _audioSources.Length; i++)
            _audioSources[i].volume = volume; 
    }

    public void Play(string audioName)
    {
        if (!audioDatas.ContainsKey(audioName))
        {
            Debug.LogError("Audio didn't found.");
            return;
        }

        for (int i = 0; i < _audioSources.Length; i++)
        {
            AudioSource audioSource = _audioSources[i];

            if (!audioSource.isPlaying)
            {
                audioSource.clip = audioDatas[audioName];
                audioSource.Play();
                break;
            }
        }
    }

    public void PlayCatMeow()
    {
        int rand = Random.Range(1, 6);
        App.system.soundEffect.Play($"CatMeow_{rand}");
    }

    private void OnSetVolumeChange(object value)
    {
        float volume = Convert.ToSingle(value);
        SetVolume(volume);
    }
}
