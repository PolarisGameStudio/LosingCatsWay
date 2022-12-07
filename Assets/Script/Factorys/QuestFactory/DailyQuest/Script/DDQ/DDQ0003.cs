using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DDQ0003", menuName = "Factory/Quests/DDQ/Create DDQ0003")]
public class DDQ0003 : DailyQuest
{
    public override void Init()
    {
        App.controller.cultive.OnChangeLitter += Bind;

    }

    public void Bind()
    {
        Progress++;

        if (Progress == TargetCount)
        {
            App.controller.cultive.OnChangeLitter -= Bind;
        }
    }
}
