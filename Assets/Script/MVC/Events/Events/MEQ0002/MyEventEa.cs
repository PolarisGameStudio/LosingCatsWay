using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEventEa : MyEvent
{
    public Quest[] quests;
    
    public override void Open()
    {
        print("EA");
    }

    public override void Init()
    {
        // for (int i = 0; i < quests.Length; i++)
        // {
        //     Quest quest = quests[i];
        //     
        //     if (quest.IsReceived)
        //         continue;
        //     
        //     
        // }
    }
}
