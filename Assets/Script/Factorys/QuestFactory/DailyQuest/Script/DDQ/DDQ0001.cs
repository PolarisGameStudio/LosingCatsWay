using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DDQ0001", menuName = "Factory/Quests/DDQ/Create DDQ0001")]
public class DDQ0001 : DailyQuest
{
    public override void Init()
    {
        App.controller.cultive.OnPlayCat += BindWithValue;
        App.controller.cultive.OnFeedFood += Bind;
        App.controller.cultive.OnFeedWater += Bind;
        App.controller.cultive.OnChangeLitter += Bind;
    }

    public void Bind()
    {
        Progress++;

        if (Progress == TargetCount)
        {
            App.controller.cultive.OnFeedFood -= Bind;
            App.controller.cultive.OnFeedWater -= Bind;
            App.controller.cultive.OnChangeLitter -= Bind;
        }
    }

    public void BindWithValue(object value)
    {
        Progress++;

        if (Progress == TargetCount)
        {
            App.controller.cultive.OnPlayCat -= BindWithValue;
        }
    }
}
