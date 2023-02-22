using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DDQ0001", menuName = "Factory/Quests/DDQ/Create DDQ0001")]
public class DDQ0001 : DailyQuest
{
    public override void Init()
    {
        base.Init();
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
