using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

public class FlowTask_E29_ClickFeed : FlowTask
{
    private UIView view;

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        App.controller.feed.Open();
        view.InstantHide();

        DOVirtual.DelayedCall(0.4f, () =>
        {
            base.Exit();
        });
    }
}