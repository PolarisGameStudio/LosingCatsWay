using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class View_Clinic : ViewBehaviour
{
    [Title("UIViews")]
    public View_ClinicChooseFunction chooseFunction;
    public View_ClinicChooseCat chooseCat;
    public View_ClinicCheck checkCat;
    public View_ClinicInvoice invoice;
    public View_ClinicFunction function;
    public View_ClinicResult result;

    [Title("Nav")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI diamondText;

    public override void Init()
    {
        base.Init();
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
    }

    private void OnCoinChange(object value)
    {
        int coin = (int)value;
        coinText.text = coin.ToString();
    }

    private void OnDiamondChange(object value)
    {
        int diamond = (int)value;
        diamondText.text = diamond.ToString();
    }
}
