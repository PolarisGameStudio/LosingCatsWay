using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Events : ModelBehavior
{
    private int selectIndex = -1;

    public int SelectIndex
    {
        get => selectIndex;
        set
        {
            selectIndex = value;
            OnSelectIndexChange(value);
        }
    }

    public ValueChange OnSelectIndexChange;
}
