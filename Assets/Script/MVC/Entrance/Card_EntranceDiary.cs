using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_EntranceDiary : MvcBehaviour
{
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private Vector2 catSkinStartPosition;

    public void SetData(CloudLosingCatData losingCatData)
    {
        if (losingCatData == null)
        {
            SetActive(false);
            return;
        }
        SetActive(true);
        catSkin.ChangeSkin(losingCatData);

        if (losingCatData.CatData.CatAge <= 3)
            return;

        catSkin.transform.localPosition = catSkinStartPosition;
    }

    private void SetActive(bool value)
    {
        gameObject.SetActive(value);
        catSkin.SetActive(value);
    }
}
