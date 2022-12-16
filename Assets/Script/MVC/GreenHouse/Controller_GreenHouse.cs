using System.Collections;
using System.Collections.Generic;
using I2.Parallax;
using UnityEngine;

public class Controller_GreenHouse : ControllerBehavior
{
    [SerializeField] private I2Parallax_Layer[] parallaxLayers;
    
    #region Basic

    public void Open()
    {
        App.system.bgm.FadeIn().Play("GreenHouse");
        UnlockGyro();
        
        App.view.greenHouse.Open();
    }

    public void Close()
    {
        LockGyro();
        
        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            App.view.greenHouse.Close();
            App.controller.map.Open();
        });
    }

    #endregion

    #region Gyro

    public void LockGyro()
    {
        for (int i = 0; i < parallaxLayers.Length; i++)
        {
            parallaxLayers[i].UpdateLayer(Vector2.zero);
            parallaxLayers[i].enabled = false;
        }
    }

    public void UnlockGyro()
    {
        for (int i = 0; i < parallaxLayers.Length; i++)
        {
            parallaxLayers[i].UpdateLayer(Vector2.zero);
            parallaxLayers[i].enabled = true;
        }
    }

    #endregion

    public void OpenSideMenu()
    {
        App.system.sideMenu.OnOpen = LockGyro;
        App.system.sideMenu.OnOnlyClose = UnlockGyro;
        App.system.sideMenu.Open();
    }
}
