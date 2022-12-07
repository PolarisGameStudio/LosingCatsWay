using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

public class FlowTask_E39_ClickRoomTypeGame : FlowTask
{
    private UIView view;

    public override void Enter()
    {
        view = GetComponent<UIView>();
        view.Show();
    }

    public override void Exit()
    {
        DOVirtual.DelayedCall(0.25f, () =>
        {
            //App.controller.chooseBuild.SelectRoomBoughtType(0);
            App.controller.chooseBuild.SelectRoomType(1);

            //App.controller.chooseBuild.RefreshSelectedRooms();
        });

        view.InstantHide();

        DOVirtual.DelayedCall(0.4f, () =>
        {
            base.Exit();
        });
    }
}