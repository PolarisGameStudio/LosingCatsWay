using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller_Bag : ControllerBehavior
{
    public View_BagChooseCat viewBagChooseCat;
    private UnityAction<string> _chooseCatAction;
    
    public void Open()
    {
        App.view.bag.Open();
        ChooseType(0);
        ChooseItem(0);
    }

    public void Close()
    {
        App.system.soundEffect.Play("Button");
        App.controller.lobby.Open();
        App.view.bag.Close();
        ChooseType(-1);
    }

    public void ChooseType(int type)
    {
        if (App.model.bag.Type == type)
            return;

        App.model.bag.Type = type;
        ItemType targetType = ItemType.All;
        
        switch (type)
        {
            case 1:
                targetType = ItemType.Feed;
                break;
            case 2:
                targetType = ItemType.Tool;
                break;
            case 3:
                targetType = ItemType.Litter;
                break;
            case 4:
                targetType = ItemType.Room;
                break;
            case 5:
                targetType = ItemType.CatSkin;
                break;
            case 6:
                targetType = ItemType.Special;
                break;
        }
        
        List<Item> buffer = App.factory.itemFactory.GetHoldItems(targetType);
        
        for (int i = buffer.Count - 1; i >= 0; i--)
        {
            if (buffer[i].notShowAtBag)
                buffer.RemoveAt(i);
        }
        
        App.model.bag.SelectedItems = buffer;

        ChooseItem(-1);
        
        if (buffer.Count > 0)
            ChooseItem(0);
    }

    public void ChooseItem(int index)
    {
        if (App.model.bag.SelectedItems.Count <= 0)
        {
            App.model.bag.SelectedItem = null;
            return;
        }
        
        App.system.soundEffect.Play("Button");
        
        if (index == -1)
        {
            App.model.bag.SelectedItem = null;
            return;
        }
        
        Item item = App.model.bag.SelectedItems[index];

        if (App.model.bag.SelectedItem == item)
            return;
        
        App.model.bag.SelectedItem = item;
        App.view.bag.SetItemFocus(index);
    }

    public void UseItem()
    {
        App.system.soundEffect.Play("Button");
        _chooseCatAction = null;

        var item = App.model.bag.SelectedItem;
    
        if (item.id == "ITL00021")
        {
            item.Count--;
            App.system.cloudSave.UpdateCloudItemData();

            List<Reward> rewards = new List<Reward>();
            rewards.Add(new Reward
            {
                item = App.factory.itemFactory.GetItem("ITL00002"),
                count = 1
            });
            rewards.Add(new Reward
            {
                item = App.factory.itemFactory.GetItem("ITL00007"),
                count = 1
            });
            rewards.Add(new Reward
            {
                item = App.factory.itemFactory.GetItem("ITL00012"),
                count = 1
            });
            rewards.Add(new Reward
            {
                item = App.factory.itemFactory.GetItem("ITL00017"),
                count = 1
            });

            App.system.reward.Open(rewards.ToArray());
            ChooseType(-1);
            ChooseType(2);
        }
        
        if (item.id == "ITL00022")
        {
            item.Count--;
            App.system.cloudSave.UpdateCloudItemData();
            
            List<Reward> rewards = new List<Reward>();
            rewards.Add(new Reward
            {
                item = App.factory.itemFactory.GetItem("ITL00003"),
                count = 1
            });
            rewards.Add(new Reward
            {
                item = App.factory.itemFactory.GetItem("ITL00008"),
                count = 1
            });
            rewards.Add(new Reward
            {
                item = App.factory.itemFactory.GetItem("ITL00013"),
                count = 1
            });
            rewards.Add(new Reward
            {
                item = App.factory.itemFactory.GetItem("ITL00018"),
                count = 1
            });
            
            App.system.reward.Open(rewards.ToArray());
            ChooseType(-1);
            ChooseType(2);
        }
        
        if (item.id == "ITL00023")
        {
            item.Count--;
            App.system.cloudSave.UpdateCloudItemData();
            
            List<Reward> rewards = new List<Reward>();
            rewards.Add(new Reward
            {
                item = App.factory.itemFactory.GetItem("ITL00004"),
                count = 1
            });
            rewards.Add(new Reward
            {
                item = App.factory.itemFactory.GetItem("ITL00009"),
                count = 1
            });
            rewards.Add(new Reward
            {
                item = App.factory.itemFactory.GetItem("ITL00014"),
                count = 1
            });
            rewards.Add(new Reward
            {
                item = App.factory.itemFactory.GetItem("ITL00019"),
                count = 1
            });
            
            App.system.reward.Open(rewards.ToArray());
            ChooseType(-1);
            ChooseType(2);
        }
        
        if (item.id == "ISL00001")
        {
            viewBagChooseCat.Open(BagChooseCatType.LosingCat, BagChooseCatExceptType.Flower);
            
            _chooseCatAction = (catId) =>
            {
                CloudLosingCatData cloudLosingCatData =
                    App.model.cloister.LosingCatDatas.Find(x => x.CatData.CatId == catId);
                cloudLosingCatData.LosingCatStatus.Add("Flower");
                App.system.cloudSave.UpdateLosingCatStatusData(cloudLosingCatData);
                item.Count--;
                ChooseType(0);
                ChooseType(6);
                
            };
        }
        
        if (item.id == "ISL00004")
        {
            viewBagChooseCat.Open(BagChooseCatType.LosingCat, BagChooseCatExceptType.AngelCat);
            
            _chooseCatAction = (catId) =>
            {
                CloudLosingCatData cloudLosingCatData =
                    App.model.cloister.LosingCatDatas.Find(x => x.CatData.CatId == catId);
                cloudLosingCatData.LosingCatStatus.Add("AngelCat");
                App.system.cloudSave.UpdateLosingCatStatusData(cloudLosingCatData);
                item.Count--;
                ChooseType(0);
                ChooseType(6);
                
                App.system.cat.CheckAngelCat();
            };
        }
    }

    public void ChooseCatOk(string catId)
    {
        _chooseCatAction?.Invoke(catId);
        _chooseCatAction = null;
    }
    
}