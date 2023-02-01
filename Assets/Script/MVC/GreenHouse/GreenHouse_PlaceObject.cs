using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GreenHouse_PlaceObject : MvcBehaviour
{
    public GameObject noneObject;
    public CatFlower flowerObject;
    public GameObject nameObject;
    public TextMeshProUGUI nameText;

    private int _chooseFlowerIndex;
    
    public void Click(int index)
    {
        App.controller.greenHouse.OpenChooseFlower(index);
    }

    public void ChooseFlower()
    {
        
    }

    public void Active(CloudLosingCatData cloudLosingCatData)
    {
        flowerObject.gameObject.SetActive(true);
        noneObject.SetActive(false);
        
        flowerObject.ChangeSkin(cloudLosingCatData);
    }

    public void ActiveByChooseCat(CloudLosingCatData cloudLosingCatData, int index)
    {
        flowerObject.ChangeSkin(cloudLosingCatData);
        _chooseFlowerIndex = index;
    }

    public void Close()
    {
        flowerObject.gameObject.SetActive(false);
        noneObject.SetActive(true);
    }
}