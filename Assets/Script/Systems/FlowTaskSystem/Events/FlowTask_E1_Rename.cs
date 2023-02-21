using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using UnityEngine;

public class FlowTask_E1_Rename : FlowTask
{
    public override void Enter()
    {
        base.Enter();
        App.system.playerRename.Open();
        App.system.playerRename.OnRenameComplete += Exit;
    }

    public override void Exit()
    {
        App.system.playerRename.OnRenameComplete -= Exit;
        base.Exit();
    }
}