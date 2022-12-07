using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Entrance : ModelBehavior
{
    private List<Cat> cats = new List<Cat>();
    private Cat deadCat;
    private int openType = 0; //0:正常 1:死

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

    /// <summary>
    /// 開啓玄關的類型
    /// 0:正常
    /// 1:死
    /// </summary>
    public int OpenType
    {
        get => openType;
        set
        {
            openType = value;
            OnOpenTypeChange(value);
        }
    }

    public ValueChange OnCatsChange;
    public ValueChange OnDeadCatChange;
    public ValueChange OnOpenTypeChange;
}
