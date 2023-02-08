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
            print("t1");
            App.view.lobby.mallRedPoint.SetActive(true);
            return;    
        }

        PurchaseRecord purchaseRecord = App.model.mall.PurchaseRecords[id];

        if (purchaseRecord.BuyCount < mallItems[0].limitCount)
        {
            print("t2");
            App.view.lobby.mallRedPoint.SetActive(true);
            return;   
        }

        print("t3");
        App.view.lobby.mallRedPoint.SetActive(false);
    }
}