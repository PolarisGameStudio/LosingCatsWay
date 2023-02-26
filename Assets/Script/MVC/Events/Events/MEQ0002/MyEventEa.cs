using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEventEa : MyEvent
{
    public Quest[] quests;
    public MEQ00002_Card[] cards;

    public override void Open()
    {
        RefreshUI();
    }

    public override void Init()
    {
        for (int i = 0; i < quests.Length; i++)
            quests[i].Init();
        
        RefreshUI();
    }

    public override bool CheckRedPoint()
    {
        for (int i = 0; i < quests.Length; i++)
        {
            Quest quest = quests[i];
            if (quest.IsReach && !quest.IsReceived)
                return true;
        }

        return false;
    }

    public void GetReward(int index)
    {
        var quest = quests[index];
     
        if (!quest.IsReach)
            return;
        
        Reward[] rewards = quest.Rewards;
        App.system.reward.Open(rewards);

        quest.IsReceived = true;
        
        RefreshUI();
        App.controller.events.RefreshRedPoint();
    }

    private void RefreshUI()
    {
        for (int i = 0; i < cards.Length; i++)
            cards[i].SetData(quests[i]);
    }
}