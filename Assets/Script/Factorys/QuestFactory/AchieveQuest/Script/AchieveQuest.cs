using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveQuest : Quest
{
    protected string commonId
    {
        get
        {
            return id.Split('_')[0];
        }
    }

    public override int Progress
    {
        get
        {
            return App.system.quest.QuestProgressData[commonId];
        }
        set
        {
            App.system.quest.QuestProgressData[commonId] = value;
        }
    }

    public override bool IsReceived
    {
        get
        {
            int status = Convert.ToInt32(id.Split('_')[1]);
            int receivedStatus = App.system.quest.QuestReceivedStatusData[commonId];
            return receivedStatus >= status;
        }
        set
        {
            int status = Convert.ToInt32(id.Split('_')[1]);
            App.system.quest.QuestReceivedStatusData[commonId] = value ? status : status - 1;
        }
    }
}
