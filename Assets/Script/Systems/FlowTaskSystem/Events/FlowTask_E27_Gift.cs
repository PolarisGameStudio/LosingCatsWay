using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.UI;

public class FlowTask_E27_Gift : FlowTask
{
    public string tutorialId;
    private UIView view;

    public override void Enter()
    {
        base.Enter();

        view = GetComponent<UIView>();
        view.Show();

        //

        App.system.reward.OnClose += Exit;
        // App.system.reward.Open();
    }

    public override void Exit()
    {
        App.system.reward.OnClose -= Exit;
        base.Exit();
    }
}