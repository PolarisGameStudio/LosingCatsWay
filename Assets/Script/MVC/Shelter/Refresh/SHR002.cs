using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Factory/Quests/SHR/002")]
public class SHR002 : Quest
{
    public override void Init()
    {
        App.controller.shelter.OnAdsRefresh += AddProgress;
    }

    private void AddProgress()
    {
        Progress++;
        if (IsReach)
            App.controller.shelter.OnAdsRefresh -= AddProgress;
    }
}
