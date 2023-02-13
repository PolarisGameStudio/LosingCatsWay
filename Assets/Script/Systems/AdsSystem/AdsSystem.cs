using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class AdsSystem : MvcBehaviour
{
    [Title("Debug")] public bool debugMode;

    private RewardedAd rewardedAd;
    private UnityAction _endAction;
    private UnityAction _failAction;

    public bool isEditorMode = false;

    private void Start()
    {
#if UNITY_EDITOR
        isEditorMode = true;
#endif
    }

    public void Active(AdsType adsType, UnityAction endAction, UnityAction failAction = null)
    {
        if (isEditorMode)
        {
            endAction?.Invoke();
            return;
        }
        
        AddEvent();

        string adUnitId;
        
#if UNITY_IOS
        adUnitId = GetAdUnitIdByIos(adsType);
#else
        adUnitId = GetAdUnitIdByAndroid(adsType);
#endif
        App.system.waiting.Open();

        _endAction = null;
        _failAction = null;
        
        _endAction = endAction;
        
        if (failAction != null)
            _failAction = failAction;
        
        rewardedAd = new RewardedAd(adUnitId);
        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);
    }

    private void AddEvent()
    {
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }

    private void ClearEvent()
    {
        rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
        rewardedAd.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
        rewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        rewardedAd.OnAdClosed -= HandleRewardedAdClosed;
    }

    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        print("Loaded Ok");

        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
            App.system.waiting.Close();
        }
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("Loaded Not Ok");
        App.system.waiting.Close();
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            _failAction?.Invoke();
        });
        ClearEvent();
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        print("Play Faild");
        App.system.waiting.Close();
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            _failAction?.Invoke();
        });
        ClearEvent();
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        print("Play End");
        ClearEvent();
        _endAction?.Invoke();
    }

    private void HandleUserEarnedReward(object sender, GoogleMobileAds.Api.Reward args)
    {
        print("Get Reward");
    }

    private string GetAdUnitIdByAndroid(AdsType adsType)
    {
        if (debugMode)
            return "ca-app-pub-3940256099942544/5224354917";

        switch (adsType)
        {
            case AdsType.MallFeed:
                return "ca-app-pub-4564963026533389/7905706296";
            case AdsType.MallTool:
                return "ca-app-pub-4564963026533389/3825459740";
            case AdsType.MallCoin:
                return "ca-app-pub-4564963026533389/1531869633";
            case AdsType.MallDiamond:
                return "ca-app-pub-4564963026533389/3898463930";
            case AdsType.CatchCatRun:
                return "ca-app-pub-4564963026533389/2931142480";
            case AdsType.ShelterRefresh:
                return "ca-app-pub-4564963026533389/2907091756";
            case AdsType.DailyQuest:
                return "ca-app-pub-4564963026533389/8886214736";
            case AdsType.SignMonthlySign:
                return "ca-app-pub-4564963026533389/6149771495";
            case AdsType.LobbyCatLosing:
                return "ca-app-pub-4564963026533389/6141483895";
        }

        return "ca-app-pub-3940256099942544/5224354917";
    }

    private string GetAdUnitIdByIos(AdsType adsType)
    {
        if (debugMode)
            return "ca-app-pub-3940256099942544/1712485313";

        switch (adsType)
        {
            case AdsType.MallFeed:
                return "ca-app-pub-4564963026533389/7977040243";
            case AdsType.MallTool:
                return "ca-app-pub-4564963026533389/9165416333";
            case AdsType.MallCoin:
                return "ca-app-pub-4564963026533389/5575056423";
            case AdsType.MallDiamond:
                return "ca-app-pub-4564963026533389/9322729749";
            case AdsType.CatchCatRun:
                return "ca-app-pub-4564963026533389/4028601737";
            case AdsType.ShelterRefresh:
                return "ca-app-pub-4564963026533389/2711650631";
            case AdsType.DailyQuest:
                return "ca-app-pub-4564963026533389/7908136995";
            case AdsType.SignMonthlySign:
                return "ca-app-pub-4564963026533389/6632153079";
            case AdsType.LobbyCatLosing:
                return "ca-app-pub-4564963026533389/1252668038";
        }

        return "ca-app-pub-3940256099942544/1712485313";
    }
}

public enum AdsType
{
    MallFeed,
    MallTool,
    MallCoin,
    MallDiamond,
    CatchCatRun,
    ShelterRefresh,
    DailyQuest,
    SignMonthlySign,
    LobbyCatLosing
}