using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CDQ0007", menuName = "Factory/Quests/CDQ/Create CDQ0007")]
public class CDQ0007 : DailyQuest
{
    public override void Init()
    {
        App.system.littleGame.OnClose += Bind;
    }

    private void Bind()
    {
        Progress ++;

        if (Progress >= TargetCount)
        {
            App.system.littleGame.OnClose -= Bind;
        }
    }
}
