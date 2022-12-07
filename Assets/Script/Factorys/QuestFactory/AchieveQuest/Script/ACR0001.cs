using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ACR0001", menuName = "Factory/Quests/ACR/Create ACR0001")]
public class ACR0001 : AchieveQuest
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
        CloudCatData cloudCatData = (CloudCatData)value;
        int catAgeLevel = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays);

        if (catAgeLevel == 0)
            Progress++;
    }
}