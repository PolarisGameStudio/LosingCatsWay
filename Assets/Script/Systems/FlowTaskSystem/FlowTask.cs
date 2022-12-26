using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;

public class FlowTask : SerializedMonoBehaviour
{
    #region MVC

    protected MyApplication App
    {
        get
        {
            return FindObjectOfType<MyApplication>();
        }
    }

    #endregion

    public string flowId;
    public float exitDelay;
    public bool UseDragCamera;
    public bool UsePinchCamera;
    public bool useBgMask;
    public bool useUiView;
    public bool showLobby;

    [ShowIf("useUiView")] public UIView uIView;

    public virtual void Enter()
    {
        App.system.flowTask.ActiveDragCamera(UseDragCamera);
        App.system.flowTask.ActivePinchCamera(UsePinchCamera);
        App.system.flowTask.ActiveBgMask(useBgMask);

        if (useUiView)
            uIView.Show();

        if (showLobby)
            App.controller.lobby.Open();
        else
            App.controller.lobby.Close();
    }

    public virtual void Exit()
    {
        DOVirtual.DelayedCall(exitDelay, () => 
        {
            if (useUiView)
                uIView.InstantHide();

            App.system.flowTask.NextTask();
        });
    }
}
