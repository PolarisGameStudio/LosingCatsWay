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
        {
            cards[i].SetActive(false);
        }
    }

    private void OnCatsChange(object value)
    {
        List<Cat> cats = (List<Cat>)value;

        for (int i = 0; i < cards.Length; i++)
        {
            Card_FeedItem card = cards[i];
            int index = i;

            if (index >= cats.Count)
            {
                card.SetActive(false);
            }
            else
            {
                card.SetActive(true);
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