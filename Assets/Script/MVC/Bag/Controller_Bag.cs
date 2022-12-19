using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Bag : ControllerBehavior
{
    public void Open()
    {
        App.view.bag.Open();
        ChooseType(0);
        ChooseItem(0);
    }

    public void Close()
    {
        App.controller.lobby.Open();
        App.view.bag.Close();
        App.system.soundEffect.Play("Button");
    }

    public void ChooseType(int type)
    {
        App.system.soundEffect.Play("Button");
        
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
        
        var item = App.model.bag.SelectedItem;
    
        if (item.id == "ITL00021")
        {
            App.factory.itemFactory.GetItem("ITL00021").Count -= 1;
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
            App.model.bag.SelectedItems = App.model.bag.SelectedItems;
            
            return;
        }
        
        if (item.id == "ITL00022")
        {
            App.factory.itemFactory.GetItem("ITL00022").Count -= 1;
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
            App.model.bag.SelectedItems = App.model.bag.SelectedItems;
            
            return;
        }
        
        if (item.id == "ITL00023")
        {
            App.factory.itemFactory.GetItem("ITL00023").Count -= 1;
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
            App.model.bag.SelectedItems = App.model.bag.SelectedItems;
            
            return;
        }
    }
}