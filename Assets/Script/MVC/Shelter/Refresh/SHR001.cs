using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Factory/Quests/SHR/001")]
public class SHR001 : Quest
{
    public override void Init()
    {
        App.controller.shelter.OnFreeRefresh += AddProgress;
    }

    private void AddProgress()
    {
        Progress++;
        if (IsReach)
            App.controller.shelter.OnFreeRefresh -= AddProgress;
    }
}
