using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.UI;

public class FlowTask_E28_AfterGift : FlowTask
{
    private UIView view;

    public override void Enter()
    {
        view = GetComponent<UIView>();
        view.Show();
    }

    public override void Exit()
    {
        App.system.shortcut.ToLobby();
        view.InstantHide();

        DOVirtual.DelayedCall(0.4f, () =>
        {
            base.Exit();
        });
    }

}