using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

public class TutorialActor_AdoptCat : TutorialActor
{
    public override void Enter()
    {
        GetCat();
    }

    private void GetCat()
    {
        DebugTool_Cat debugToolCat = new DebugTool_Cat();
        CloudCatData cloudCatData = debugToolCat.GetCloudCatData(App.system.player.PlayerId, false);
        cloudCatData.CatDiaryData.AdoptLocation = "OutSide";

        App.system.quest.QuestProgressData["ACR0001"]++;
        
        App.system.catRename.CantCancel().Active(cloudCatData, "Shelter", () =>
        {
            cloudCatData.CatData.Owner = App.system.player.PlayerId;
            App.system.cat.CreateCatObject(cloudCatData);
            Exit();
        });
        
        base.Enter();
    }
}
