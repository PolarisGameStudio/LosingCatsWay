using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Entrance : ControllerBehavior
{
    [SerializeField] private GameObject leftObject, rightObject;
    public PitchShift pitchShift;

    public void Init()
    {
        App.system.openFlow.AddAction(Open);
    }
    
    public void Open()
    {
        App.view.entrance.Open();
        
        if (App.model.entrance.OpenType == 0)
        {
            App.model.entrance.OpenType = 0; // 刷新Value
            App.model.entrance.Cats = App.system.cat.GetCats();
            // return;
        }

        if (App.model.entrance.OpenType == 1) //有死掉的貓
            App.view.entrance.PlayCatDead();
    }

    public void Close()
    {
        App.controller.cloister.OnClose -= Close;
        
        App.view.entrance.Close();
        App.model.entrance.OpenType = 0;
        if (!App.system.openFlow.isEnd)
            App.system.openFlow.NextAction();
    }

    public void CloseDeadEffect()
    {
        App.view.entrance.CloseDeadEffect();
    }

    public void OpenChooseDiary()
    {
        App.model.entrance.SelectedDiaryIndex = 0;
        
        leftObject.SetActive(App.model.entrance.LosingCatDatas.Count > 1);
        rightObject.SetActive(App.model.entrance.LosingCatDatas.Count > 1);
        RefreshDiary();
        App.view.entrance.OpenChooseDiary();
    }

    private void CloseChooseDiary()
    {
        App.view.entrance.CloseChooseDiary();
    }

    public void ToLeft()
    {
        var losingCatDatas = App.model.entrance.LosingCatDatas;
        int index = App.model.entrance.SelectedDiaryIndex;
        index--;

        if (index < 0)
            index = losingCatDatas.Count - 1;
        
        App.model.entrance.SelectedDiaryIndex = index;
        RefreshDiary();
    }

    public void ToRight()
    {
        var losingCatDatas = App.model.entrance.LosingCatDatas;
        int index = App.model.entrance.SelectedDiaryIndex;
        index++;
        
        if (index > losingCatDatas.Count - 1)
            index = 0;
        
        App.model.entrance.SelectedDiaryIndex = index;
        RefreshDiary();
    }

    private void RefreshDiary()
    {
        int index = App.model.entrance.SelectedDiaryIndex;
        var losingCatDatas = App.model.entrance.LosingCatDatas;
        
        CloudLosingCatData center = losingCatDatas[index];
        CloudLosingCatData previousData = null;
        CloudLosingCatData nextData = null;
        
        if (losingCatDatas.Count > 1)
        {
            int previousIndex = index - 1;
            int nextIndex = index + 1;
            
            if (nextIndex > losingCatDatas.Count - 1)
                nextIndex = 0;
            if (previousIndex < 0)
                previousIndex = losingCatDatas.Count - 1;
            
            previousData = losingCatDatas[previousIndex];
            nextData = losingCatDatas[nextIndex];
        }
        
        //0中間 1左邊 2右邊
        List<CloudLosingCatData> tmpDatas = new List<CloudLosingCatData>();
        tmpDatas.Add(center);
        tmpDatas.Add(previousData);
        tmpDatas.Add(nextData);
        
        App.model.entrance.SortedLosingCatDatas = tmpDatas;
    }

    public void ReadDiary()
    {
        CloseChooseDiary();
        App.view.entrance.Close(); // 不要觸發NextAction
        
        var index = App.model.entrance.SelectedDiaryIndex;
        var losingCatData = App.model.entrance.LosingCatDatas[index];
        App.model.diary.LosingCatData = losingCatData;
        
        App.controller.diary.Open();
        App.controller.cloister.OnClose += Close; // 通過Close觸發NextAction
    }
}
