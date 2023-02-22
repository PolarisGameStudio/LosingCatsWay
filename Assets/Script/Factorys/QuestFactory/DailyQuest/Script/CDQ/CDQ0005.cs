using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CDQ0005", menuName = "Factory/Quests/CDQ/Create CDQ0005")]
public class CDQ0005 : DailyQuest
{
    public override void Init()
    {
        base.Init();
        App.system.bigGames.OnClose += Bind;
    }

    private void Bind()
    {
        Progress ++;

        if (Progress >= TargetCount)
        {
            App.system.bigGames.OnClose -= Bind;
        }
    }
}
