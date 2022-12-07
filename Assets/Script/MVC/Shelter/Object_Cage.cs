using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Object_Cage : MvcBehaviour
{
    public CatSkin catSkin;
    public Button button;

    public void RefreshCat(CloudCatData cloudCatData)
    {
        catSkin.ChangeSkin(cloudCatData);
    }

    public void Select(int index)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        {
            App.controller.shelter.SelectAdopt(index);
        });
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        catSkin.SetActive(active);
    }
}
