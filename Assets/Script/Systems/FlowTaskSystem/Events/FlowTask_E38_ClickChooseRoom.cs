using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

public class FlowTask_E38_ClickChooseRoom : FlowTask
{
    private UIView view;

    public override void Enter()
    {
        view = GetComponent<UIView>();
        view.Show();
    }

    public override void Exit()
    {
        App.controller.build.OpenChooseBuild();
        view.InstantHide();

        DOVirtual.DelayedCall(0.4f, () =>
        {
            base.Exit();
        });
    }
}