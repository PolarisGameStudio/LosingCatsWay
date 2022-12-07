using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

public class FlowTask_E37_ClickBuild : FlowTask
{
    private UIView view;

    public override void Enter()
    {
        view = GetComponent<UIView>();
        view.Show();
    }

    public override void Exit()
    {
        App.controller.lobby.OpenBuildMode();
        view.InstantHide();

        DOVirtual.DelayedCall(0.4f, () =>
        {
            base.Exit();
        });
    }
}