using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OpenFlowSystem : MvcBehaviour
{
    private List<UnityAction> openFlowActions = new List<UnityAction>();
    private int flowIndex;
    public bool isEnd;

    public void Init()
    {
        if (openFlowActions.Count <= 0)
            return;
        StartAction();
    }

    public void AddAction(UnityAction action)
    {
        openFlowActions.Add(action);
    }

    [Button]
    public void StartAction()
    {
        flowIndex = 0;
        isEnd = false;
        NextAction();
    }

    public void NextAction()
    {
        if (isEnd)
            return;

        if (flowIndex >= openFlowActions.Count)
        {
            isEnd = true;
            App.controller.lobby.SetBuffer();
            return;
        }

        openFlowActions[flowIndex].Invoke();
        flowIndex++;
    }
}
