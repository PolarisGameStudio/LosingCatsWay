using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_GreenHouse : ViewBehaviour
{
    // GreenHouseDatas
    public GreenHouse_PlaceObject[] places;

    // ChooseCat
    public GreenHouse_PlaceObject chooseFlowerObject;
    public Transform chooseFlowerContent;
    
    public override void Init()
    {
        base.Init();
        App.model.greenHouse.OnChooseFlowersChange += OnChooseFlowersChange;
    }

    public void RefreshGreenHousePlace()
    {
        for (int i = 0; i < places.Length; i++)
        {
            places[i].Close();
        }
        
        List<GreenHouseData> greenHouseDatas = App.model.greenHouse.GreenHouseDatas;

        for (int i = 0; i < greenHouseDatas.Count; i++)
        {
            var data = App.model.cloister.LosingCatDatas.Find(x => x.CatData.CatId == greenHouseDatas[i].FlowerID);
            places[greenHouseDatas[i].Position].Active(data);
        }
    }

    private void OnChooseFlowersChange(object value)
    {
        for (int i = 0; i < chooseFlowerContent.childCount; i++)
            Destroy(chooseFlowerContent.GetChild(i).gameObject);

        List<CloudLosingCatData> chooseFlowers = (List<CloudLosingCatData>)value;

        for (int i = 0; i < chooseFlowers.Count; i++)
        {
            GreenHouse_PlaceObject tmp = Instantiate(chooseFlowerObject, chooseFlowerContent);
            tmp.ActiveByChooseCat(chooseFlowers[i], i);
        }
    }
}