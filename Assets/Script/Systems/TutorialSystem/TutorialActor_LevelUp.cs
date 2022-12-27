using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActor_LevelUp : TutorialActor
{
    public override void Enter()
    {
        App.system.reward.OnClose += Exit;
        App.system.player.AddExp(App.system.player.NextLevelExp);
        base.Enter();
    }

    public override void Exit()
    {
        App.system.reward.OnClose -= Exit;
        base.Exit();
    }
}
