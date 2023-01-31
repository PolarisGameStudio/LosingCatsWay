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
    
    public void Click(int index)
    {
        App.controller.greenHouse.ChooseFlower(index);
    }

    public void Active(CloudLosingCatData cloudLosingCatData)
    {
        flowerObject.gameObject.SetActive(true);
        noneObject.SetActive(false);
        
        flowerObject.ChangeSkin(cloudLosingCatData);
    }

    public void Close()
    {
        flowerObject.gameObject.SetActive(false);
        noneObject.SetActive(true);
    }
}