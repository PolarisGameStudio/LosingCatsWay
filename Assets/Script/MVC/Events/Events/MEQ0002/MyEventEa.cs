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

    public void GetReward(int index)
    {
        var quest = quests[index];
     
        if (!quest.IsReach)
            return;
        
        Reward[] rewards = quest.Rewards;
        App.system.reward.Open(rewards);

        quest.IsReceived = true;
        
        RefreshUI();
    }

    private void RefreshUI()
    {
        for (int i = 0; i < cards.Length; i++)
            cards[i].SetData(quests[i]);
    }
}