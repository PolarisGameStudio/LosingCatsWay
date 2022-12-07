using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using I2.Loc;

public class Controller_Settings : ControllerBehavior
{
    public void Init()
    {
        LoadSettings();
    }

    public void Open()
    {
        App.view.settings.Open();
    }

    public void Close()
    {
        SaveSettings();
        App.view.settings.Close();
    }

    public void ChangeSoundEffectVolume(float volume)
    {
        App.model.settings.SeVolume = volume;
    }

    public void ChangeBgmVolume(float volume)
    {
        App.model.settings.BgmVolume = volume;
    }

    public void SelectLanguage(int index)
    {
        App.model.settings.LanguageIndex = index;
        //TODO L2
        var languages = LocalizationManager.Sources[0].GetLanguages();
        LocalizationManager.CurrentLanguage = languages[index];
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SeVolume", App.model.settings.SeVolume);
        PlayerPrefs.SetFloat("BgmVolume", App.model.settings.BgmVolume);
        PlayerPrefs.SetInt("LanguageIndex", App.model.settings.LanguageIndex);
    }

    private void LoadSettings()
    {
        if (!PlayerPrefs.HasKey("SeVolume"))
            ChangeSoundEffectVolume(1);
        else
            ChangeSoundEffectVolume(PlayerPrefs.GetFloat("SeVolume"));

        if (!PlayerPrefs.HasKey("BgmVolume"))
            ChangeBgmVolume(1);
        else
            ChangeBgmVolume(PlayerPrefs.GetFloat("BgmVolume"));

        if (!PlayerPrefs.HasKey("LanguageIndex"))
            SelectLanguage(0);
        else
            SelectLanguage(PlayerPrefs.GetInt("LanguageIndex"));
    }
}
