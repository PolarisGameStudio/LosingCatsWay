using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DDQ0002", menuName = "Factory/Quests/DDQ/Create DDQ0002")]
public class DDQ0002 : DailyQuest
{
    public override void Init()
    {
        //TODO �ӫ��[callback
    }

    public void Bind()
    {
        Progress++;

        if (Progress == TargetCount)
        {
            //�ӫ���callback
        }
    }
}
