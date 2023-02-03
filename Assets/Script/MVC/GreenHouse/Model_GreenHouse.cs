using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_GreenHouse : ModelBehavior
{
    private List<GreenHouseData> greenHouseDatas;
    private List<CloudLosingCatData> chooseFlowers;

    public int selectPositionIndex;
    
    public List<GreenHouseData> GreenHouseDatas
    {
        get => greenHouseDatas;
        set => greenHouseDatas = value;
    }

    public List<CloudLosingCatData> ChooseFlowers
    {
        get => chooseFlowers;
        set
        {
            chooseFlowers = value;
            OnChooseFlowersChange.Invoke(value);
        }
    }

    public ValueChange OnGreenHouseDatasChange;
    public ValueChange OnChooseFlowersChange;
}