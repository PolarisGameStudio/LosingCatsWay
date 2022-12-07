using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using System;
using DG.Tweening;

public class Navigation : MvcBehaviour
{
    [SerializeField] private TextMeshProUGUI CoinText;
    [SerializeField] private TextMeshProUGUI DiamondText;

    private UIView uiView;

    private void Start()
    {
        uiView = GetComponent<UIView>();
    }

    public void Init()
    {
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
    }

    private void OnDiamondChange(object value)
    {
        int diamond = Convert.ToInt32(value);
        DiamondText.text = diamond.ToString();
    }

    private void OnCoinChange(object value)
    {
        int coin = Convert.ToInt32(value);
        CoinText.text = coin.ToString();
    }
}
