using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;

public class FlowTask_E36_LevelUp : FlowTask
{
    //public UIView eventView;

    public override void Enter()
    {
        base.Enter();
        //eventView.Show();
        App.system.player.Exp = 0;
        App.system.player.Level++;

        App.system.reward.OnClose += () =>
        {
            App.system.shortcut.ToLobby();
            Exit();
        };
    }

    public override void Exit()
    {
        App.system.reward.OnClose -= () =>
        {
            App.system.shortcut.ToLobby();
            Exit();
        };
        //eventView.Hide();
        base.Exit();
    }
}
