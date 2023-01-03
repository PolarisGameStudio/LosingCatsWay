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
    [SerializeField] private GameObject starBorder;
    [SerializeField] private GameObject[] stars;

    [Title("Selected")] [SerializeField] private GameObject selectedBg;
    [SerializeField] private Transform borderTransform;
    [SerializeField] private Image selectedIcon;

    public void SetSelect(bool value)
    {
        selectedBg.SetActive(value);
        borderTransform.gameObject.SetActive(value);

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
        
        //todo 數量0就消失
        darkMask.SetActive(item.Count <= 0);

        if (item.skinLevel <= 0)
        {
            starBorder.SetActive(false);
            return;
        }
        
        starBorder.SetActive(true);

        for (int i = 0; i < stars.Length; i++)
            stars[i].SetActive(false);
        for (int i = 0; i < item.skinLevel; i++)
            stars[i].SetActive(true);
    }

    public void Select()
    {
        int index = transform.GetSiblingIndex() - 1; // 第一個是卸裝
        App.controller.information.ChooseSkin(index);
    }
}
