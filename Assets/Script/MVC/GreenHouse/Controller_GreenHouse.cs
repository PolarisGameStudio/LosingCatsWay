using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_GreenHouse : ControllerBehavior
{
    #region Basic

    public void Open()
    {
        App.system.bgm.FadeIn().Play("GreenHouse");
        App.view.greenHouse.Open();
    }

    public void Close()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            App.view.greenHouse.Close();
            App.controller.map.Open();
        });
    }

    #endregion
}
