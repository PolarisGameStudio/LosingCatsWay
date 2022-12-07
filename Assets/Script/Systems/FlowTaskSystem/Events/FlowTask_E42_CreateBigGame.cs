using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;

public class FlowTask_E42_CreateBigGame : FlowTask
{
    public UIView eventView;

    public override void Enter()
    {
        base.Enter();
        eventView.Show();

        var cat = App.system.cat.GetCats()[0];
        App.controller.followCat.Select(cat);
        cat.OpenBigGame();
    }

    public override void Exit()
    {
        base.Exit();
        eventView.Hide();
    }
}
