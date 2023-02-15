using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MEQ0002_7", menuName = "Factory/Quests/MEQ/Create MEQ0002_7")]
public class MEQ0002_7 : Quest
{
    public override void Init()
    {
        base.Init();
        App.system.tnr.OnDoLigation += Bind;
    }

    public void Bind()
    {
        Progress ++;

        if (Progress >= TargetCount)
        {
            App.system.tnr.OnDoLigation -= Bind;
        }
    }
}
