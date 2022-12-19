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
        var losingCats = App.model.cloister.LosingCatDatas;
        
        leftObject.SetActive(losingCats.Count > 1);
        rightObject.SetActive(losingCats.Count > 1);
        
        CloudLosingCatData centerCatData = losingCats[index];
        CloudLosingCatData leftCatData = null;
        CloudLosingCatData rightCatData = null;

        //Left
        int previous = index - 1;
        if (previous < 0)
            previous = losingCats.Count - 1;
        //Right
        int next = index + 1;
        if (next > losingCats.Count - 1)
            next = 0;
        
        if (losingCats.Count > 1)
        {
            leftCatData = losingCats[previous];
            rightCatData = losingCats[next];
        }

        App.model.entrance.LeftCatData = leftCatData;
        App.model.entrance.RightCatData = rightCatData;
        App.model.entrance.CenterCatData = centerCatData;
    }

    public void ReadDiary()
    {
        App.model.cloister.SelectedLosingCatData = App.model.entrance.CenterCatData; //TODO Diary也要有自己的SelectedLosingCatData
        App.controller.diary.Open();
    }
}
