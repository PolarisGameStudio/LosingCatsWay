using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Lobby : ControllerBehavior
{
    public void Init()
    {
        App.model.lobby.TmpExp = App.system.player.Exp;
        App.model.lobby.TmpLevel = App.system.player.Level;
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

    public void AddExpBuffer(int value)
    {
        App.model.lobby.ExpBuffer += value;
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
        if (App.model.lobby.ExpBuffer > 0)
        {
            App.model.lobby.TmpExp += App.model.lobby.ExpBuffer;
            App.view.lobby.expParticle.Play();
            App.model.lobby.ExpBuffer = 0;
        }
        
        if (App.model.lobby.LevelBuffer > App.model.lobby.TmpLevel)
        {
            App.model.lobby.TmpLevel = App.model.lobby.LevelBuffer;
        }

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
}