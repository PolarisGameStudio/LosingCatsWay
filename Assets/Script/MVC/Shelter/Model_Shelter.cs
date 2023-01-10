using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

public class Model_Shelter : ModelBehavior
{
    private List<CloudCatData> cloudCatDatas;
    private CloudCatData selectedAdoptCloudCatData;
    private int selectedCageIndex;
    private List<Cat> myCats;
    private int freeRefresh; //3
    private int adsRefresh; //5
    private DateTime cooldown;

    public List<CloudCatData> CloudCatDatas
    {
        get => cloudCatDatas;
        set
        {
            cloudCatDatas = value;
            OnCloudCatDatasChange(value);
        }
    }

    public CloudCatData SelectedAdoptCloudCatData
    {
        get => selectedAdoptCloudCatData;
        set
        {
            selectedAdoptCloudCatData = value;
            OnSelectedAdoptCloudCatDataChange(value);
        }
    }

    public int SelectedCageIndex
    {
        get => selectedCageIndex;
        set => selectedCageIndex = value;
    }

    public int FreeRefresh
    {
        get => freeRefresh;
        set
        {
            freeRefresh = value;
            OnFreeRefreshChange?.Invoke(value);
        }
    }

    public int AdsRefresh
    {
        get => adsRefresh;
        set
        {
            adsRefresh = value;
            OnAdsRefreshChange?.Invoke(value);
        }
    }

    public DateTime Cooldown
    {
        get => cooldown;
        set
        {
            cooldown = value;
            OnCooldownChange?.Invoke(value);
        }
    }

    public ValueChange OnCloudCatDatasChange;
    public ValueChange OnSelectedAdoptCloudCatDataChange;
    public ValueChange OnFreeRefreshChange;
    public ValueChange OnAdsRefreshChange;
    public ValueChange OnCooldownChange;
}
