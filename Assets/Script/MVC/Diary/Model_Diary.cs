using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Diary : ModelBehavior
{
    private int pageIndex;
    private CloudSave_DiaryData selectedDiaryData;
    private CloudLosingCatData losingCatData;

    public int PageIndex
    {
        get => pageIndex;
        set
        {
            pageIndex = value;
            OnPageIndexChange(value);
        }
    }

    public CloudSave_DiaryData SelectedDiaryData
    {
        get => selectedDiaryData;
        set
        {
            selectedDiaryData = value;
            OnSelectedDiaryDataChange(value);
        }
    }

    public CloudLosingCatData LosingCatData
    {
        get => losingCatData;
        set
        {
            losingCatData = value;
            OnLosingCatDataChange(value);
        }
    }

    public ValueChange OnPageIndexChange;
    public ValueChange OnSelectedDiaryDataChange;
    public ValueChange OnLosingCatDataChange;
}
