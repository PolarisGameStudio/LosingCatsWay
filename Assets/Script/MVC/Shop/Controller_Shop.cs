using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Controller_Shop : ControllerBehavior
{
    public CallbackValue OnBuyComplete; //購買完成
    public CallbackValueToValue OnBuyByValue; //購買指定數量

    public void Open()
    {
        App.system.bgm.FadeIn().Play("Shop");
        App.view.shop.Open();
        SelectType(0);
    }

    public void Close()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            App.view.shop.Close();
            App.controller.map.Open();
        });
    }

    public void OpenPayment()
    {
        App.view.shop.OpenShopBuy();
    }

    public void ClosePayment()
    {
        App.view.shop.CloseShopBuy();
    }

    public void SelectType(int index)
    {
        if (App.model.shop.SelectedType == index) 
            return;
        
        App.model.shop.SelectedType = index;

        if (index == -1)
            return;

        ItemType targetType = ItemType.All;
        switch (index)
        {
            case 0:
                targetType = ItemType.All;
                break;
            case 1:
                targetType = ItemType.Feed;
                break;
            case 2:
                targetType = ItemType.Litter;
                break;
            case 3:
                targetType = ItemType.Tool;
                break;
            case 4:
                targetType = ItemType.Room;
                break;
            case 5:
                targetType = ItemType.Special;
                break;
        }

        List<Item> items = App.factory.itemFactory.GetItemByType((int)targetType);

        for (int i = items.Count - 1; i >= 0; i--)
            if (items[i].unlockLevel <= 0)
                items.RemoveAt(i);
        items = items.OrderByDescending(i => i.Unlock).ToList();
        
        App.model.shop.SelectedItems = items;
    }

    public void SelectItem(int index)
    {
        Item item = App.model.shop.SelectedItems[index];
        App.model.shop.SelectedItem = item;
        ChangeBuyCount(1);
        OpenPayment();
    }

    // Slider
    public void ChangeBuyCount(float value)
    {
        App.model.shop.BuyCount = (int)value;
        App.model.shop.TotalAmount = App.model.shop.SelectedItem.price * App.model.shop.BuyCount;
    }

    public void AddBuyCount()
    {
        int count = App.model.shop.BuyCount;
        count++;
        App.model.shop.BuyCount = Mathf.Clamp(count, 1, 99);
        App.model.shop.TotalAmount = App.model.shop.SelectedItem.price * App.model.shop.BuyCount;
    }

    public void ReduceBuyCount()
    {
        int count = App.model.shop.BuyCount;
        count--;
        App.model.shop.BuyCount = Mathf.Clamp(count, 1, 99);
        App.model.shop.TotalAmount = App.model.shop.SelectedItem.price * App.model.shop.BuyCount;
    }

    public void Buy()
    {
        Item item = App.model.shop.SelectedItem;

        App.system.confirm.ActiveByInsert(ConfirmTable.BuyConfirm, "", item.Name, () =>
        {
            if (item.itemBoughtType == ItemBoughtType.Coin)
            {
                if (!App.system.player.ReduceMoney(App.model.shop.TotalAmount))
                    DOVirtual.DelayedCall(0.1f,
                        () => App.system.confirm.Active(ConfirmTable.NoMoney, OpenTopUp));
                else
                {
                    item.Count += App.model.shop.BuyCount;
                    App.system.cloudSave.UpdateCloudItemData();
                    ClosePayment();
                    DOVirtual.DelayedCall(0.1f, () => App.system.confirm.OnlyConfirm().Active(ConfirmTable.IsAddToBag));
                }

                OnBuyComplete?.Invoke(item);
                OnBuyByValue?.Invoke(item, App.model.shop.BuyCount);

                return;
            }

            if (item.itemBoughtType == ItemBoughtType.Diamond)
            {
                if (!App.system.player.ReduceDiamond(App.model.shop.TotalAmount))
                    DOVirtual.DelayedCall(0.1f, () => App.system.confirm.Active(ConfirmTable.NoDiamond, OpenTopUp));
                else
                {
                    item.Count += App.model.shop.BuyCount;
                    App.system.cloudSave.UpdateCloudItemData();
                    ClosePayment();
                    DOVirtual.DelayedCall(0.1f, () => App.system.confirm.OnlyConfirm().Active(ConfirmTable.IsAddToBag));
                }
            }
        });
    }
    
    private void OpenTopUp()
    {
        App.controller.mall.Open();
        DOVirtual.DelayedCall(0.25f, () => App.controller.mall.SelectPage(6));
    }
}
