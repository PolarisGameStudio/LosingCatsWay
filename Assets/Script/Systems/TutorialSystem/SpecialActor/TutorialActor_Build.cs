using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActor_Build : TutorialActor_Unmask
{
    public override void Exit()
    {
        if (!App.model.build.CanBuild)
            return;
        App.controller.build.Build();
        App.controller.build.Close();
        base.Exit();
    }
}
