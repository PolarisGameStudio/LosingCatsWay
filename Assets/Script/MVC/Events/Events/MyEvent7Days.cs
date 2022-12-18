using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using UnityEngine;

public class MyEvent7Days : MyEvent
{
    public GameObject[] masks;
    public List<Reward[]> Rewards;
    
    public override void Open()
    {
        int receivedStatus = App.system.quest.QuestReceivedStatusData[id];

        for (int i = 0; i < receivedStatus; i++)
            masks[i].SetActive(true);
    }

    public void Click()
    {
        int receivedStatus = App.system.quest.QuestReceivedStatusData[id];

        if (receivedStatus >= 7)
            return;
        
        DateTime notTime = Timestamp.GetCurrentTimestamp().ToDateTime();
        DateTime lastReceivedTime = App.system.quest.QuestReceivedTimeData[id].ToDateTime();
        
        if ((notTime - lastReceivedTime).Days < 1)
            return;
        
        App.system.quest.QuestReceivedStatusData[id] = receivedStatus + 1;
        App.system.quest.QuestReceivedTimeData[id] = Timestamp.GetCurrentTimestamp();
        
        App.system.reward.Open(Rewards[receivedStatus]);
        masks[receivedStatus].SetActive(true);
    }
}
