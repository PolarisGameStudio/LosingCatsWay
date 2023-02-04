using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Sirenix.OdinInspector;
using UnityEngine;

public class AdsSystem : MonoBehaviour
{
    [Title("Debug")] public bool debugMode;

    private RewardedAd rewardedAd;

    public void Active(AdsType adsType)
    {
        AddEvent();

        string adUnitId;
        
#if UNITY_ANDROID
        adUnitId = GetAdUnitIdByAndroid(adsType);
#elif UNITY_IPHONE
        adUnitId = GetAdUnitIdByIos(adsType);
#else
        adUnitId = GetAdUnitIdByAndroid(adsType);
#endif

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
        }
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("Loaded Not Ok");
        ClearEvent();
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        print("Play Faild");
        ClearEvent();
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        print("Play End");
        ClearEvent();
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
            case AdsType.ShopGetItem:
                return "ca-app-pub-4564963026533389/3825459740";
            case AdsType.ShelterRefresh:
                return "ca-app-pub-4564963026533389/2907091756";
            case AdsType.DailyQuest:
                return "ca-app-pub-4564963026533389/8886214736";
            case AdsType.CatchCatRunAway:
                return "ca-app-pub-4564963026533389/2931142480";
        }

        return "ca-app-pub-3940256099942544/5224354917";
    }

    private string GetAdUnitIdByIos(AdsType adsType)
    {
        if (debugMode)
            return "ca-app-pub-3940256099942544/1712485313";

        switch (adsType)
        {
            case AdsType.ShopGetItem:
                return "ca-app-pub-4564963026533389/9165416333";
            case AdsType.ShelterRefresh:
                return "ca-app-pub-4564963026533389/2711650631";
            case AdsType.DailyQuest:
                return "ca-app-pub-4564963026533389/7908136995";
            case AdsType.CatchCatRunAway:
                return "ca-app-pub-4564963026533389/4028601737";
        }

        return "ca-app-pub-3940256099942544/1712485313";
    }
}

public enum AdsType
{
    ShopGetItem,
    ShelterRefresh,
    DailyQuest,
    CatchCatRunAway
}