using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

public class FlowTask_E40_ClickGameRoom : FlowTask
{
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        App.controller.chooseBuild.Select(0);
        base.Exit();
    }
}