using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Controller_Park : ControllerBehavior
{
    [Button]
    public void Open()
    {
        App.view.park.Open();
    }

    [Button]
    public void Close()
    {
        App.view.park.Close();
    }
}
