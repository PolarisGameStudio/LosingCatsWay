using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActor_AdoptCat : TutorialActor
{
    public override void Enter()
    {
        GetCat();
    }

    private async void GetCat()
    {
        int randomIndex = Random.Range(0, 2);
        var cloudCatDatas = await App.system.cloudSave.LoadCloudCatDatasByOwner($"Location{randomIndex}", 1);
        var cloudCatData = cloudCatDatas.Count > 0 ? cloudCatDatas[0] : null;
        
        if (cloudCatData == null || cloudCatData.CatSurviveData.IsUseToFind || CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays) != 0)
        {
            Invoke("GetCat", 0.25f);
            return;
        }

        CancelInvoke("GetCat");
        App.system.catRename.CantCancel().Active(cloudCatData, () =>
        {
            cloudCatData.CatData.Owner = App.system.player.PlayerId;
            App.system.cloudSave.UpdateCloudCatData(cloudCatData);
            App.system.cat.CreateCatObject(cloudCatData);
            Exit();
        });
        
        base.Enter();
    }
}
