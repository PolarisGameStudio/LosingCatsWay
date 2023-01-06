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
    private bool isActive;
    private bool isEnd;

    public void Init()
    {
        if (openFlowActions.Count <= 0) return;
        StartAction();
    }

    public void AddAction(UnityAction action)
    {
        openFlowActions.Add(action);
    }

    // public void RemoveAction(UnityAction action)
    // {
    //     openFlowActions.Remove(action);
    // }

    [Button]
    public void StartAction()
    {
        flowIndex = 0;
        isEnd = false;
        isActive = true;
        NextAction();
    }

    public void NextAction()
    {
        if (isEnd) return;
        if (!isActive) return;

        if (flowIndex >= openFlowActions.Count)
        {
            isEnd = true;
            isActive = false;
            print("End of actions.");
            return;
        }

        openFlowActions[flowIndex].Invoke();
        flowIndex++;
    }
}
