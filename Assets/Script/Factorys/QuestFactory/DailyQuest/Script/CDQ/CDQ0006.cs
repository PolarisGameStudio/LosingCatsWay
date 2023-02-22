using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CDQ0006", menuName = "Factory/Quests/CDQ/Create CDQ0006")]
public class CDQ0006 : DailyQuest
{
    public override void Init()
    {
        base.Init();
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
