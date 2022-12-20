using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CDQ0001", menuName = "Factory/Quests/CDQ/Create CDQ0001")]
public class CDQ0001 : DailyQuest
{
    public override void Init()
    {
        App.controller.cultive.OnAddFun += BindWithValue;
    }

    private void BindWithValue(object value)
    {
        int addValue = (int)value;

        Progress += addValue;

        if (Progress >= TargetCount)
        {
            App.controller.cultive.OnAddFun -= BindWithValue;
        }
    }
}