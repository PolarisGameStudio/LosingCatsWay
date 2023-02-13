using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Lobby : ControllerBehavior
{
    public void Init()
    {
        App.model.lobby.ExpBuffer = App.system.player.Exp;
        App.model.lobby.NextExpBuffer = App.system.player.NextLevelExp;
        App.model.lobby.LevelBuffer = App.system.player.Level;
        App.model.lobby.TmpMoney = App.system.player.Coin;
        App.model.lobby.TmpDiamond = App.system.player.Diamond;
    }
    
    public void Open()
    {
        App.system.bgm.FadeIn().Play("Lobby");
        App.view.lobby.Open();
        App.system.room.OpenRooms();
        
        SetBuffer();
    }

    public void Close()
    {
        App.view.lobby.Close();
    }

    public void OpenBuildMode()
    {
        Close();
        App.controller.build.Open();
        App.system.soundEffect.Play("Button");
    }

    public void OpenBag()
    {
        App.controller.bag.Open();
        Close();
        App.system.soundEffect.Play("Button");
    }

    public void OpenFeed()
    {
        App.controller.feed.Open();
        Close();
        App.system.soundEffect.Play("Button");
    }

    public void OpenMap()
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

    public void OpenScreenshot()
    {
        App.system.screenshot.OnScreenshotComplete += CloseScreenshot;
        App.system.screenshot.OnScreenshotCancel += CloseScreenshot;
        Close();
        App.system.screenshot.Open();
        App.system.soundEffect.Play("Button");
    }

    private void CloseScreenshot()
    {
        Open();
        App.system.screenshot.OnScreenshotComplete -= CloseScreenshot;
        App.system.screenshot.OnScreenshotCancel -= CloseScreenshot;
    }

    public void OpenInformation()
    {
        Close();
        App.controller.information.Open();
        App.system.soundEffect.Play("Button");
    }

    public void OpenFriend()
    {
        Close();
        App.controller.friend.Open();
    }

    public void OpenMail()
    {
        App.system.mail.Open();
    }

    public void OpenMall()
    {
        App.controller.mall.Open();
        DOVirtual.DelayedCall(0.25f, () => App.controller.mall.SelectPage(0));
    }

    public void OpenTopUp()
    {
        App.controller.mall.Open();
        DOVirtual.DelayedCall(0.25f, () => App.controller.mall.SelectPage(6));
    }

    public void OpenCatGuide()
    {
        App.controller.catGuide.Open();
    }

    public void OpenDailyQuest()
    {
        App.controller.dailyQuest.Open();
    }

    public void OpenPost()
    {
        App.system.post.Open();
    }

    public void OpenSign()
    {
        App.controller.monthSign.Open();
    }

    public void OpenSideMenu()
    {
        App.system.sideMenu.Open();
    }

    public void OpenArchive()
    {
        App.controller.pedia.SelectTab(0);
    }

    public void OpenEvent()
    {
        App.controller.events.Open();
    }

    #region Buffer

    public void AddExpBuffer(int expBuffer, int nextExpBuffer)
    {
        App.model.lobby.ExpBuffer = expBuffer;
        App.model.lobby.NextExpBuffer = nextExpBuffer;
    }

    public void AddLevelBuffer(int value)
    {
        App.model.lobby.LevelBuffer = value;
    }

    public void AddMoneyBuffer(int value)
    {
        App.model.lobby.MoneyBuffer += value;
    }

    public void AddDiamondBuffer(int value)
    {
        App.model.lobby.DiamondBuffer += value;
    }

    public void SetBuffer()
    {
        int levelBuffer = App.model.lobby.LevelBuffer; // 未顯示等級
        int tmpLevel = App.model.lobby.TmpLevel; // 已顯示等級
        int expBuffer = App.model.lobby.ExpBuffer; // 未顯示經驗
        int tmpExp = App.model.lobby.TmpExp; // 已顯示經驗
        
        if (levelBuffer > tmpLevel)
        {
            if (tmpLevel != -1)
                App.view.lobby.expParticle.Play();
            App.model.lobby.TmpLevel = levelBuffer;
        }
        
        if (expBuffer != tmpExp && tmpExp != -1)
            App.view.lobby.expParticle.Play();
        App.view.lobby.SetExpFill(App.model.lobby.ExpBuffer, App.model.lobby.NextExpBuffer);
        App.model.lobby.TmpExp = App.model.lobby.ExpBuffer;
        
        if (App.model.lobby.MoneyBuffer > 0)
        {
            App.model.lobby.TmpMoney += App.model.lobby.MoneyBuffer;
            App.view.lobby.moneyParticle.Play();
            App.model.lobby.MoneyBuffer = 0;
        }
        
        if (App.model.lobby.DiamondBuffer > 0)
        {
            App.model.lobby.TmpDiamond += App.model.lobby.DiamondBuffer;
            App.view.lobby.diamondParticle.Play();
            App.model.lobby.DiamondBuffer = 0;
        }
    }
    
    #endregion
}