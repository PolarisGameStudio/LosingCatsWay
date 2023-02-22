using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Controller_Cloister : ControllerBehavior
{
    [SerializeField] private GameObject catFlowerObject;

    public Callback OnClose;
    
    #region Basic

    public async Task Init()
    {
        var losingCats = await App.system.cloudSave.LoadCloudLosingCatDatas(App.system.player.PlayerId);
        for (int i = losingCats.Count - 1; i >= 0; i--)
        {
            if (losingCats[i].IsExpired)
            {
                var tmp = losingCats[i];
                losingCats.RemoveAt(i);
                
                if (!tmp.LosingCatStatus.Contains("First")) //不是第一隻才刪掉
                    App.system.cloudSave.DeleteLosingCatData(tmp);
            }
        }
        
        App.model.cloister.LosingCatDatas = losingCats;
    }

    public void Open()
    {
        App.view.cloister.Open();

        if (App.model.cloister.LosingCatDatas.Count > 0)
            Select(0); //Show
        else
            Select(-1); //Hide
    }

    public void Close()
    {
        App.view.cloister.Close();
        OnClose?.Invoke();
    }

    public void OpenDiary()
    {
        App.model.diary.LosingCatData = App.model.cloister.SelectedLosingCatData;
        App.controller.diary.Open();
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
        var data = datas[index];

        datas.RemoveAt(index);
        
        App.system.cloudSave.DeleteLosingCatData(data);
        App.model.cloister.LosingCatDatas = datas;
        
        Select(0);
    }

    public void UseFlower()
    {
        var data = App.model.cloister.SelectedLosingCatData;
        if (data.LosingCatStatus.Contains("Flower"))
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_AlreadyUsePotion);
            return;
        }

        var item = App.factory.itemFactory.GetItem("ISL00001");
        if (item.Count <= 0)
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_NoProps);
            return;
        }

        App.system.confirm.Active(ConfirmTable.Hints_UsePotion, () =>
        {
            item.Count -= 1;
            data.LosingCatStatus.Add("Flower");
            App.system.cloudSave.SaveLosingCatData(data);
            
            //ValueChange
            App.model.cloister.SelectedLosingCatData = data;
            var datas = App.model.cloister.LosingCatDatas;
            App.model.cloister.LosingCatDatas = datas;
        });
    }

    public void OpenMailFromDev()
    {
        App.system.mailFromDev.Open();
    }
    
    #endregion
}
