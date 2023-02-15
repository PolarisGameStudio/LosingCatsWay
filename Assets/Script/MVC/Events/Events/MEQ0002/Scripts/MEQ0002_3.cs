using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MEQ0002_3", menuName = "Factory/Quests/MEQ/Create MEQ0002_3")]
public class MEQ0002_3 : Quest
{
    public override void Init()
    {
        base.Init();
        App.system.myTime.OnFirstLogin += Bind;
    }

    public void Bind()
    {
        Progress++;

        if (Progress == TargetCount)
        {
            App.system.myTime.OnFirstLogin -= Bind;
        }
    }
}
