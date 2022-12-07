using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Runtime.UIManager.Containers;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using Random = UnityEngine.Random;

public class FindCatSystem : MvcBehaviour
{
    [SerializeField] private UIView view;
    [SerializeField] private FindCatMap[] FindCatMaps;

    private FindCatMap activeMap;

    public async void ActiveMap(int index, bool isTutorial = false)
    {
        FindCatMaps[index].SetCloudCatData(null);
        FindCatMaps[index].IsTutorial = isTutorial;
        
        var cloudCatDatas = await App.system.cloudSave.LoadCloudCatDatasByOwner($"Location{0}", 1); //TODO G8後改回Index
        var cloudCatData = cloudCatDatas.Count > 0 ? cloudCatDatas[0] : null;
        
        if (cloudCatData == null || cloudCatData.CatSurviveData.IsUseToFind)
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.MapNoCats, () =>
            {
                App.system.transition.OnlyClose();
                App.controller.map.Open();
            });
            return;
        }
        
        App.system.transition.OnlyClose();
        App.view.map.Close();
        FindCatMaps[index].SetCloudCatData(cloudCatData);
        FindCatMaps[index].Open();
        view.Show();

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

    public async void ActiveCurrentMap()
    {
        activeMap.SetCloudCatData(null);
        
        var cloudCatDatas = await App.system.cloudSave.LoadCloudCatDatasByOwner($"Location{0}", 1); //TODO G8後改回Index
        var cloudCatData = cloudCatDatas.Count > 0 ? cloudCatDatas[0] : null;
        
        if (cloudCatData == null || cloudCatData.CatSurviveData.IsUseToFind)
        {
            Invoke(nameof(ActiveCurrentMap), 0.25f);
            return;
        }
        
        activeMap.SetCloudCatData(cloudCatData);
        activeMap.Open();
        view.Show();
        DOVirtual.DelayedCall(0.5f, App.system.transition.OnlyClose);
    }
}