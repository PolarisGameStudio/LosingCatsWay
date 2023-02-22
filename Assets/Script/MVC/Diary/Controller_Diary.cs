using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Controller_Diary : ControllerBehavior
{
    [SerializeField] private GameObject leftButtonObject;
    [SerializeField] private GameObject rightButtonObject;
    
    public void Open()
    {
        App.view.diary.Open();
        App.model.diary.MemoryCount = GetCatMemoryValue();
        App.model.diary.MemoryScore = GetMemoryScoreValue();
        ToPage(-1);
    }

    public void Close()
    {
        App.view.diary.Close();
    }

    public void CloseByOpenCloister()
    {
        Close();
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

    public void GetCatMemory()
    {
        var data = App.model.diary.LosingCatData;
        if (data.IsGetMemory)
            return;

        Item memoryItem = App.factory.itemFactory.GetItem("CatMemory");
        int memoryCount = App.model.diary.MemoryCount;
        
        Reward reward = new Reward { count = memoryCount, item = memoryItem };
        List<Reward> rewards = new List<Reward> { reward };
        App.system.reward.Open(rewards.ToArray());
        
        data.IsGetMemory = true;
        App.model.diary.LosingCatData = data;
        App.system.cloudSave.SaveLosingCatData(data);
    }

    private int GetCatMemoryValue()
    {
        int result;
        var data = App.model.diary.LosingCatData;

        int max = data.CatData.SurviveDays * 2;
        max = max == 0 ? 1 : max;
        
        List<int> scores = new List<int>
        {
            data.CatDiaryData.DiarySatietyScore,
            data.CatDiaryData.DiaryLitterScore,
            data.CatDiaryData.DiaryMoistureScore,
            data.CatDiaryData.DiaryFavourbilityScore
        };
        int min = scores.Min();
        int range = min / max;

        if (range < 25)
            result = 1;
        else if (range < 50)
            result = 3;
        else if (range < 75)
            result = 8;
        else if (range < 100)
            result = 12;
        else
            result = 15;

        return result;
    }

    private int GetMemoryScoreValue()
    {
        int result;
        var data = App.model.diary.LosingCatData;

        int max = data.CatData.SurviveDays * 2;
        max = max == 0 ? 1 : max;
        
        List<int> scores = new List<int>
        {
            data.CatDiaryData.DiarySatietyScore,
            data.CatDiaryData.DiaryLitterScore,
            data.CatDiaryData.DiaryMoistureScore,
            data.CatDiaryData.DiaryFavourbilityScore
        };
        int min = scores.Min();
        int range = min / max;

        if (range < 25)
            result = 20;
        else if (range < 50)
            result = 40;
        else if (range < 75)
            result = 60;
        else if (range < 100)
            result = 80;
        else
            result = 100;

        return result;
    }
}
