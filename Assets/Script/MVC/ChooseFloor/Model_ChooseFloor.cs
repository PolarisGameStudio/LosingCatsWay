using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_ChooseFloor : ModelBehavior
{
    private int usingFloorIndex = -1;

    public int UsingFloorIndex
    {
        get => usingFloorIndex;
        set
        {
            OnUsingFloorIndexChange(usingFloorIndex, value);
            usingFloorIndex = value;
        }
    }

    public ValueFromToChange OnUsingFloorIndexChange;
}
