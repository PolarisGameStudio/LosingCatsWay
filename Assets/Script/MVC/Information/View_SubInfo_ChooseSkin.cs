using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class View_SubInfo_ChooseSkin : ViewBehaviour
{
    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private Transform content;
    [SerializeField] private Card_CatChooseSkin card;
    [SerializeField] private GameObject confirmButton;
    [SerializeField] private GameObject noSkinSelectedObject;
    [SerializeField] private Transform noSkinBorderTransform;

    private List<Card_CatChooseSkin> cards = new List<Card_CatChooseSkin>();

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
        App.model.information.OnSkinItemsChange += OnSkinItemsChange;
        App.model.information.OnSelectedSkinIndexChange += OnSelectedSkinIndexChange;
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
        
        for (int i = 0; i < cards.Count; i++)
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

        for (int i = 1; i < content.childCount; i++) // 0 = 脫skin
        {
            Destroy(content.GetChild(i).gameObject);
        }
        
        cards.Clear();
        
        for (int i = 0; i < skinItems.Count; i++)
        {
            var tmp = Instantiate(card, content);
            tmp.SetData(skinItems[i]);
            tmp.SetSelect(false);
            cards.Add(tmp);
        }
    }
}
