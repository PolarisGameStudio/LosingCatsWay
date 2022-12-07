using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CDQ0003", menuName = "Factory/Quests/CDQ/Create CDQ0003")]
public class CDQ0003 : DailyQuest
{
    public override void Init()
    {
        App.controller.cultive.OnPlayCat += BindWithValue;
    }

    private void BindWithValue(object value)
    {
        string playId = value.ToString();
        
        if (!playId.Equals("ICP00003"))
            return;
        
        Progress++;

        if (Progress == TargetCount)
        {
            App.controller.cultive.OnPlayCat -= BindWithValue;
        }
    }
}
