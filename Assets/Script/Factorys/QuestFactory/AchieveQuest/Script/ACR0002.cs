using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ACR0002", menuName = "Factory/Quests/ACR/Create ACR0002")]
public class ACR0002 : AchieveQuest
{
    public override void Init()
    {
    }

    public override int Progress
    {
        get
        {
            App.system.quest.QuestProgressData[commonId] = App.model.friend.Friends.Count;
            return App.system.quest.QuestProgressData[commonId];
        }
        set
        {
            App.system.quest.QuestProgressData[commonId] = value;
        }
    }
}