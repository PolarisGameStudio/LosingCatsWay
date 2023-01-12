using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_BagChooseCat : MvcBehaviour
{
    public Button button;
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
    
    public void SetData(CloudLosingCatData losingCatData)
    {
        catSkin.ChangeSkin(losingCatData);
        nameText.text = losingCatData.CatData.CatName;
        selectedName.text = losingCatData.CatData.CatName;
    }

    public void SetSelect(bool value)
    {
        selectedObject.SetActive(value);

        if (value)
            frameRect.DOScale(Vector2.one, 0.25f).From(new Vector2(1.1f, 1.1f));
        else
            frameRect.DOKill();
    }
}