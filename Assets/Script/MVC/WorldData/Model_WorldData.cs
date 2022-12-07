using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_WorldData : ModelBehavior
{
    private Cloud_WorldData worldData;

    public Cloud_WorldData WorldData
    {
        get => worldData;
        set
        {
            worldData = value;
            OnWorldDataChange(value);
        }
    }

    public ValueChange OnWorldDataChange;
}
