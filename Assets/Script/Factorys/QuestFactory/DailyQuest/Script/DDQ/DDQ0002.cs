using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DDQ0002", menuName = "Factory/Quests/DDQ/Create DDQ0002")]
public class DDQ0002 : DailyQuest
{
    public override void Init()
    {
        App.controller.cultive.OnFeedFood += Bind;
        App.controller.cultive.OnFeedWater += Bind;
    }

    public void Bind()
    {
        Progress++;

        if (Progress == TargetCount)
        {
            App.controller.cultive.OnFeedFood -= Bind;
            App.controller.cultive.OnFeedWater -= Bind;
        }
    }
}
