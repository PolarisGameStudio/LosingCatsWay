using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.Common.Extensions;
using Spine;
using UnityEngine;

public class Controller_Information : ControllerBehavior
{
    private string skinBeforePreview;
    private bool isRecordSkin;
    
    public void Open()
    {
        App.view.information.Open();
        App.model.information.MyCats = App.system.cat.GetCats();
        RefreshDiamondUnlockSlotPrice();
    }
   
    public void Close()
    {
        App.view.information.Close();
    }

    public void CloseByOpenLobby()
    {
        Close();
        App.system.soundEffect.Play("Button");
        App.controller.lobby.Open();
    }

    public void OpenSubInfomation()
    {
        App.system.soundEffect.Play("Button");
        App.view.information.view_SubInformation.Open();
        SelectTab(0);
    }

    public void CloseSubInformation()
    {
        App.system.soundEffect.Play("Button");
        App.view.information.view_SubInformation.Close();
        App.controller.information.Open();
        CancelPreviewSkin();
    }

    public void RenamePlayer()
    {
        App.system.playerRename.Open();
    }

    public void CopyPlayerId()
    {
        App.system.soundEffect.Play("Button");
        string playerId = App.system.player.PlayerId;
        playerId.CopyToClipboard();
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_Copy);
    }

    public void SelectCat(int index)
    {
        App.system.soundEffect.Play("Button");
        App.model.information.SelectedCat = App.model.information.MyCats[index];
        OpenSubInfomation();
    }
    
    public void RenameCat()
    {
        CloseSubInformation();
        CloudCatData cloudCatData = App.model.information.SelectedCat.cloudCatData;
        App.system.catRename.Active(cloudCatData, "Shelter", () =>
        {
            var cat = App.model.information.SelectedCat;
            App.model.information.SelectedCat = cat;
            OpenSubInfomation();
        }, () => OpenSubInfomation());
    }

    public void CopyCatId()
    {
        CloudCatData cloudCatData = App.model.information.SelectedCat.cloudCatData;
        string catId = cloudCatData.CatData.CatId;
        catId.CopyToClipboard();
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_Copy);
    }

    public void DiamondUnlockCatSlot()
    {
        int price = App.model.information.NextDiamondSlotPrice;
        App.system.confirm.ActiveByInsert(ConfirmTable.Hints_Buy1, "", App.factory.stringFactory.GetItemName("ULK001"),() =>
        {
            if (!App.system.player.ReduceDiamond(price))
            {
                DOVirtual.DelayedCall(0.1f, () =>
                {
                    App.system.confirm.Active(ConfirmTable.Hints_NoDiamond, OpenTopUp);
                });
                return;
            }
            
            App.system.player.DiamondCatSlot += 1;
            App.model.information.MyCats = App.system.cat.GetCats();
            RefreshDiamondUnlockSlotPrice();
            DOVirtual.DelayedCall(0.1f, () => App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_UnlockCatSlot));
        });
    }

    private void RefreshDiamondUnlockSlotPrice()
    {
        int nextSlot = App.system.player.DiamondCatSlot + 1;
        int result;
        switch (nextSlot)
        {
            case 1:
                result = 120;
                break;
            case 2:
                result = 180;
                break;
            case 3:
                result = 300;
                break;
            case 4:
                result = 600;
                break;
            case 5:
                result = 900;
                break;
            case 6:
                result = 1080;
                break;
            case 7:
                result = 1200;
                break;
            case 8:
                result = 1200;
                break;
            case 9:
                result = 1200;
                break;
            case 10:
                result = 1200;
                break;
            case 11:
                result = 1200;
                break;
            case 12:
                result = 1200;
                break;
            default:
                result = -1;
                break;
        }

        App.model.information.NextDiamondSlotPrice = result;
    }

    public void OpenMap() // 獲得新貓咪的卡片可點擊
    {
        App.system.cat.PauseCatsGame(true);

        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            Close();
            App.system.room.CloseRooms();
            App.controller.map.Open();
        });
    }

    public void SelectTab(int index)
    {
        App.system.soundEffect.Play("Button");
        App.model.information.TabIndex = index;
        
        if (index == 0)
        {
            OpenStatus();
            return;
        }

        if (index == 1)
        {
            OpenChooseSkin();
        }
    }

    private void OpenStatus()
    {
        App.system.soundEffect.Play("Button");
        App.view.information.view_SubInformation.OpenStatus();
        CloseChooseSkin();
    }

    private void CloseStatus()
    {
        App.view.information.view_SubInformation.CloseStatus();
    }

    #region ChooseSkin
    
    private void OpenChooseSkin()
    {
        App.system.soundEffect.Play("Button");
        App.model.information.SkinItems = App.factory.itemFactory.GetItemByType((int)ItemType.CatSkin);
        App.view.information.view_SubInformation.OpenChooseSkin();
        CloseStatus();
    }

    private void CloseChooseSkin()
    {
        App.view.information.view_SubInformation.CloseChooseSkin();
        CancelPreviewSkin();
    }
    
    public void ChooseSkin(int index)
    {
        App.system.soundEffect.Play("Button");
        App.model.information.SelectedSkinIndex = index;

        if (!isRecordSkin)
        {
            var cat = App.model.information.SelectedCat;
            skinBeforePreview = cat.cloudCatData.CatSkinData.UseSkinId;
            isRecordSkin = true;
        }
        
        PreviewSkin();
    }
    
    private void PreviewSkin()
    {
        int index = App.model.information.SelectedSkinIndex;
        var cat = App.model.information.SelectedCat;

        if (index >= 0)
        {
            Item skinItem = App.model.information.SkinItems[index];
            cat.cloudCatData.CatSkinData.UseSkinId = skinItem.id;
        }
        else
            cat.cloudCatData.CatSkinData.UseSkinId = string.Empty;
        
        App.model.information.SelectedCat = cat;
    }

    private void CancelPreviewSkin()
    {
        if (!isRecordSkin)
            return;
        
        var cat = App.model.information.SelectedCat;
        
        if (skinBeforePreview.IsNullOrEmpty() && cat.cloudCatData.CatSkinData.UseSkinId.IsNullOrEmpty())
            return;
        
        cat.cloudCatData.CatSkinData.UseSkinId = skinBeforePreview;
        App.model.information.SelectedCat = cat;
        App.model.information.MyCats = App.model.information.MyCats;
        skinBeforePreview = string.Empty;
        isRecordSkin = false;
    }

    public void ConfirmChooseSkin()
    {
        App.system.soundEffect.Play("Button");
        
        int index = App.model.information.SelectedSkinIndex;

        isRecordSkin = false;
        
        SpineSetSkinHappy();
        
        if (!skinBeforePreview.IsNullOrEmpty())
        {
            Item lastSkinItem = App.factory.itemFactory.GetItem(skinBeforePreview);
            lastSkinItem.Count++;
            skinBeforePreview = String.Empty;
        }
        
        if (index == -1)
        {
            // 脫
            OpenChooseSkin();
            App.system.cat.RefreshCatSkin();
            App.SaveData();
            return;
        }

        Item skinItem = App.model.information.SkinItems[index];
        skinItem.Count--;
        OpenChooseSkin();
        App.system.cat.RefreshCatSkin();
        App.SaveData();
    }

    private void SpineSetSkinHappy()
    {
        TrackEntry t = App.view.information.view_SubInformation.catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "Rearing_Cat/Rearing_Look_IDLE", false);
        t.Complete += WaitSpineSetSkinHappy;
    }

    private void WaitSpineSetSkinHappy(TrackEntry trackEntry)
    {
        trackEntry.Complete -= WaitSpineSetSkinHappy;
        App.view.information.view_SubInformation.catSkin.skeletonGraphic.AnimationState.SetAnimation(0, "AI_Main/IDLE_Ordinary01", true);
    }
    
    #endregion
    
    private void OpenTopUp()
    {
        App.controller.mall.Open();
        DOVirtual.DelayedCall(0.25f, () => App.controller.mall.SelectPage(6));
    }
}
