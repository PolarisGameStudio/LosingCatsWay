using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActor_Rewards : TutorialActor
{
    [SerializeField] private Reward[] rewards;

    public override void Enter()
    {
        App.system.reward.OnClose += Exit;
        base.Enter();
        App.system.reward.Open(rewards);
    }

    public override void Exit()
    {
        App.system.reward.OnClose -= Exit;
        base.Exit();
    }
}
