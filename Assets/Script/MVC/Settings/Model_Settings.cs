using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Settings : ModelBehavior
{
    private float seVolume;
    private float bgmVolume;

    private int languageIndex;

    public float SeVolume
    {
        get => seVolume;
        set
        {
            seVolume = value;
            OnSeVolumeChange?.Invoke(value);
        }
    }

    public float BgmVolume
    {
        get => bgmVolume;
        set
        {
            bgmVolume = value;
            OnBgmVolumeChange?.Invoke(value);
        }
    }

    public int LanguageIndex
    {
        get => languageIndex;
        set
        {
            languageIndex = value;
            OnLanguageIndexChange?.Invoke(value);
        }
    }

    public ValueChange OnSeVolumeChange;
    public ValueChange OnBgmVolumeChange;
    public ValueChange OnLanguageIndexChange;
}
