using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MEQ0002_5", menuName = "Factory/Quests/MEQ/Create MEQ0002_5")]
public class MEQ0002_5 : Quest
{
    public override void Init()
    {
        base.Init();
        App.system.tnr.OnAdoptCat += Bind;
        App.controller.shelter.OnAdoptCat += Bind;
    }

    public void Bind(object value)
    {
        Progress ++;

        if (Progress >= TargetCount)
        {
            App.system.tnr.OnAdoptCat -= Bind;
            App.controller.shelter.OnAdoptCat -= Bind;
        }
    }
}
