using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Entrance : ModelBehavior
{
    private List<Cat> cats = new List<Cat>();
    private Cat deadCat;
    private int openType = 0; //0:正常 1:死
    private int selectedDiaryIndex;
    private CloudLosingCatData leftCatData, rightCatData, centerCatData;

    public List<Cat> Cats
    {
        get => cats;
        set
        {
            cats = value;
            OnCatsChange(value);
        }
    }

    public Cat DeadCat
    {
        get => deadCat;
        set
        {
            deadCat = value;
            OnDeadCatChange(value);
        }
    }

    /// 開啓玄關的類型
    /// 0:正常
    /// 1:死
    public int OpenType
    {
        get => openType;
        set
        {
            openType = value;
            OnOpenTypeChange(value);
        }
    }

    public int SelectedDiaryIndex
    {
        get => selectedDiaryIndex;
        set => selectedDiaryIndex = value;
    }

    public CloudLosingCatData LeftCatData
    {
        get => leftCatData;
        set => leftCatData = value;
    }

    public CloudLosingCatData RightCatData
    {
        get => rightCatData;
        set => rightCatData = value;
    }

    public CloudLosingCatData CenterCatData
    {
        get => centerCatData;
        set => centerCatData = value;
    }

    public ValueChange OnCatsChange;
    public ValueChange OnDeadCatChange;
    public ValueChange OnOpenTypeChange;
    public ValueChange OnLeftCatDataChange, OnRightCatDataChange, OnCenterCatDataChange;
}
