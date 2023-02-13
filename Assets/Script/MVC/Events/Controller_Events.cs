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
        
        if (!App.system.openFlow.isEnd)
            App.system.openFlow.NextAction();
        else
            App.controller.lobby.SetBuffer();
    }

    public void Select(int index)
    {
        if (App.model.events.SelectIndex == index) return;
        App.system.soundEffect.Play("ED00010");
        App.model.events.SelectIndex = index;
        myEvents[index].Open();
    }
}
