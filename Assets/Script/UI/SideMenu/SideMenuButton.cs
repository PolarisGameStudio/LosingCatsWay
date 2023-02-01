using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMenuButton : MvcBehaviour
{
    public void OpenSideMenu()
    {
        App.system.sideMenu.Open();
    }
}
