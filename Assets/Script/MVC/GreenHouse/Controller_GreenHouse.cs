using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using I2.Parallax;
using UnityEngine;

public class Controller_GreenHouse : ControllerBehavior
{
    [SerializeField] private I2Parallax_Layer[] parallaxLayers;
    [SerializeField] private UIView chooseFlower;

    #region Basic

    public void Open()
    {
        App.system.bgm.FadeIn().Play("GreenHouse");
        UnlockGyro();
        
        App.view.greenHouse.Open();
        chooseFlower.InstantHide();
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

    #region ChooseFlower

    public void ChooseFlower(int index)
    {
        chooseFlower.Show();
    }

    public void CloseChooseFlower()
    {
        chooseFlower.Hide();
    }

    #endregion
    
    public void OpenSideMenu()
    {
        App.system.sideMenu.OnOpen = LockGyro;
        App.system.sideMenu.OnOnlyClose = UnlockGyro;
        App.system.sideMenu.Open();
    }
}
