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
}
