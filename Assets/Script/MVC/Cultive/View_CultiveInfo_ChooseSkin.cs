using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class View_CultiveInfo_ChooseSkin : ViewBehaviour
{
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private Card_CultiveChooseSkin[] cards;
    [SerializeField] private GameObject confirmButton;
    [SerializeField] private GameObject noSkinSelectedObject;
    [SerializeField] private Transform noSkinBorderTransform;

    public override void Open()
    {
        base.Open();
        _scrollbar.value = 1;
        confirmButton.SetActive(false);
        noSkinSelectedObject.SetActive(false);
    }

    public override void Init()
    {
        base.Init();
        App.model.cultive.OnSkinItemsChange += OnSkinItemsChange;
        App.model.cultive.OnSelectedSkinIndexChange += OnSelectedSkinIndexChange;
    }

    private void OnSelectedSkinIndexChange(object value)
    {
        int index = (int)value;

        noSkinSelectedObject.SetActive(index == -1);
        if (index == -1)
            noSkinBorderTransform.DOScale(new Vector2(1.05f, 1.05f), 0.1f).From(Vector2.one)
                .SetLoops(2, LoopType.Yoyo);
        else
            noSkinBorderTransform.DOKill();
        
        for (int i = 0; i < cards.Length; i++)
        {
            if (i == index)
                cards[i].SetSelect(true);
            else
                cards[i].SetSelect(false);
        }

        confirmButton.SetActive(true);
    }

    private void OnSkinItemsChange(object value)
    {
        var skinItems = (List<Item>)value;

        for (int i = 0; i < cards.Length; i++)
        {
            if (i >= skinItems.Count)
            {
                cards[i].gameObject.SetActive(false);
                continue;
            }
            
            cards[i].SetData(skinItems[i]);
            cards[i].SetSelect(false);
            cards[i].gameObject.SetActive(true);
        }
    }
}
