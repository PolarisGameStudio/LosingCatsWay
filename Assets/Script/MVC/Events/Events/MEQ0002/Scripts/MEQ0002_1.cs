using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MEQ0002_1", menuName = "Factory/Quests/MEQ/Create MEQ0002_1")]
public class MEQ0002_1 : Quest
{
    public override void Init()
    {
        App.system.catchCat.map.OnGotcha += Bind;
    }

    public void Bind()
    {
        Progress++;

        if (Progress == TargetCount)
        {
            App.system.catchCat.map.OnGotcha -= Bind;
        }
    }
}
