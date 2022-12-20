using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Entrance : ControllerBehavior
{
    [SerializeField] private GameObject leftObject, rightObject;
    
    public void Init()
    {
        App.system.openFlow.AddAction(Open);
    }

    public void Open()
    {
        App.view.entrance.Open();

        if (App.model.entrance.OpenType == 0)
        {
            App.model.entrance.Cats = App.system.cat.GetCats();
            return;
        }

        if (App.model.entrance.OpenType == 1) //有死掉的貓
        {
            App.model.entrance.DeadCat = App.system.cat.MyDeadCats()[0];
        }
    }

    public void Close()
    {
        App.view.entrance.CloseChooseDiary();
        App.view.entrance.Close();
        App.system.openFlow.NextAction();
    }

    public void OpenChooseDiary()
    {
        App.model.entrance.SelectedDiaryIndex = 0;
        App.model.entrance.AllDeadCats = new List<Cat>(App.system.cat.MyDeadCats());
        leftObject.SetActive(App.model.entrance.AllDeadCats.Count > 1);
        rightObject.SetActive(App.model.entrance.AllDeadCats.Count > 1);
        RefreshDiary();
        App.view.entrance.OpenChooseDiary();
    }

    public void ToLeft()
    {
        var losingCats = App.model.cloister.LosingCatDatas;
        int index = App.model.entrance.SelectedDiaryIndex;
        index--;

        if (index < 0)
            index = losingCats.Count - 1;
        
        App.model.entrance.SelectedDiaryIndex = index;
    }

    public void ToRight()
    {
        var losingCats = App.model.cloister.LosingCatDatas;
        int index = App.model.entrance.SelectedDiaryIndex;
        index++;
        
        if (index > losingCats.Count - 1)
            index = 0;
        
        App.model.entrance.SelectedDiaryIndex = index;
    }

    private void RefreshDiary()
    {
        int index = App.model.entrance.SelectedDiaryIndex;
        var deadCats = App.model.entrance.AllDeadCats;
        
        List<Cat> previousCats = new List<Cat>();
        List<Cat> nextCats = new List<Cat>();
        for (int i = 0; i < index; i++)
            previousCats.Add(deadCats[i]);
        for (int i = index; i < deadCats.Count; i++)
            nextCats.Add(deadCats[i]);

        nextCats.AddRange(previousCats);
        App.model.entrance.AllDeadCats = nextCats;
    }

    public void ReadDiary()
    {
        var cat = App.model.entrance.SelectedDeadCat;
        var losingCatData = new CloudLosingCatData();
        losingCatData.CatData = cat.cloudCatData.CatData;
        losingCatData.CatDiaryData = cat.cloudCatData.CatDiaryData;
        losingCatData.CatSkinData = cat.cloudCatData.CatSkinData;
        
        App.model.cloister.SelectedLosingCatData = losingCatData; //TODO Diary也要有自己的SelectedLosingCatData
        App.controller.diary.Open();
    }
}
