using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DDQ0004", menuName = "Factory/Quests/DDQ/Create DDQ0004")]
public class DDQ0004 : DailyQuest
{
    public override void Init()
    {
        base.Init();
        App.controller.shop.OnBuyByValue += Bind;
    }

    public void Bind(object item, object count)
    {
        int value = (int) count;
        Progress += value;

        if (Progress >= TargetCount)
        {
            App.controller.shop.OnBuyByValue -= Bind;
        }
    }
}
