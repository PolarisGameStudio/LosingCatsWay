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

    public void OpenCloister()
    {
        App.controller.cloister.Open();
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

    public void OpenChooseFlower(int index)
    {
        chooseFlower.Show();

        List<CloudLosingCatData> cloudLosingCatDatas =
            App.model.cloister.LosingCatDatas.FindAll(x => x.LosingCatStatus.Contains("Flower"));

        List<GreenHouseData> greenHouseDatas = App.model.greenHouse.GreenHouseDatas;
        for (int i = 0; i < greenHouseDatas.Count; i++)
        {
            string id = greenHouseDatas[i].FlowerID;

            for (int j = 0; j < cloudLosingCatDatas.Count; j++)
            {
                var cloudLosingCatData = cloudLosingCatDatas[i];

                if (cloudLosingCatData.CatData.CatId.Equals(id))
                    cloudLosingCatDatas.Remove(cloudLosingCatData);
            }
        }

        print(cloudLosingCatDatas.Count);
        print(greenHouseDatas.Count);

        App.model.greenHouse.ChooseFlowers = cloudLosingCatDatas;
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
