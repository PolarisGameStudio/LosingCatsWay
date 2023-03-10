using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_Settings : ViewBehaviour
{
    [Title("Audio")]
    [SerializeField] private Slider seSlider;
    [SerializeField] private Slider bgmSlider;

    [Title("Language")] [SerializeField] private GameObject[] langMasks;

    [Title("BindAccount")]
    [SerializeField] private GameObject googleLinkButton;
    [SerializeField] private GameObject appleLinkButton;
    [SerializeField] private GameObject googleMask;
    [SerializeField] private GameObject appleMask;

    public override void Init()
    {
        base.Init();
        App.model.settings.OnSeVolumeChange += OnSeVolumeChange;
        App.model.settings.OnBgmVolumeChange += OnBgmVolumeChange;
        App.model.settings.OnLanguageIndexChange += OnLanguageIndexChange;
    }

    public void SetLinkStatus(bool flag)
    {
        googleLinkButton.SetActive(false);
        appleLinkButton.SetActive(false);
        googleMask.SetActive(false);
        appleMask.SetActive(false);

        GameObject linkButton = googleLinkButton;
        GameObject mask = googleMask;
        
#if UNITY_IOS
        linkButton = appleLinkButton;
        mask = appleMask;
#endif
        
        linkButton.SetActive(flag);
        mask.SetActive(!flag);
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
