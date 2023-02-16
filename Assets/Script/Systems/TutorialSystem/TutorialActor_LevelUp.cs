using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActor_LevelUp : TutorialActor
{
    public override void Enter()
    {
        App.system.reward.OnClose += Exit;
        App.system.player.Level = 2;
        App.system.player.Exp = 0;
        App.model.lobby.LevelBuffer = 2;
        App.model.lobby.TmpLevel = 2;
        base.Enter();
    }

    public override void Exit()
    {
        App.system.reward.OnClose -= Exit;
        base.Exit();
    }
}
