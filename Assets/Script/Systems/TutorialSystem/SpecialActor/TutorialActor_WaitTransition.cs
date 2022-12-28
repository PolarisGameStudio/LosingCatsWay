using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class TutorialActor_WaitTransition : TutorialActor
{
    public override void Enter()
    {
        App.system.transition.OnTransitionEnd += Exit;
        base.Enter();
    }

    public override void Exit()
    {
        App.system.transition.OnTransitionEnd -= Exit;
        base.Exit();
    }
}
