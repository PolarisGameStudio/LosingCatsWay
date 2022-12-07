using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Diary : ModelBehavior
{
    private int pageIndex;
    private CloudSave_DiaryData selectedDiaryData;

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

    public ValueChange OnPageIndexChange;
    public ValueChange OnSelectedDiaryDataChange;
}
