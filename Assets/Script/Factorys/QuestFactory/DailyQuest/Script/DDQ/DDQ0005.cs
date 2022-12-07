using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DDQ0005", menuName = "Factory/Quests/DDQ/Create DDQ0005")]
public class DDQ0005 : DailyQuest
{
    public override void Init()
    {
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
