using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

public class FlowTask_E7_ClickFeedRoom : FlowTask_Mask
{
    //private UIView view;
    public GameObject blockRaycastObject;
    
    public override void Enter()
    {
        blockRaycastObject.SetActive(true);
        StartCoroutine(Coroutine());
    }

    IEnumerator Coroutine()
    {
        App.controller.chooseBuild.SelectRoomType(0);
        yield return new WaitForSecondsRealtime(0.5f);
        focusTarget = App.view.chooseBuild.GetChooseRoomItem(1);
        base.Enter();
        blockRaycastObject.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();
    }
}