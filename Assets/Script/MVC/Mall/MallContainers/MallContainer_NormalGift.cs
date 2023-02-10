using System;
using System.Collections.Generic;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using UnityEngine.Purchasing;

public class MallContainer_NormalGift : MallContainer
{
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
            return;    
        }

        PurchaseRecord purchaseRecord = App.model.mall.PurchaseRecords[id];

        if (purchaseRecord.BuyCount < mallItems[0].limitCount)
        {
            App.view.lobby.mallRedPoint.SetActive(true);
            return;   
        }

        App.view.lobby.mallRedPoint.SetActive(false);
    }
}