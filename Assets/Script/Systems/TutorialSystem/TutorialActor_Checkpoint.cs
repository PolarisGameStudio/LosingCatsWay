using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActor_Checkpoint : TutorialActor
{
    public override void Enter()
    {
        base.Enter();
        App.system.cloudSave.SaveCloudSaveData(); //TODO 優化教學存檔
        Exit();
    }
}
