using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_GreenHouse : ViewBehaviour
{
    public override void Init()
    {
        base.Init();
        App.model.greenHouse.OnGreenHouseDatasChange += OnGreenHouseDatasChange;
    }

    private void OnGreenHouseDatasChange(object value)
    {
        List<GreenHouseData> greenHouseDatas = (List<GreenHouseData>)value;
        
    }
}