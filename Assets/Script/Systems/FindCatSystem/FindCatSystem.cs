using UnityEngine;
using Doozy.Runtime.UIManager.Containers;
using DG.Tweening;
using Sirenix.OdinInspector;
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

    [Title("NoFindCats")] [SerializeField] private int findCountLimit;
    [SerializeField, ReadOnly] private int alreadyFindCount;

    private FindCatMap activeMap;
    private int mapIndex;
    
    private async void ActiveMap()
    {
        FindCatMaps[mapIndex].SetCloudCatData(null);
        
        var cloudCatDatas = await App.system.cloudSave.LoadCloudCatDatasByOwner($"Location{mapIndex}", 1);
        var cloudCatData = cloudCatDatas.Count > 0 ? cloudCatDatas[0] : null;
        
        if (cloudCatData == null || cloudCatData.CatSurviveData.IsUseToFind)
        {
            if (IsFindLimit())
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.MapNoCats, () =>
                {
                    App.system.findCat.ActiveGate(mapIndex);
                    DOVirtual.DelayedCall(1f, App.system.transition.OnlyClose);
                });
            else
            {
                alreadyFindCount++;
                DOVirtual.DelayedCall(0.25f, ActiveMap);
            }
            return;
        }
        
        App.system.transition.OnlyClose();
        FindCatMaps[mapIndex].SetCloudCatData(cloudCatData);
        PlayMapBgm();
        FindCatMaps[mapIndex].Open();
        activeMap = FindCatMaps[mapIndex];
    }

    public void Pause()
    {
        if (App.system.tutorial.isTutorial)
            return;
        if (activeMap == null)
            return;
        
        activeMap.Pause();
    }

    public void Exit()
    {
        if (App.system.tutorial.isTutorial)
            return;
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
        alreadyFindCount = 0;

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
        
        if (App.system.player.CanAdoptCatCount <= 0)
        {
            App.system.confirm.Active(ConfirmTable.ConfirmFindCatWithoutAdopt, () =>
            {
                App.system.transition.OnlyOpen();
                DOVirtual.DelayedCall(1f, () =>
                {
                    gateView.InstantHide();
                    ActiveMap();
                });
            });
        }
        else
        {
            App.system.transition.OnlyOpen();
            DOVirtual.DelayedCall(1f, () =>
            {
                gateView.InstantHide();
                ActiveMap();
            });
        }
    }

    public void GoAbandon()
    {
        App.system.abandon.Active($"Location{mapIndex}");
    }

    public void ActiveCurrentGate()
    {
        // App.system.transition.OnlyClose();
        App.system.bgm.FadeOut();
        
        alreadyFindCount = 0;
        
        gateBg.sprite = backgrounds[mapIndex];
        
        string id = $"Location{mapIndex}";
        gateName.text = App.factory.stringFactory.GetMapNameString(id);
        gateContent.text = App.factory.stringFactory.GetMapContentString(id);
        
        view.Show();
        gateView.Show();

        DOVirtual.DelayedCall(0.5f, App.system.transition.OnlyClose);
    }

    private void PlayMapBgm()
    {
        if (mapIndex == 0)
        {
            App.system.bgm.FadeIn().Play("Street");
            return;
        }
        
        App.system.bgm.FadeIn().Play("CloudCatHotel");
    }

    private bool IsFindLimit()
    {
        return alreadyFindCount >= findCountLimit;
    }

    public void CloseToMap()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            Close();
            gateView.InstantHide();
            App.controller.map.Open();
        });
    }
}