using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Item_ChooseCat : MvcBehaviour
{
    public CatSkin catSkin;
    
    public TextMeshProUGUI catNameText;
    public TextMeshProUGUI selectedCatNameText;

    public GameObject selectedBg;
    public RectTransform frameRect;

    public bool isFriendView;

    public void SetData(Cat cat)
    {
        catSkin.ChangeSkin(cat.cloudCatData);

        var catName = cat.cloudCatData.CatData.CatName;
        
        catNameText.text = catName;
        selectedCatNameText.text = catName;
    }
    
    public void SetSelect(bool value)
    {
        selectedBg.SetActive(value);

        if (value)
            frameRect.DOScale(Vector2.one, 0.25f).From(new Vector2(1.1f, 1.1f));
        else
            frameRect.DOKill();
    }

    public void Select()
    {
        if (isFriendView)
        {
            int index = transform.GetSiblingIndex();
            App.controller.friend.ChooseCat(index);
            return;
        }
    }
}
