using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Events : ControllerBehavior
{
    public MyEvent[] myEvents;

    public void Init()
    {
        App.system.openFlow.AddAction(Open);
        for (int i = 0; i < myEvents.Length; i++)
            myEvents[i].Init();
    }

    [HorizontalGroup("Split", 0.5f)]
    [Button(30)]
    public void Open()
    {
        App.view.events.Open();
        Select(0);
    }

    [HorizontalGroup("Split", 0.5f)]
    [Button(30)]
    public void Close()
    {
        App.view.events.Close();
        App.system.openFlow.NextAction();
    }

    public void Select(int index)
    {
        if (App.model.events.SelectIndex == index) return;
        App.model.events.SelectIndex = index;
        myEvents[index].Open();
    }
}
