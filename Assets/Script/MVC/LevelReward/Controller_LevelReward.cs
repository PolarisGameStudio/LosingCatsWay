using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_LevelReward : ControllerBehavior
{
    public void Init()
    {
        App.model.levelReward.MaxLevel = 40;
        App.view.levelReward.CheckLobbyRed();
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
        App.controller.lobby.SetBuffer();
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
}
