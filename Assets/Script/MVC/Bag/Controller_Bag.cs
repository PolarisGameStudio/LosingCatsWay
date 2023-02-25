using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller_Bag : ControllerBehavior
{
    public View_BagChooseCat viewBagChooseCat;
    private UnityAction<string> _chooseCatAction;

    public void Init()
    {
        RefreshReds();
    }

    public void Open()
    {
        App.view.bag.Open();
        ChooseType(0);
        ChooseItem(0);
    }

    public void Close()
    {
        App.view.bag.Close();
    }

    public void CloseByOpenLobby()
    {
        Close();
        App.system.soundEffect.Play("Button");
        App.controller.lobby.Open();
        App.model.bag.Type = -1;
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

        int redPoint = PlayerPrefs.GetInt("BagRedPoint" + type, 0);

        if (redPoint >= 1)
        {
            PlayerPrefs.SetInt("BagRedPoint" + type, 0);
            RefreshReds();
        }
    }

    public void ChooseItem(int index)
    {
        if (App.model.bag.SelectedItems.Count <= 0)
        {
            App.model.bag.SelectedItem = null;
            App.system.soundEffect.Play("ED00010");
            return;
        }
        
        if (index == -1)
        {
            App.model.bag.SelectedItem = null;
            return;
        }
        
        Item item = App.model.bag.SelectedItems[index];

        if (App.model.bag.SelectedItem == item)
            return;
        
        App.system.soundEffect.Play("ED00010");

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
                App.system.cloudSave.SaveLosingCatData(cloudLosingCatData);
                item.Count--;
                ChooseType(0);
                ChooseType(6);
                App.SaveData();
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
                App.system.cloudSave.SaveLosingCatData(cloudLosingCatData);
                item.Count--;
                ChooseType(0);
                ChooseType(6);
                
                App.system.cat.CheckAngelCat();
                App.SaveData();
            };
        }
    }

    public void ChooseCatOk(string catId)
    {
        _chooseCatAction?.Invoke(catId);
        _chooseCatAction = null;
    }

    public void RefreshReds()
    {
        bool hasRed = false;

        for (int i = 1; i <= 6; i++)
        {
            int bagRedPoint = PlayerPrefs.GetInt("BagRedPoint" + i, 0);
            App.view.bag.redPoints[i - 1].SetActive(bagRedPoint >= 1);
            
            if (bagRedPoint == 1)
                hasRed = true;
        }
        
        App.view.lobby.bagRedPoint.SetActive(hasRed);
    }
}