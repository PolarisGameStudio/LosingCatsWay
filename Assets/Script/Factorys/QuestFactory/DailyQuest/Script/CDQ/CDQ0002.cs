using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CDQ0002", menuName = "Factory/Quests/CDQ/Create CDQ0002")]
public class CDQ0002 : DailyQuest
{
    public override void Init()
    {
        App.controller.cultive.OnAddSatiety += BindWithValue;
    }

    private void BindWithValue(object value)
    {
        int addValue = Mathf.RoundToInt((float)value);

        Progress += addValue;

        if (Progress >= TargetCount)
        {
            App.controller.cultive.OnAddSatiety -= BindWithValue;
        }
    }
}
