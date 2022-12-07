using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CDQ0001", menuName = "Factory/Quests/CDQ/Create CDQ0001")]
public class CDQ0001 : DailyQuest
{
    public override void Init()
    {
        App.controller.cultive.OnPlayCat += BindWithValue;
    }

    private void BindWithValue(object value)
    {
        string playId = value.ToString();
        
        if (!playId.Equals("ICP00001"))
            return;
        
        Progress++;

        if (Progress == TargetCount)
        {
            App.controller.cultive.OnPlayCat -= BindWithValue;
        }
    }
}
