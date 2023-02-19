using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class MallContainer : ViewBehaviour
{
    public Mall_Item[] mallItems;
    public Scrollbar scrollBar;

    [Title("UI")] public Item_Mall_Limited[] itemMallLimiteds;

    public override void Open()
    {
        base.Open();

        Refresh();

        if (scrollBar != null)
            scrollBar.value = 0;
    }

    public void Purchase()
    {
        App.system.waiting.Open();
    }

    public virtual void Refresh()
    {
        for (int i = 0; i < mallItems.Length; i++)
        {
            var mallItem = mallItems[i];

            if (!App.model.mall.PurchaseRecords.ContainsKey(mallItem.id) ||
                mallItem.refreshType == MallItemRefreshType.Infinity)
            {
                itemMallLimiteds[i].Close();
                continue;
            }

            var buyCount = App.model.mall.PurchaseRecords[mallItem.id].BuyCount;
            var limitCount = mallItem.limitCount;

            if (mallItem.refreshType == MallItemRefreshType.OnlyOne)
            {
                if (buyCount >= limitCount)
                {
                    itemMallLimiteds[i].Open();
                    continue;
                }
            }

            if (buyCount < limitCount)
            {
                itemMallLimiteds[i].Close();
                continue;
            }

            PurchaseRecord purchaseRecord = App.model.mall.PurchaseRecords[mallItem.id];
            DateTime lastBuyTime = purchaseRecord.LastBuyTime.ToDateTime();
            DateTime nowTime = Timestamp.GetCurrentTimestamp().ToDateTime();

            if (mallItem.refreshType == MallItemRefreshType.PerDay)
            {
                if (nowTime.Year > lastBuyTime.Year || nowTime.Month > lastBuyTime.Month || nowTime.Day > lastBuyTime.Day)
                {
                    itemMallLimiteds[i].Close();
                    App.model.mall.PurchaseRecords[mallItem.id].BuyCount = 0;
                }
                else
                    itemMallLimiteds[i].Open();
            }
            
            if (mallItem.refreshType == MallItemRefreshType.PerWeek)
            {
                int lastWeek = App.system.myTime.GetWeekOfYear(lastBuyTime);
                int nowWeek = App.system.myTime.GetWeekOfYear(nowTime);

                if (nowTime.Year > lastBuyTime.Year || nowWeek > lastWeek)
                {
                    itemMallLimiteds[i].Close();
                    App.model.mall.PurchaseRecords[mallItem.id].BuyCount = 0;
                }
                else
                    itemMallLimiteds[i].Open();
            }
            
            if (mallItem.refreshType == MallItemRefreshType.PerMonth)
            {
                if (nowTime.Year > lastBuyTime.Year || nowTime.Month > lastBuyTime.Month)
                {
                    itemMallLimiteds[i].Close();
                    App.model.mall.PurchaseRecords[mallItem.id].BuyCount = 0;
                }
                else
                    itemMallLimiteds[i].Open();
            }
        }
    }

    public virtual void OpenInformation(int index)
    {
        var rewards = mallItems[index].rewards;
        App.controller.mall.OpenPreviewPackageView(rewards);
    }

    public void GetItem(int index)
    {
        var itemMall = mallItems[index];

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

        App.controller.mall.OnBuyMallItem?.Invoke();
        App.system.reward.Open(itemMall.rewards);
        Refresh();
    }

    public void GetItem(Reward[] rewards)
    {
        App.controller.mall.OnBuyMallItem?.Invoke();
        App.system.reward.Open(rewards);
    }

    public void BuyItem(int index)
    {
        var itemMall = mallItems[index];

        App.system.confirm.ActiveByInsert(ConfirmTable.Hints_Buy1, string.Empty, itemMall.Name, () =>
        {
            // var itemMall = mallItems[index];

            if (itemMall.itemBoughtType == ItemBoughtType.Free)
            {
                GetItem(index);
                return;
            }

            int price = itemMall.price;
            int myCount = App.system.player.Coin; // todo ReduceMoney(PlayerSystem)
            ConfirmTable confirmTable = ConfirmTable.Hints_NoMoney;

            if (itemMall.itemBoughtType == ItemBoughtType.Diamond) // todo ReduceDiamond(PlayerSystem)
            {
                myCount = App.system.player.Diamond;
                confirmTable = ConfirmTable.Hints_NoDiamond;
            }

            if (myCount < price)
            {
                App.system.confirm.Active(confirmTable);
                return;
            }

            GetItem(index);
        });
    }

    public void OnPurchaseComplete(int index)
    {
        GetItem(index);
        App.system.waiting.Close();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason purchaseFailureReason)
    {
        App.system.waiting.Close();
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_TradeFail);
    }
}