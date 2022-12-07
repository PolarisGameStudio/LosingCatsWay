using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class Card_Abandon : MvcBehaviour
{
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private TextMeshProUGUI nameText;
    
    [Title("Selected")]
    [SerializeField] private GameObject selectedObject;
    [SerializeField] private TextMeshProUGUI selectedName;

    [Title("DoTween")] [SerializeField] private RectTransform frameRect;

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        catSkin.SetActive(active);
    }

    public void SetData(CloudCatData cloudCatData)
    {
        catSkin.ChangeSkin(cloudCatData);
        nameText.text = cloudCatData.CatData.CatName;
        selectedName.text = cloudCatData.CatData.CatName;
    }

    public void SetSelect(bool value)
    {
        selectedObject.SetActive(value);

        if (value)
            frameRect.DOScale(Vector2.one, 0.25f).From(new Vector2(1.1f, 1.1f));
        else
            frameRect.DOKill();
    }

    public void Select()
    {
        int index = transform.GetSiblingIndex();
        App.system.abandon.Select(index);
    }
}
