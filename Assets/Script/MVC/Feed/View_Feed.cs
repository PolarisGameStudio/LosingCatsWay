using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using System;
using DG.Tweening;

public class View_Feed : ViewBehaviour
{
    [SerializeField] private Card_FeedItem[] cards;

    [Title("Nav")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI diamondText;
    
    public override void Init()
    {
        base.Init();
        App.model.feed.OnCatsChange += OnCatsChange;
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
    }

    public override void Close()
    {
        base.Close();
        for (int i = 0; i < cards.Length; i++)
            cards[i].SetActiveContainer(false);
    }

    private void OnCatsChange(object value)
    {
        List<Cat> cats = (List<Cat>)value;

        for (int i = 0; i < cards.Length; i++)
        {
            Card_FeedItem card = cards[i];
            int index = i;
            int slotLine = Mathf.CeilToInt(cats.Count / 3f);

            // 排版而已
            for (int j = 0; j < cards.Length; j++)
                cards[i].gameObject.SetActive(false);

            for (int j = 0; j < slotLine; j++)
                cards[i].gameObject.SetActive(true);
            
            if (index >= cats.Count)
            {
                card.SetActiveContainer(false);
            }
            else
            {
                card.SetActiveContainer(true);
                card.SetData(cats[i]);
            }
        }
    }

    private void OnCoinChange(object value)
    {
        int coin = Convert.ToInt32(value);
        coinText.text = coin.ToString();
    }

    private void OnDiamondChange(object value)
    {
        int diamond = Convert.ToInt32(value);
        diamondText.text = diamond.ToString();
    }
}