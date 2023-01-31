using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_GreenHouse : ModelBehavior
{
    private List<GreenHouseData> greenHouseDatas;

    public List<GreenHouseData> GreenHouseDatas
    {
        get => greenHouseDatas;
        set
        {
            greenHouseDatas = value;
            OnGreenHouseDatasChange.Invoke(value);
        }
    }
    
    public ValueChange OnGreenHouseDatasChange;
}