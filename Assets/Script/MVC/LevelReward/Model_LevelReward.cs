using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_LevelReward : ModelBehavior
{
    private int maxLevel = 1;

    public int MaxLevel
    {
        get => maxLevel;
        set
        {
            maxLevel = value;
            OnMaxLevelChange?.Invoke(value);
        }
    }

    public ValueChange OnMaxLevelChange;
}
