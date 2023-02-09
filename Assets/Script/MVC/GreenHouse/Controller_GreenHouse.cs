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

    public void Init()
    {
        App.view.greenHouse.RefreshGreenHousePlace();
    }

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
        App.view.greenHouse.Close();
    }

    public void CloseByOpenMap()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            Close();
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
        int positionIndex = index;
        int pageIndex = 0; 

        var greenHouseData =
            App.model.greenHouse.GreenHouseDatas.Find(x => x.Page == pageIndex && x.Position == positionIndex);

        if (greenHouseData != null)
        {
            App.system.confirm.Active(ConfirmTable.Fix, () =>
            {
                App.model.greenHouse.GreenHouseDatas.Remove(greenHouseData);
                App.view.greenHouse.RefreshGreenHousePlace();
            });
            chooseFlower.Hide();
            return;
        }
        
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

        App.model.greenHouse.ChooseFlowers = cloudLosingCatDatas;
        App.model.greenHouse.selectPositionIndex = index;
    }

    public void ChooseFlower(int index)
    {
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            int flowerIndex = index;
            int positionIndex = App.model.greenHouse.selectPositionIndex;

            GreenHouseData greenHouseData = new GreenHouseData();
            greenHouseData.Page = 0;
            greenHouseData.Position = positionIndex;
            greenHouseData.FlowerID = App.model.greenHouse.ChooseFlowers[flowerIndex].CatData.CatId;

            App.model.greenHouse.GreenHouseDatas.Add(greenHouseData);
            App.view.greenHouse.RefreshGreenHousePlace();
            CloseChooseFlower();
        });
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
