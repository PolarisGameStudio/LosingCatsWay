using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_LevelReward : ControllerBehavior
{
    public void Init()
    {
        App.view.levelReward.CheckLobbyRed();
        App.view.levelReward.CheckCardsReceive();
        SetCardsData();
    }
    
    public void Open()
    {
        App.view.levelReward.Open();
    }

    public void Close()
    {
        App.view.levelReward.Close();
    }
    
    public void CloseBySetBuffer()
    {
        Close();
        App.controller.lobby.ActiveBuffer();
    }

    public void ReceiveReward()
    {
        int receiveIndex = App.system.quest.QuestReceivedStatusData["LR001"];
        if (receiveIndex >= App.system.player.Level)
            return;
        int tmpLevel = receiveIndex + 1;
        Reward[] rewards = App.factory.itemFactory.GetRewardsByLevel(tmpLevel);
        App.system.reward.Open(rewards);
        var card = App.view.levelReward.cards[receiveIndex];
        card.SetCanReceive(false);
        card.SetReceive(true);
        App.system.quest.QuestReceivedStatusData["LR001"]++;
        App.view.levelReward.CheckLobbyRed();
    }

    public void OpenDailyQuest()
    {
        Close();
        App.controller.dailyQuest.Open();
    }
    
    private void SetCardsData()
    {
        for (int i = 0; i < 40; i++)
        {
            int level = i + 1;
            App.view.levelReward.cards[i].SetData(level);
        }
    }

    
}
