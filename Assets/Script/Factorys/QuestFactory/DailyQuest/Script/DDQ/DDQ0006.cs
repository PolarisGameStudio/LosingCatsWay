using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DDQ0006", menuName = "Factory/Quests/DDQ/Create DDQ0006")]
public class DDQ0006 : DailyQuest
{
    public override void Init()
    {
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
