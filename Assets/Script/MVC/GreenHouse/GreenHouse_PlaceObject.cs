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
        App.controller.greenHouse.ChooseFlower(_chooseFlowerIndex);
    }

    public void Active(CloudLosingCatData cloudLosingCatData)
    {
        flowerObject.gameObject.SetActive(true);
        noneObject.SetActive(false);
        nameObject.SetActive(true);
        
        nameText.text = cloudLosingCatData.CatData.CatName;

        flowerObject.ChangeSkin(cloudLosingCatData);
        flowerObject.DoAnimation(true);
    }

    public void ActiveByChooseCat(CloudLosingCatData cloudLosingCatData, int index)
    {
        flowerObject.ChangeSkin(cloudLosingCatData);
        nameText.text = cloudLosingCatData.CatData.CatName;
        _chooseFlowerIndex = index;
    }

    public void Close()
    {
        flowerObject.gameObject.SetActive(false);
        noneObject.SetActive(true);
        nameObject.SetActive(false);
    }
}