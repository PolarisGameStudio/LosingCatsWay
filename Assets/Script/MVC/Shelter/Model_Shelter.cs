using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Shelter : ModelBehavior
{
    private List<CloudCatData> cloudCatDatas;
    private CloudCatData selectedAdoptCloudCatData;
    private int selectedCageIndex;
    private List<Cat> myCats;

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
        set
        {
            selectedCageIndex = value;
        }
    }


    public ValueChange OnCloudCatDatasChange;
    public ValueChange OnSelectedAdoptCloudCatDataChange;
}
