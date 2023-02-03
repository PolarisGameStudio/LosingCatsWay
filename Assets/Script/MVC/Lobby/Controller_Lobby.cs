using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Lobby : ControllerBehavior
{
    public Callback OnLobbyDelayOpen; //todo 噴錢鑽石經驗
    
    public void Open()
    {
        App.system.bgm.FadeIn().Play("Lobby");
        App.view.lobby.Open();
        App.system.room.OpenRooms();

        DOVirtual.DelayedCall(0.25f, () => OnLobbyDelayOpen?.Invoke()); //todo 噴錢鑽石經驗
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
}