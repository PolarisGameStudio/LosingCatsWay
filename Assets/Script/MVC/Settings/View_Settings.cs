using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class View_Settings : ViewBehaviour
{
    [Title("Audio")]
    [SerializeField] private Slider seSlider;
    [SerializeField] private Slider bgmSlider;

    [Title("Language")] [SerializeField] private GameObject[] langMasks;

    public override void Init()
    {
        base.Init();
        App.model.settings.OnSeVolumeChange += OnSeVolumeChange;
        App.model.settings.OnBgmVolumeChange += OnBgmVolumeChange;
        App.model.settings.OnLanguageIndexChange += OnLanguageIndexChange;
    }

    private void OnSeVolumeChange(object value)
    {
        float volume = Convert.ToSingle(value);
        seSlider.value = volume;
    }

    private void OnBgmVolumeChange(object value)
    {
        float volume = Convert.ToSingle(value);
        bgmSlider.value = volume;
    }

    private void OnLanguageIndexChange(object value)
    {
        int index = Convert.ToInt32(value);

        for (int i = 0; i < langMasks.Length; i++)
        {
            if (i == index)
                langMasks[i].SetActive(true);
            else
                langMasks[i].SetActive(false);
        }
    }
}
