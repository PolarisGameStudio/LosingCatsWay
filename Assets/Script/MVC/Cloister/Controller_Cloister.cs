using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Cloister : ControllerBehavior
{
    [SerializeField] private GameObject catFlowerObject;

    #region Basic

    public async void Init()
    {
        var losingCats = await App.system.cloudSave.LoadCloudLosingCatDatas(App.system.player.PlayerId);
        App.model.cloister.LosingCatDatas = losingCats;
    }

    public void Open()
    {
        App.controller.greenHouse.LockGyro();
        App.view.cloister.Open();

        if (App.model.cloister.LosingCatDatas.Count > 0)
            Select(0); //Show
        else
            Select(-1); //Hide
    }

    public void Close()
    {
        App.view.cloister.Close();
    }

    public void CloseToGreenHouse()
    {
        Close();
        App.controller.greenHouse.UnlockGyro();
    }

    public void OpenDiary()
    {
        App.controller.diary.Open();
    }

    public void CloseDiary()
    {
        App.controller.diary.Close();
    }

    #endregion

    #region Cloister
    
    public void Select(int index)
    {
        if (index == -1)
        {
            catFlowerObject.SetActive(false);
            return;
        }
        
        if (App.model.cloister.SelectedIndex == index)
            return;

        App.model.cloister.SelectedIndex = index;
        App.model.cloister.SelectedLosingCatData = App.model.cloister.LosingCatDatas[index];
    }

    public void UseFlower()
    {
        var data = App.model.cloister.SelectedLosingCatData;
        
        if (data.CatDiaryData.UsedFlower)
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix);
            return;
        }
        
        //TODO UsedFlowerItem.Count

        App.system.confirm.Active(ConfirmTable.RefreshConfirm, () =>
        {
            //TODO UsedFlowerItem.Count -1
            data.CatDiaryData.UsedFlower = true;
            App.system.cloudSave.UpdateLosingCatDiaryData(data);
            
            //ValueChange
            App.model.cloister.SelectedLosingCatData = data;
            var datas = App.model.cloister.LosingCatDatas;
            App.model.cloister.LosingCatDatas = datas;
        });
    }
    
    #endregion
}
