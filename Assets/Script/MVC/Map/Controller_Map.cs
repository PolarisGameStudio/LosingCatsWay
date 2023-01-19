using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Controller_Map : ControllerBehavior
{
    public void Open()
    {
        App.system.bgm.FadeIn().Play("Map");
        App.view.map.Open();
    }

    public void OpenShelter()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0.5f, () =>
        {
            App.view.map.Close();
            App.controller.shelter.Open();
        });
    }

    public void OpenFindCat(int index)
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0.5f, () =>
        {
            App.view.map.Close();
            App.system.findCat.ActiveGate(index);
        });
    }

    public void OpenShop()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0.5f, () =>
        {
            App.view.map.Close();
            App.controller.shop.Open();
        });
    }

    public void OpenHospital()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0.15f, () =>
        {
            App.view.map.Close();
            App.controller.clinic.Open();
        });
    }

    public void OpenGreenHouse()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0.5f, () =>
        {
            App.view.map.Close();
            App.controller.greenHouse.Open();
        });
    }

    public void OpenPark()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0.5f, () =>
        {
            App.view.map.Close();
            App.controller.park.Open();
        });
    }

    public void Close()
    {
        App.system.cat.PauseCatsGame(false);
        App.system.bgm.FadeOut();
        App.system.room.OpenRooms();
        App.system.transition.Active(0, () =>
        {
            App.view.map.Close();
            DOVirtual.DelayedCall(0.175f, App.controller.lobby.Open);
        });
    }
}
