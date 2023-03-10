using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Controller_Map : ControllerBehavior
{
    public void Init()
    {
        CheckGreenHouseUnlock();
    }

    public void Open()
    {
        App.system.bgm.FadeIn().Play("Map");
        App.view.map.Open();
    }

    public void OpenShelter()
    {
        App.system.soundEffect.Play("ED00004");
        App.system.bgm.FadeOut();
        App.system.transition.Active(0.15f, () =>
        {
            App.view.map.Close();
            App.controller.shelter.Open();
        });
    }

    public void OpenFindCat(int index)
    {
        App.system.soundEffect.Play("ED00004");
        App.system.bgm.FadeOut();
        App.system.transition.Active(0.5f, () =>
        {
            App.view.map.Close();
            App.system.findCat.ActiveGate(index);
        });
    }

    public void OpenShop()
    {
        App.system.soundEffect.Play("ED00004");
        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            App.view.map.Close();
            App.controller.shop.Open();
        });
    }

    public void OpenHospital()
    {
        App.system.soundEffect.Play("ED00004");
        App.system.bgm.FadeOut();
        App.system.transition.Active(0.15f, () =>
        {
            App.view.map.Close();
            App.controller.hospital.Open();
        });
    }

    public void OpenGreenHouse()
    {
        App.system.soundEffect.Play("ED00004");
        App.system.bgm.FadeOut();
        App.system.transition.Active(0.5f, () =>
        {
            App.view.map.Close();
            App.controller.greenHouse.Open();
        });
    }

    public void OpenPark()
    {
        App.system.soundEffect.Play("ED00004");
        App.system.bgm.FadeOut();
        App.system.transition.Active(0.5f, () =>
        {
            App.view.map.Close();
            App.controller.park.Open();
        });
    }

    public void OpenWorldData()
    {
        App.system.soundEffect.Play("ED00004");
        App.controller.worldData.Open();
    }

    public void Close()
    {
        App.view.map.Close();
    }

    public void CloseByOpenLobby()
    {
        App.system.cat.PauseCatsGame(false);
        App.system.bgm.FadeOut();
        App.system.room.OpenRooms();
        App.system.transition.Active(0, () =>
        {
            Close();
            App.system.grid.SetCameraToOrigin();
            DOVirtual.DelayedCall(0.175f, App.controller.lobby.Open);
        });
    }

    private void CheckGreenHouseUnlock()
    {
        View_Map map = App.view.map;

        if (App.system.player.CatDeadCount == 0)
        {
            map.greenHouseMask.SetActive(true);
            map.greenHouseSkeletonGraphic.timeScale = 0;
            map.greenHouseButton.interactable = false;
        }
        else
        {
            map.greenHouseMask.SetActive(false);
            map.greenHouseSkeletonGraphic.timeScale = 1;
            map.greenHouseButton.interactable = true;
        }
    }

    // unlock
    public void PlayUnlockGreenHouse()
    {
        App.view.map.greenHouseUnlockEffect.SetActive(true);
        CheckGreenHouseUnlock();
        DOVirtual.DelayedCall(3.5f, () => { App.view.map.greenHouseUnlockEffect.SetActive(false); });
    }
}