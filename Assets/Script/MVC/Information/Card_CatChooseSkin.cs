using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_CatChooseSkin : MvcBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private GameObject darkMask;

    [Title("Selected")] [SerializeField] private GameObject selectedBg;
    [SerializeField] private Transform borderTransform;
    [SerializeField] private Image selectedIcon;

    public void SetSelect(bool value)
    {
        selectedBg.SetActive(value);

        if (value)
            borderTransform.DOScale(new Vector2(1.05f, 1.05f), 0.1f).From(Vector2.one).SetLoops(2, LoopType.Yoyo);
        else
            borderTransform.DOKill();
    }

    public void SetData(Item item)
    {
        icon.sprite = item.icon;
        selectedIcon.sprite = item.icon;
        nameText.text = item.Name;
        countText.text = item.Count.ToString();
        
        darkMask.SetActive(item.Count <= 0);
    }

    public void Select()
    {
        int index = transform.GetSiblingIndex() - 1; // 第一個是卸裝
        App.controller.information.ChooseSkin(index);
    }
}
