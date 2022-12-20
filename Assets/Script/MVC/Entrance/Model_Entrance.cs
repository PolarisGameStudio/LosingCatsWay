using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Entrance : ModelBehavior
{
    private List<Cat> cats;
    private Cat deadCat;
    private int openType = 0; //0:正常 1:死
    private int selectedDiaryIndex;
    private List<CloudLosingCatData> losingCatDatas;
    private List<CloudLosingCatData> sortedLosingCatDatas;

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

    public List<CloudLosingCatData> LosingCatDatas
    {
        get => losingCatDatas;
        set => losingCatDatas = value;
    }

    public List<CloudLosingCatData> SortedLosingCatDatas
    {
        get => sortedLosingCatDatas;
        set
        {
            sortedLosingCatDatas = value;
            OnSortedLosingCatDatasChange(value);
        }
    }

    public ValueChange OnCatsChange;
    public ValueChange OnDeadCatChange;
    public ValueChange OnOpenTypeChange;
    // public ValueChange OnAllDeadCatsChange;
    public ValueChange OnSortedLosingCatDatasChange;
}
