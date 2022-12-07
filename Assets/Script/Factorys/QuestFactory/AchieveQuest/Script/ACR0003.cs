using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ACR0003", menuName = "Factory/Quests/ACR/Create ACR0003")]
public class ACR0003 : AchieveQuest
{
    public override void Init()
    {
        App.system.cat.OnCatDead += Bind;
    }

    public override void CancelBind()
    {
        App.system.cat.OnCatDead -= Bind;
    }
    
    public void Bind()
    {
        Progress++;
    }
}
