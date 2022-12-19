using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_EntranceDiary : MvcBehaviour
{
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private GameObject effectObject;
    [SerializeField] private GameObject maskObject;

    public void SetData(CloudLosingCatData losingCatData)
    {
        catSkin.ChangeSkin(losingCatData);
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
        catSkin.SetActive(value);
    }

    public void SetSelect(bool value)
    {
        effectObject.SetActive(value);
        maskObject.SetActive(!value);
    }
}
