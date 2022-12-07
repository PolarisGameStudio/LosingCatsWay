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
            OnSeVolumeChange(value);
        }
    }

    public float BgmVolume
    {
        get => bgmVolume;
        set
        {
            bgmVolume = value;
            OnBgmVolumeChange(value);
        }
    }

    public int LanguageIndex
    {
        get => languageIndex;
        set
        {
            languageIndex = value;
            OnLanguageIndexChange(value);
        }
    }

    public ValueChange OnSeVolumeChange;
    public ValueChange OnBgmVolumeChange;
    public ValueChange OnLanguageIndexChange;
}
