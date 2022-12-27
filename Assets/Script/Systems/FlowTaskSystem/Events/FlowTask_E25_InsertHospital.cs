using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;

public class FlowTask_E25_InsertHospital : FlowTask_Mask
{
    public override void Enter()
    {
        base.Enter();
    }

    public void Active() //DEBUG ´¡¶¤E25
    {
        App.controller.clinic.OnFunctionComplete += Exit;
        base.uIView.Hide();
        App.system.flowTask.FocusMaskClose();
    }

    public override void Exit()
    {
        App.controller.clinic.OnFunctionComplete -= Exit;
        App.controller.clinic.Close();
        base.Exit();
    }
}
