using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ACR0007", menuName = "Factory/Quests/ACR/Create ACR0007")]
public class ACR0007 : AchieveQuest
{
    public override void Init()
    {
    }

    public override int Progress
    {
        get
        {
            // var count = App.system.room.MyRooms.Find(x => x.roomData.roomType == RoomType.Features).Count;
            int count = App.system.room.FeatureRoomsCount;
            App.system.quest.QuestProgressData[commonId] = count;
            return App.system.quest.QuestProgressData[commonId];
        }
        set
        {
            App.system.quest.QuestProgressData[commonId] = value;
        }
    }
}
