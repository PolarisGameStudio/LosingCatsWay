using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CDQ0003", menuName = "Factory/Quests/CDQ/Create CDQ0003")]
public class CDQ0003 : DailyQuest
{
    public override void Init()
    {
        App.controller.cultive.OnAddMoisture += BindWithValue;
    }

    private void BindWithValue(object value)
    {
        int addValue = Mathf.RoundToInt((float)value);

        Progress += addValue;

        if (Progress >= TargetCount)
        {
            App.controller.cultive.OnAddMoisture -= BindWithValue;
        }
    }
}
