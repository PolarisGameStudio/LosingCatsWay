using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_GreenHouse : ViewBehaviour
{
    // ChooseCat
    public GreenHouse_PlaceObject chooseFlowerObject;
    public Transform chooseFlowerContent;
    
    public override void Init()
    {
        base.Init();
        App.model.greenHouse.OnGreenHouseDatasChange += OnGreenHouseDatasChange;
        App.model.greenHouse.OnChooseFlowersChange += OnChooseFlowersChange;
    }

    private void OnGreenHouseDatasChange(object value)
    {
        List<GreenHouseData> greenHouseDatas = (List<GreenHouseData>)value;
        
    }
    
    private void OnChooseFlowersChange(object value)
    {
        for (int i = 0; i < chooseFlowerContent.childCount; i++)
            Destroy(chooseFlowerContent.GetChild(i).gameObject);

        List<CloudLosingCatData> chooseFlowers = (List<CloudLosingCatData>)value;

        for (int i = 0; i < chooseFlowers.Count; i++)
        {
            GreenHouse_PlaceObject tmp = Instantiate(chooseFlowerObject);
            tmp.Active(chooseFlowers[i]);
        }
    }
}