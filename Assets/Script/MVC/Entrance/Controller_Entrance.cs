using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controller_Entrance : ControllerBehavior
{
    public Callback OnOpen;
    public Callback OnClose;

    public void Init()
    {
        App.system.openFlow.AddAction(Open);
    }

    public void Open()
    {
        App.view.entrance.Open();

        if (App.model.entrance.OpenType == 0)
        {
            App.model.entrance.Cats = App.system.cat.GetCats();
            return;
        }

        if (App.model.entrance.OpenType == 1) //有死掉的貓
        {
            App.model.entrance.DeadCat = App.system.cat.MyDeadCats()[0];
        }
    }

    public void Close()
    {
        App.view.entrance.Close();
        App.system.openFlow.NextAction();
    }
}
