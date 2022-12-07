using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.UI;

public class FlowTask_E41_Build : FlowTask
{
    public Button targetButton;
    public Button okButton;

    public GameObject cancelButton;

    private UIView view;

    public override void Enter()
    {
        base.Enter();

        view = GetComponent<UIView>();
        view.Show();

        cancelButton.SetActive(false);

        InvokeRepeating("FitTargetButton", 0, 0.25f);
    }


    public override void Exit()
    {
        CancelInvoke("FitTargetButton");

        view.InstantHide();
        cancelButton.SetActive(true);
        App.controller.build.Build();
        App.controller.build.Close();

        DOVirtual.DelayedCall(0.4f, () => { base.Exit(); });
    }

    private void FitTargetButton()
    {
        okButton.interactable = targetButton.interactable;
    }
}