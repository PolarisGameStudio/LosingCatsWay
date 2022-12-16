using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class MallContainer : ViewBehaviour
{
    public Mall_Item[] mallItems;
    public Scrollbar scrollBar;

    public override void Open()
    {
        base.Open();
        if (scrollBar != null)
            scrollBar.value = 0;
    }

    public virtual void Refresh()
    {
    }
    
    public virtual void OpenInformation(int index)
    {
        var rewards = mallItems[index].rewards;
        App.controller.mall.OpenPreviewPackageView(rewards);
    }
    
    public virtual void BuyItem(int index)
    {
        var itemMall = mallItems[index];

        if (itemMall.refreshType != MallItemRefreshType.Infinity)
        {
            string id = itemMall.id;

            if (App.model.mall.PurchaseRecords.ContainsKey(id))
            {
                PurchaseRecord purchaseRecord = App.model.mall.PurchaseRecords[id];

                purchaseRecord.BuyCount++;
                purchaseRecord.LastBuyTime = Timestamp.GetCurrentTimestamp();

                App.model.mall.PurchaseRecords[id] = purchaseRecord;
            }
            else
            {
                PurchaseRecord purchaseRecord = new PurchaseRecord();

                purchaseRecord.BuyCount = 1;
                purchaseRecord.LastBuyTime = Timestamp.GetCurrentTimestamp();

                App.model.mall.PurchaseRecords.Add(id, purchaseRecord);
            }
        }
        
        App.system.reward.Open(itemMall.rewards);
        Refresh();
    }
}