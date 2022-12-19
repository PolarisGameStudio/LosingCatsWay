using System;
using Firebase.Firestore;
using Sirenix.OdinInspector;

public class MallContainer_SuperGift : MallContainer
{
    [Title("UI")]
    public Item_Mall_Limited[] itemMallLimiteds;
    
    public override void Refresh()
    {
        for (int i = 0; i < mallItems.Length; i++)
        {
            var mallItem = mallItems[i];

            if (!App.model.mall.PurchaseRecords.ContainsKey(mallItem.id))
                continue;
 
            var buyCount = App.model.mall.PurchaseRecords[mallItem.id].BuyCount;
            var limitCount = mallItem.limitCount;

            if (buyCount >= limitCount)
                RefreshTime(mallItem);
        }
        
        for (int i = 0; i < mallItems.Length; i++)
            RefreshUI(i, mallItems[i]);
    }

    private void RefreshTime(Mall_Item mallItem)
    {
        MallItemRefreshType refreshType = mallItem.refreshType;

        // 跳過
        if (refreshType == MallItemRefreshType.Infinity || refreshType == MallItemRefreshType.OnlyOne ||
            refreshType == MallItemRefreshType.MonthlyCard)
            return;

        DateTime lastBuyTime = App.model.mall.PurchaseRecords[mallItem.id].LastBuyTime.ToDateTime();
        DateTime nowTime = Timestamp.GetCurrentTimestamp().ToDateTime();

        
        // 每月
        if (refreshType == MallItemRefreshType.PerMonth)
        {
            if (lastBuyTime.Month != nowTime.Month)
                App.model.mall.PurchaseRecords.Remove(mallItem.id);
            
            return;
        }

        // 每日 每週
        int targetDay = 1;

        if (refreshType == MallItemRefreshType.PerWeek)
            targetDay = 7;
        
        if ((nowTime - lastBuyTime).TotalDays >= targetDay)
            App.model.mall.PurchaseRecords.Remove(mallItem.id);
    }

    private void RefreshUI(int index, Mall_Item mallItem)
    {
        if (!App.model.mall.PurchaseRecords.ContainsKey(mallItem.id))
        {
            itemMallLimiteds[index].Open();
            return;
        }

        int stock = mallItem.limitCount - App.model.mall.PurchaseRecords[mallItem.id].BuyCount;

        if (stock == 0)
            itemMallLimiteds[index].Close();
        else
            itemMallLimiteds[index].Open();
    }
}