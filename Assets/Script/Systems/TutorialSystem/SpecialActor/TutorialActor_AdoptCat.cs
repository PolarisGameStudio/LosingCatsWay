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

    private async void GetCat()
    {
        DebugTool_Cat debugToolCat = new DebugTool_Cat();
        CloudCatData cloudCatData = debugToolCat.GetCreateCat(App.system.player.PlayerId, false);
        
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Cats").Document(cloudCatData.CatData.CatId);
        await docRef.SetAsync(cloudCatData);

        App.system.catRename.CantCancel().Active(cloudCatData, "Shelter", () =>
        {
            cloudCatData.CatData.Owner = App.system.player.PlayerId;
            App.system.cloudSave.UpdateCloudCatData(cloudCatData);
            App.system.cat.CreateCatObject(cloudCatData);
            Exit();
        });
        
        base.Enter();
    }
}
