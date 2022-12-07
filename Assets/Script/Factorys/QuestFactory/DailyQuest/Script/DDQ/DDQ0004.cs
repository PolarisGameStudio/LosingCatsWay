using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DDQ0004", menuName = "Factory/Quests/DDQ/Create DDQ0004")]
public class DDQ0004 : DailyQuest
{
    public override void Init()
    {
        //TODO °Ó«°¥[callback
    }

    public void Bind()
    {
        Progress++;

        if (Progress == TargetCount)
        {
            //°Ó«°´îcallback
        }
    }
}
