using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DDQ0003", menuName = "Factory/Quests/DDQ/Create DDQ0003")]
public class DDQ0003 : DailyQuest
{
    public override void Init()
    {
        base.Init();
        App.system.catchCat.map.OnGameEnd += Bind;
    }

    public void Bind()
    {
        Progress++;

        if (Progress == TargetCount)
        {
            App.system.catchCat.map.OnGameEnd -= Bind;
        }
    }
}
