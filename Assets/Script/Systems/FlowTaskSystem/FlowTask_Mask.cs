using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlowTask_Mask : FlowTask
{
    public RectTransform focusTarget;
    public bool fitTarget;
    public UnityEvent onClick;

    public override void Enter()
    {
        base.Enter();
        App.system.flowTask.FocusMaskOpen(focusTarget, () => onClick?.Invoke());
    }

    public override void Exit()
    {
        App.system.flowTask.FocusMaskClose();
        base.Exit();
    }
}