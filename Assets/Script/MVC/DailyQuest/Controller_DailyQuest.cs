using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Controller_DailyQuest : ControllerBehavior
{
    [SerializeField] private MyTween_Scale totalGetTween;
    
    public Callback OnGetReward;
    public Callback OnGetAdsReward;

    #region Basic

    public void Init()
    {
        App.system.openFlow.AddAction(Open);
        
        App.system.myTime.OnFirstLogin += DrawQuests;
        App.system.myTime.OnAlreadyLogin += InitQuests;
        App.model.dailyQuest.TotalQuest = App.factory.questFactory.TotalDailyQuest;
    }

    [Button]
    private void DrawQuests()
    {
        List<Quest> tmp = new List<Quest>();

        // 鑽石任務
        List<Quest> diamondQuests = App.factory.questFactory.GetDiamondDailyQuests();

        for (int i = 0; i < 2; i++)
        {
            int randomIndex = Random.Range(0, diamondQuests.Count);
            tmp.Add(diamondQuests[randomIndex]);
            diamondQuests.RemoveAt(randomIndex);
        }

        // 魚幣任務
        List<Quest> coinQuests = App.factory.questFactory.GetCoinDailyQuests();

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, coinQuests.Count);
            tmp.Add(coinQuests[randomIndex]);
            coinQuests.RemoveAt(randomIndex);
        }

        // 清除任務Progress
        for (int i = 0; i < tmp.Count; i++)
        {
            tmp[i].Progress = 0;
            tmp[i].IsReceived = false;
        }

        App.model.dailyQuest.Quests = tmp;

        InitQuests();

        App.model.dailyQuest.TotalQuest.Progress = 0;
        App.model.dailyQuest.TotalQuest.IsReceived = false;
    }

    private void InitQuests()
    {
        App.model.dailyQuest.Quests = App.model.dailyQuest.Quests;
        for (int i = 0; i < App.model.dailyQuest.Quests.Count; i++)
        {
            var quest = App.model.dailyQuest.Quests[i];
            quest.Init();
        }
    }

    public void Open()
    {
        App.model.dailyQuest.Quests = App.model.dailyQuest.Quests;

        int count = 0;
        for (int i = 0; i < App.model.dailyQuest.Quests.Count; i++)
        {
            if (App.model.dailyQuest.Quests[i].IsReach)
                count++;
        }
        
        App.model.dailyQuest.TotalQuest.Progress = count;
        App.model.dailyQuest.TotalQuest = App.model.dailyQuest.TotalQuest;
        
        App.view.dailyQuest.Open();
    }

    public void Close()
    {
        App.view.dailyQuest.Close();
        
        if (!App.system.openFlow.isEnd)
            App.system.openFlow.NextAction(); // todo 每日任務後面有流程的話會因爲切到等階出BUG 但因爲要讓流程END所以要呼叫Next
    }

    public void CloseBySetBuffer()
    {
        Close();
        App.controller.lobby.SetBuffer();
    }

    public void OpenLevelReward()
    {
        App.view.dailyQuest.Close();
        App.controller.levelReward.Open();
    }

    #endregion

    private void OpenWatchAd()
    {
        App.view.dailyQuest.watchAd.Open();
    }

    public void CloseWatchAd()
    {
        App.view.dailyQuest.watchAd.Close();
    }

    #region GetReward

    public void SelectReward(int index)
    {
        App.model.dailyQuest.GetRewardIndex = index;
        
        if (!App.system.player.Vip)
            OpenWatchAd();
        else
            GetAdsReward();
    }
    
    public void GetReward()
    {
        App.system.soundEffect.Play("ED00011");
        CloseWatchAd();
        OnGetReward?.Invoke();

        int index = App.model.dailyQuest.GetRewardIndex;
        var quest = App.model.dailyQuest.Quests[index];
        
        if (!quest.IsReach)
            return;
        
        if (quest.IsReceived)
            return;

        App.model.dailyQuest.RewardExp += quest.exp;

        App.system.reward.Open(quest.Rewards);

        quest.IsReceived = true;
        App.model.dailyQuest.Quests[index] = quest;
        
        App.model.dailyQuest.Quests = App.model.dailyQuest.Quests;
        
        App.system.player.AddExp(App.model.dailyQuest.RewardExp);
        App.model.dailyQuest.RewardExp = 0;
    }

    public void GetAdsReward()
    {
        App.system.ads.Active(AdsType.DailyQuest, () =>
        {
            CloseWatchAd();
            OnGetAdsReward?.Invoke();

            int index = App.model.dailyQuest.GetRewardIndex;
            var quest = App.model.dailyQuest.Quests[index];
        
            if (!quest.IsReach)
                return;
        
            if (quest.IsReceived)
                return;
        
            App.model.dailyQuest.RewardExp += quest.exp;

            var doubleRewards = quest.Rewards;
            
            for (int i = 0; i < doubleRewards.Length; i++)
                doubleRewards[i].count *= 2;

            App.system.reward.Open(doubleRewards);

            quest.IsReceived = true;
            App.model.dailyQuest.Quests[index] = quest;

            App.model.dailyQuest.Quests = App.model.dailyQuest.Quests;
        
            App.system.player.AddExp(App.model.dailyQuest.RewardExp);
            App.model.dailyQuest.RewardExp = 0;
        });
    }
    
    public void GetTotalDailyReward()
    {
        var quest = App.model.dailyQuest.TotalQuest;
        
        if (!quest.IsReach)
            return;

        if (quest.IsReceived)
            return;

        App.model.dailyQuest.RewardExp += quest.exp;
        
        App.system.reward.Open(quest.Rewards);

        quest.IsReceived = true;

        App.model.dailyQuest.TotalQuest = quest;
        
        App.system.player.AddExp(App.model.dailyQuest.RewardExp);
        App.model.dailyQuest.RewardExp = 0;
        
        totalGetTween.Play();
    }

    public void GetAllAdsReward()
    {
        List<Reward> tmp = new List<Reward>();
        bool hasReward = false;

        for (int i = 0; i < 5; i++)
        {
            var quest = App.model.dailyQuest.Quests[i];

            if (!quest.IsReach)
                continue;

            if (quest.IsReceived)
                continue;

            hasReward = true;

            // App.model.dailyQuest.RewardExp += quest.exp;
            App.system.player.AddExp(quest.exp);

            var doubleReward = quest.Rewards;

            for (int j = 0; j < doubleReward.Length; j++)
                doubleReward[i].count *= 2;

            tmp.AddRange(doubleReward);

            quest.IsReceived = true;
            App.model.dailyQuest.Quests[i] = quest;
            App.model.dailyQuest.Quests = App.model.dailyQuest.Quests;
        }

        if (!hasReward)
            return;
        
        // App.system.player.AddExp(App.model.dailyQuest.RewardExp);
        App.model.dailyQuest.RewardExp = 0;
        
        App.system.reward.Open(tmp.ToArray());
    }

    #endregion
}