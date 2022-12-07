using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Cloister : ModelBehavior
{
    private List<CloudLosingCatData> losingCatDatas;
    private CloudLosingCatData selectedLosingCatData;
    private int selectedIndex = -1;

    public List<CloudLosingCatData> LosingCatDatas
    {
        get => losingCatDatas;
        set
        {
            losingCatDatas = value;
            OnLosingCatDatasChange?.Invoke(value);
        }
    }

    public CloudLosingCatData SelectedLosingCatData
    {
        get => selectedLosingCatData;
        set
        {
            selectedLosingCatData = value;
            OnSelectedLosingCatChange?.Invoke(value);
        }
    }

    public int SelectedIndex
    {
        get => selectedIndex;
        set
        {
            selectedIndex = value;
            //TODO ValueChange
        }
    }

    public ValueChange OnLosingCatDatasChange;
    public ValueChange OnSelectedLosingCatChange;
}
