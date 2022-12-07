using System.Collections;
using System.Collections.Generic;
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
        if (App.model.shop.SelectedType == index) return;
        App.model.shop.SelectedType = index;

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
        {
            if (items[i].notShowAtStore)
                items.RemoveAt(i);
        }
        
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
                if (App.system.player.Coin < App.model.shop.TotalAmount)
                    DOVirtual.DelayedCall(0.1f,
                        () => App.system.confirm.OnlyConfirm()
                            .Active(ConfirmTable.NoMoney, () => App.controller.mall.Open()));
                else
                {
                    App.system.player.Coin -= App.model.shop.TotalAmount;

                    if (item.itemType == ItemType.Room)
                    {
                        App.system.inventory.RoomData[item.id] += App.model.shop.BuyCount;
                    }
                    else
                    {
                        item.Count += App.model.shop.BuyCount;
                    }

                    App.system.cloudSave.UpdateCloudPlayerData();
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
                if (App.system.player.Diamond < App.model.shop.TotalAmount)
                    DOVirtual.DelayedCall(0.1f, () => App.system.confirm.OnlyConfirm().Active(ConfirmTable.NoDiamond));
                else
                {
                    App.system.player.Diamond -= App.model.shop.TotalAmount;

                    if (item.itemType == ItemType.Room)
                    {
                        App.system.inventory.RoomData[item.id] += App.model.shop.BuyCount;
                    }
                    else
                    {
                        item.Count += App.model.shop.BuyCount;
                    }

                    App.system.cloudSave.UpdateCloudPlayerData();
                    App.system.cloudSave.UpdateCloudItemData();
                    ClosePayment();
                    DOVirtual.DelayedCall(0.1f, () => App.system.confirm.OnlyConfirm().Active(ConfirmTable.IsAddToBag));
                }
            }
        });
    }
}
