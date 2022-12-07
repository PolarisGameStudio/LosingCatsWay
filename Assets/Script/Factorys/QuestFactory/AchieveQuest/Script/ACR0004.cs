using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ACR0004", menuName = "Factory/Quests/ACR/Create ACR0004")]
public class ACR0004 : AchieveQuest
{
    public override void Init()
    {
        App.controller.shelter.OnAdoptCat += BindByValue;
        App.system.tnr.OnAdoptCat += BindByValue;
    }

    public override void CancelBind()
    {
        App.controller.shelter.OnAdoptCat -= BindByValue;
        App.system.tnr.OnAdoptCat -= BindByValue;
    }

    public void BindByValue(object value)
    {
        Progress++;
    }
}