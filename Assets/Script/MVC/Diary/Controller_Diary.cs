using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Diary : ControllerBehavior
{
    [SerializeField] private GameObject leftButtonObject;
    [SerializeField] private GameObject rightButtonObject;
    
    public void Open()
    {
        App.view.diary.Open();
        ToPage(-1);
    }

    public void Close()
    {
        App.view.diary.Close();
        App.controller.cloister.Open();
    }

    private void ToPage(int index)
    {
        var datas = App.model.diary.LosingCatData.CatDiaryData.DiaryDatas;
        
        if (index < 0)
        {
            leftButtonObject.SetActive(false);
            rightButtonObject.SetActive(true);
        }
        else if (index >= datas.Count - 1)
        {
            leftButtonObject.SetActive(true);
            rightButtonObject.SetActive(false);
        }
        else
        {
            leftButtonObject.SetActive(true);
            rightButtonObject.SetActive(true);
        }
        
        App.model.diary.PageIndex = index;
        if (index >= 0)
            App.model.diary.SelectedDiaryData = datas[index];
    }

    public void PageRight()
    {
        int index = App.model.diary.PageIndex + 1;
        ToPage(index);
    }

    public void PageLeft()
    {
        int index = App.model.diary.PageIndex - 1;
        ToPage(index);
    }
}
