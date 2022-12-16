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
    
    public void Open()
    {
        App.view.information.Open();
        App.model.information.MyCats = App.system.cat.GetCats();
    }
   
    public void Close()
    {
        App.system.soundEffect.Play("Button");
        App.view.information.Close();
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
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Copied);
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
        App.system.catRename.Active(cloudCatData, () =>
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
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Copied);
    }

    public void DiamondUnlockCatSlot()
    {
        App.system.confirm.ActiveByInsert(ConfirmTable.BuyConfirm, "", App.factory.stringFactory.GetUnlock("ULK001"),() =>
        {
            if (App.system.player.Diamond < 2000)
            {
                DOVirtual.DelayedCall(0.1f, () => App.system.confirm.OnlyConfirm().Active(ConfirmTable.NotEnoughDiamond));
                return;
            }

            App.system.player.Diamond -= 2000;
            App.system.player.DiamondCatSlot += 1;
            DOVirtual.DelayedCall(0.1f, () => App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix));
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
        var cat = App.model.information.SelectedCat;
        string currentSkin = cat.cloudCatData.CatSkinData.UseSkinId;
        
        if (skinBeforePreview.IsNullOrEmpty() && !currentSkin.IsNullOrEmpty())
            skinBeforePreview = cat.cloudCatData.CatSkinData.UseSkinId;
        
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
        if (skinBeforePreview.IsNullOrEmpty())
            return;
        
        var cat = App.model.information.SelectedCat;
        cat.cloudCatData.CatSkinData.UseSkinId = skinBeforePreview;
        App.model.information.SelectedCat = cat;
        App.model.information.MyCats = App.model.information.MyCats;
        skinBeforePreview = String.Empty;
    }

    public void ConfirmChooseSkin()
    {
        App.system.soundEffect.Play("Button");
        VibrateExtension.Vibrate(VibrateType.Nope);
        
        int index = App.model.information.SelectedSkinIndex;
        var cat = App.model.information.SelectedCat;
        App.system.cloudSave.UpdateCloudCatSkinData(cat.cloudCatData);
        
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
            App.system.cloudSave.UpdateCloudItemData();
            OpenChooseSkin();
            App.system.cat.RefreshCatSkin();
            return;
        }

        Item skinItem = App.model.information.SkinItems[index];
        skinItem.Count--;
        App.system.cloudSave.UpdateCloudItemData();
        OpenChooseSkin();
        App.system.cat.RefreshCatSkin();
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
}
