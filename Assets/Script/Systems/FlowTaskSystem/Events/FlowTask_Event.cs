using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;

public class FlowTask_Event : FlowTask
{
    public UIView eventView;

    public override void Enter()
    {
        base.Enter();
        eventView.Show();
    }

    public override void Exit()
    {
        base.Exit();
        eventView.InstantHide();
    }
}
