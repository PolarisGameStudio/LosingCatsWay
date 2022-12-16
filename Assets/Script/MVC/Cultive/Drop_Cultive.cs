using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop_Cultive : MvcBehaviour, IDropHandler
{
    public bool isCat;
    // public ItemType dropItemType;

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Item item = App.model.cultive.DragItem; //TODO 解偶
        Cat cat = App.model.cultive.SelectedCat;
        
        if (item == null) return;
        
        //拒絕
        if (isCat)
        {
            // 未在倒數不可玩
            if (item.itemType == ItemType.Play && App.model.cultive.NextCleanDateTime <= App.system.myTime.MyTimeNow)
            {
                if (App.model.cultive.CleanLitterCount > 0)
                    App.controller.cultive.NoLitterCatTalk();
                else
                    App.controller.cultive.NoLitterPopUp();
                
                App.controller.cultive.Reject();
                return;
            }
            
            // 食物拒絕
            if (item.itemType == ItemType.Feed && item.itemFeedType == ItemFeedType.Food)
            {
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
                // 水分大等於100不接受
                if (cat.cloudCatData.CatSurviveData.Moisture >= 100f)
                {
                    App.controller.cultive.Reject();
                    return;
                }

                // 不喜歡的湯
                if (cat.cloudCatData.CatSurviveData.LikeSoupIndex == (int)item.waterType)
                {
                    App.controller.cultive.Reject();
                    return;
                }
            }
            
            // 零食拒絕
            if (item.itemType == ItemType.Feed && item.itemFeedType == ItemFeedType.Snack)
            {
                // 不喜歡零食
                if (cat.cloudCatData.CatSurviveData.LikeSnackIndex != (int)item.snackType)
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
