using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;

public class ShortcutSystem : MvcBehaviour
{
    private void CloseAll()
    {
        App.controller.bag.Close();
        App.controller.information.Close();
        App.controller.followCat.Close();
        App.controller.feed.Close();
        App.controller.shelter.Close();
        App.controller.hospital.Close();
        App.controller.friend.Close();
        App.controller.shop.Close();
        App.controller.greenHouse.Close();
        App.controller.map.Close();
    }
    
    public void ToLobby()
    {
        App.system.transition.Active(0, () =>
        {
            CloseAll();
            App.controller.lobby.Open();
            App.system.grid.SetCameraToOrigin();
        });
    }
}
