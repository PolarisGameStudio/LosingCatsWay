using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Diary : ModelBehavior
{
    private int pageIndex;
    private CloudSave_DiaryData selectedDiaryData;
    private CloudLosingCatData losingCatData;
    private int memoryCount;
    private int memoryScore;

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

    public int MemoryCount
    {
        get => memoryCount;
        set
        {
            memoryCount = value;
            OnMemoryCountChange?.Invoke(value);
        }
    }

    public int MemoryScore
    {
        get => memoryScore;
        set
        {
            memoryScore = value;
            OnMemoryScoreChange?.Invoke(value);
        }
    }

    public ValueChange OnPageIndexChange;
    public ValueChange OnSelectedDiaryDataChange;
    public ValueChange OnLosingCatDataChange;
    public ValueChange OnMemoryCountChange;
    public ValueChange OnMemoryScoreChange;
}
