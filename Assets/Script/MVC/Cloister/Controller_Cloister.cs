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
        for (int i = losingCats.Count - 1; i >= 0; i--)
        {
            if (losingCats[i].IsExpired)
            {
                var tmp = losingCats[i];
                App.system.cloudSave.DeleteLosingCatData(tmp);
                losingCats.RemoveAt(i);
            }
        }
        
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
        App.model.diary.LosingCatData = App.model.cloister.SelectedLosingCatData;
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

        if (index == 0)
        {
            catFlowerObject.SetActive(false);
            OpenZeroPointFiveLetter();
            App.model.cloister.SelectedIndex = 0;
            return;
        }

        App.model.cloister.SelectedIndex = index;
        App.model.cloister.SelectedLosingCatData = App.model.cloister.LosingCatDatas[index - 1];
    }

    public void Remove(int index)
    {
        if (index <= 0)
            return;

        index -= 1; //傳入的是卡片所在順序，所以減掉0.5的順位

        var datas = App.model.cloister.LosingCatDatas;
        var dataToRemove = datas[index];
        datas.RemoveAt(index);
        
        App.system.cloudSave.DeleteLosingCatData(dataToRemove);
        App.model.cloister.LosingCatDatas = datas;
        
        Select(0);
    }

    public void UseFlower()
    {
        var data = App.model.cloister.SelectedLosingCatData;
        if (data.CatDiaryData.UsedFlower)
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix);
            return;
        }

        var item = App.factory.itemFactory.GetItem("ISL00001");
        if (item.Count <= 0)
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix);
            return;
        }

        App.system.confirm.Active(ConfirmTable.RefreshConfirm, () =>
        {
            item.Count -= 1;
            data.CatDiaryData.UsedFlower = true;
            App.system.cloudSave.UpdateLosingCatDiaryData(data);
            
            //ValueChange
            App.model.cloister.SelectedLosingCatData = data;
            var datas = App.model.cloister.LosingCatDatas;
            App.model.cloister.LosingCatDatas = datas;
        });
    }

    private void OpenZeroPointFiveLetter()
    {
        //
    }
    
    #endregion
}
