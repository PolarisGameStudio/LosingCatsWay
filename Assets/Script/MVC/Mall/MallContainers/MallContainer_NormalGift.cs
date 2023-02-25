using System;
using System.Collections.Generic;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Purchasing;

public class MallContainer_NormalGift : MallContainer
{
    [Title("Red")]
    [SerializeField] private GameObject leftRed;
    [SerializeField] private GameObject dailyPackageRed;

    [Title("PriceText")]
    [SerializeField] private PriceTextHelper[] _priceTextHelpers;

    public override void Open()
    {
        base.Open();
        for (int i = 0; i < _priceTextHelpers.Length; i++)
        {
            _priceTextHelpers[i].SetText();
        }
    }

    public override void Refresh()
    {
        base.Refresh();
        RefrehRed();
    }

    public void RefrehRed()
    {
        string id = mallItems[0].id;
        
        if (!App.model.mall.PurchaseRecords.ContainsKey(id))
        {
            App.view.lobby.mallRedPoint.SetActive(true);
            dailyPackageRed.SetActive(true);
            leftRed.SetActive(true);
            return;    
        }

        PurchaseRecord purchaseRecord = App.model.mall.PurchaseRecords[id];

        if (purchaseRecord.BuyCount < mallItems[0].limitCount)
        {
            App.view.lobby.mallRedPoint.SetActive(true);
            dailyPackageRed.SetActive(true);
            leftRed.SetActive(true);
            return;   
        }

        App.view.lobby.mallRedPoint.SetActive(false);
        dailyPackageRed.SetActive(false);
        leftRed.SetActive(false);
    }
}