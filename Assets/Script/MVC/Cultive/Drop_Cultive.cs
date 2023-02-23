using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop_Cultive : MvcBehaviour, IDropHandler
{
    public bool isCat;
    // public ItemType dropItemType;
    
    [Title("Tutorial")]
    public bool canFeedFood = true;
    public bool canFeedWater = true;
    public bool canChangeLitter = true;

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public void OnDrop(PointerEventData eventData)
    {
        App.controller.cultive.isDragging = false;
        
        Item item = App.model.cultive.DragItem; //TODO 解偶
        Cat cat = App.model.cultive.SelectedCat;
        
        if (item == null) return;
        
        //拒絕
        if (isCat)
        {
            // 玩具拒絕
            if (item.itemType == ItemType.Play)
            {
                // 未在倒數不可玩
                if (App.model.cultive.NextCleanDateTime <= App.system.myTime.MyTimeNow)
                {
                    // if (App.model.cultive.CleanLitterCount > 0)
                    //     App.controller.cultive.NoLitterCatTalk();
                    // else
                    //     App.controller.cultive.NoLitterPopUp();
                    App.controller.cultive.NoLitterCatTalk();
                    App.controller.cultive.NoLitterPopUp();
                
                    App.controller.cultive.Reject();
                    return;
                }
                
                // 生病不能玩
                if (!string.IsNullOrEmpty(cat.cloudCatData.CatHealthData.SickId) || cat.cloudCatData.CatHealthData.IsBug)
                {
                    App.controller.cultive.Reject();
                    return;
                }
            }
            
            // 食物拒絕
            if (item.itemType == ItemType.Feed && item.itemFeedType == ItemFeedType.Food)
            {
                if (!canFeedFood)
                {
                    App.controller.cultive.Reject();
                    return;
                }
                
                // 飽足大等於100不接受
                if (cat.cloudCatData.CatSurviveData.Satiety >= 100f)
                {
                    App.controller.cultive.Reject();
                    return;
                }
                
                // 討厭
                if (cat.cloudCatData.CatSurviveData.HateFoodIndex == (int)item.foodType)
                {
                    App.controller.cultive.Reject();
                    return;
                }
            }
            
            // 水拒絕
            if (item.itemType == ItemType.Feed && item.itemFeedType == ItemFeedType.Water)
            {
                if (!canFeedWater)
                {
                    App.controller.cultive.Reject();
                    return;
                }
                
                // 水分大等於100不接受
                if (cat.cloudCatData.CatSurviveData.Moisture >= 100f)
                {
                    App.controller.cultive.Reject();
                    return;
                }

                // 不喜歡的湯
                if (item.waterType != WaterType.Water && cat.cloudCatData.CatSurviveData.HateSoupIndex == (int)item.waterType)
                {
                    App.controller.cultive.Reject();
                    return;
                }
            }
            
            // 零食拒絕
            if (item.itemType == ItemType.Feed && item.itemFeedType == ItemFeedType.Snack)
            {
                // 不喜歡零食
                if ((int)item.snackType == cat.cloudCatData.CatSurviveData.HateSnackIndex)
                {
                    App.controller.cultive.Reject();
                    return;
                }
            }
            
            // 罐頭拒絕
            if (item.itemType == ItemType.Feed && item.itemFeedType == ItemFeedType.Can)
            {
                // 飽足大等於100不接受
                if (cat.cloudCatData.CatSurviveData.Satiety >= 100f)
                {
                    App.controller.cultive.Reject();
                    return;
                }
            }
        }
        
        App.system.soundEffect.Play("Button");
        VibrateExtension.Vibrate(VibrateType.Nope);
        
        if (item.itemType == ItemType.Litter && !isCat)
        {
            if (!canChangeLitter)
                return;
            
            App.controller.cultive.ChangeLitter();
            return;
        }

        if (item.itemType == ItemType.Feed && isCat)
        {
            App.controller.cultive.Feed();
            return;
        }
        
        if (item.itemType == ItemType.Play && isCat)
        {
            App.controller.cultive.Play();
        }
    }
}
