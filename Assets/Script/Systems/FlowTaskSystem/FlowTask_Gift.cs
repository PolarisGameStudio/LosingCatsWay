using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowTask_Gift : FlowTask
{
    public override void Enter()
    {
        //

        App.system.reward.OnClose += Exit;
        // App.system.reward.Open();

        base.Enter();
    }

    public override void Exit()
    {
        App.system.reward.OnClose -= Exit;
        base.Exit();
    }
}
