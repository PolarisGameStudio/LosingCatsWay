using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ACR0008", menuName = "Factory/Quests/ACR/Create ACR0008")]
public class ACR0008 : AchieveQuest
{
    public override void Init()
    {
    }

    public override int Progress
    {
        get
        {
            App.system.quest.QuestProgressData[commonId] = App.system.player.Level;
            return App.system.quest.QuestProgressData[commonId];
        }
        set
        {
            App.system.quest.QuestProgressData[commonId] = value;
        }
    }
}
