using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MEQ0002_4", menuName = "Factory/Quests/MEQ/Create MEQ0002_4")]
public class MEQ0002_4 : Quest
{
    public override void Init()
    {
        App.system.player.OnReduceCoinChange += Bind;
    }

    public void Bind(object value)
    {
        int addValue = (int)value;

        Progress += addValue;

        if (Progress >= TargetCount)
        {
            App.system.player.OnReduceCoinChange -= Bind;
        }
    }
}