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
        CloudCatData cloudCatData = await debugToolCat.GetCreateCat(App.system.player.PlayerId, false);

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
