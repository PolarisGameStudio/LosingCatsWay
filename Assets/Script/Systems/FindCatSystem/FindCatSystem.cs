using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class FindCatSystem : MvcBehaviour
{
    [SerializeField] private UIView view;
    [SerializeField] private FindCatMap[] FindCatMaps;
    [SerializeField] private UIView gateView;
    [SerializeField] private Image gateBg;
    [SerializeField] private Sprite[] backgrounds;
    [SerializeField] private TextMeshProUGUI gateName;
    [SerializeField] private TextMeshProUGUI gateContent;

    private FindCatMap activeMap;
    private int mapIndex;
    
    private async void ActiveMap(int index, bool isTutorial = false)
    {
        FindCatMaps[index].SetCloudCatData(null);
        FindCatMaps[index].IsTutorial = isTutorial;
        
        var cloudCatDatas = await App.system.cloudSave.LoadCloudCatDatasByOwner($"Location{0}", 1); //TODO G8後改回Index
        var cloudCatData = cloudCatDatas.Count > 0 ? cloudCatDatas[0] : null;
        
        if (cloudCatData == null || cloudCatData.CatSurviveData.IsUseToFind)
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.MapNoCats, () =>
            {
                App.system.findCat.ActiveGate(index);
                DOVirtual.DelayedCall(1f, App.system.transition.OnlyClose);
            });
            return;
        }
        
        App.system.transition.OnlyClose();
        FindCatMaps[index].SetCloudCatData(cloudCatData);
        PlayMapBgm(index);
        FindCatMaps[index].Open();
        activeMap = FindCatMaps[index];
    }

    public void Pause()
    {
        if (activeMap == null)
            return;
        
        activeMap.Pause();
    }

    public void Exit()
    {
        if (activeMap == null)
            return;
        
        activeMap.Exit();
    }

    public void Close()
    {
        view.InstantHide();
    }

    public void ActiveGate(int index)
    {
        mapIndex = index;
        gateBg.sprite = backgrounds[index];

        App.system.bgm.FadeOut();
        string id = $"Location{index}";
        gateName.text = App.factory.stringFactory.GetMapNameString(id);
        gateContent.text = App.factory.stringFactory.GetMapContentString(id);
        
        view.Show();
        gateView.Show();
    }

    public void GoFindCat()
    {
        App.system.soundEffect.Play("Button");
        
        App.system.transition.OnlyOpen();
        DOVirtual.DelayedCall(1f, () =>
        {
            gateView.InstantHide();
            ActiveMap(mapIndex);
        });
    }

    public void GoAbandon()
    {
        App.system.transition.OnlyOpen();
        DOVirtual.DelayedCall(1f, () =>
        {
            gateView.InstantHide();
            App.system.abandon.Active($"Location{mapIndex}");
        });
    }

    public void ActiveCurrentGate()
    {
        App.system.transition.OnlyClose();
        App.system.bgm.FadeOut();
        
        gateBg.sprite = backgrounds[mapIndex];
        
        string id = $"Location{mapIndex}";
        gateName.text = App.factory.stringFactory.GetMapNameString(id);
        gateContent.text = App.factory.stringFactory.GetMapContentString(id);
        
        view.Show();
        gateView.Show();
    }

    private void PlayMapBgm(int index)
    {
        if (index == 0)
        {
            App.system.bgm.FadeIn().Play("Street");
            return;
        }
        
        App.system.bgm.FadeIn().Play("CloudCatHotel");
    }
}