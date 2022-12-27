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
        DebugTool_Cat debugToolCat = new DebugTool_Cat();
        debugToolCat.CreateCat(App.system.player.PlayerId, false);

        var cloudCatDatas = await App.system.cloudSave.LoadCloudCatDatas();
        CloudCatData cloudCatData = cloudCatDatas[0];
        
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
