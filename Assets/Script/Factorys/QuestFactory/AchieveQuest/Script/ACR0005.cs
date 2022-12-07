using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ACR0005", menuName = "Factory/Quests/ACR/Create ACR0005")]
public class ACR0005 : AchieveQuest
{
    public override void Init()
    {
        App.controller.cultive.OnChangeLitter += Bind;
    }

    public override void CancelBind()
    {
        App.controller.cultive.OnChangeLitter -= Bind;
    }

    public void Bind()
    {
        Progress++;
    }
}
