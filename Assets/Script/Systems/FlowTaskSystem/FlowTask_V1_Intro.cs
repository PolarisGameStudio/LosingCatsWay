using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowTask_V1_Intro : FlowTask
{
    public GameObject introObject;
    public GameObject flag;

    public override void Enter()
    {
        base.Enter();
        introObject.SetActive(true);
        InvokeRepeating("CheckFlag", 30, 0.25F);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void CheckFlag() 
    {
        if (flag.activeSelf)
        {
            CancelInvoke("CheckFlag");
            introObject.SetActive(false);
            Exit();
        }
    }
}
