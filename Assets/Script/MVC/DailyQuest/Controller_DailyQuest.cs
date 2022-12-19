using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Controller_DailyQuest : ControllerBehavior
{
    [SerializeField] private MyTween_Scale totalGetTween;
    
    public Callback OnClose;
    public Callback OnGetReward;
    public Callback OnGetAdsReward;

    #region Basic

    public void Init()
    {
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
        OnClose?.Invoke();
        OnClose = null;
    }

    public void OpenCatGuide()
    {
        App.view.dailyQuest.Close();
        App.controller.catGuide.Open();
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
        OpenWatchAd();
    }
    
    public void GetReward()
    {
        CloseWatchAd();
        OnGetReward?.Invoke();

        int index = App.model.dailyQuest.GetRewardIndex;
        var quest = App.model.dailyQuest.Quests[index];
        
        if (quest.IsReceived)
            return;
        
        if (!quest.IsReach)
            return;

        App.model.dailyQuest.RewardExp += quest.exp;

        App.system.reward.Open(quest.Rewards);

        quest.IsReceived = true;
        
        App.model.dailyQuest.Quests = App.model.dailyQuest.Quests;
        
        //CheckLevelUp
        OnClose += AddRewardByClose;
        App.controller.catGuide.OnClose += AddRewardByClose;
        
        App.system.cloudSave.UpdateCloudMissionData();
    }

    public void GetAdsReward()
    {
        CloseWatchAd();
        OnGetAdsReward?.Invoke();

        int index = App.model.dailyQuest.GetRewardIndex;
        var quest = App.model.dailyQuest.Quests[index];
        
        if (quest.IsReceived)
            return;
        
        if (!quest.IsReach)
            return;

        App.model.dailyQuest.RewardExp += quest.exp;
        
        App.system.reward.Open(quest.Rewards);

        quest.IsReceived = true;

        App.model.dailyQuest.Quests = App.model.dailyQuest.Quests;
        
        //CheckLevelUp
        OnClose += AddRewardByClose;
        App.controller.catGuide.OnClose += AddRewardByClose;
        
        App.system.cloudSave.UpdateCloudMissionData();
    }
    
    public void GetTotalDailyReward()
    {
        var quest = App.model.dailyQuest.TotalQuest;
        
        if (quest.IsReceived)
            return;
        
        if (!quest.IsReach)
            return;

        App.model.dailyQuest.RewardExp += quest.exp;
        
        App.system.reward.Open(quest.Rewards);

        quest.IsReceived = true;

        App.model.dailyQuest.TotalQuest = quest;
        
        //CheckLevelUp
        OnClose += AddRewardByClose;
        App.controller.catGuide.OnClose += AddRewardByClose;
        
        totalGetTween.Play();
    }

    public void GetAllAdsReward()
    {
        List<Reward> tmp = new List<Reward>();
        bool hasReward = false;

        for (int i = 0; i < 5; i++)
        {
            var quest = App.model.dailyQuest.Quests[i];

            if (quest.IsReceived) continue;

            if (!quest.IsReach) continue;

            hasReward = true;

            App.model.dailyQuest.RewardExp += quest.exp;
        
            tmp.AddRange(quest.Rewards);

            quest.IsReceived = true;

            App.model.dailyQuest.Quests = App.model.dailyQuest.Quests;
        }

        if (!hasReward)
            return;
        
        //CheckLevelUp
        OnClose += AddRewardByClose;
        App.controller.catGuide.OnClose += AddRewardByClose;
        
        App.system.reward.Open(tmp.ToArray());
        
        App.system.cloudSave.UpdateCloudMissionData();
    }

    private void AddRewardByClose()
    {
        App.system.player.AddExp(App.model.dailyQuest.RewardExp);
        App.model.dailyQuest.RewardExp = 0;
    }

    #endregion
}