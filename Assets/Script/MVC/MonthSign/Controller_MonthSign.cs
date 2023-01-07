using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

public class Controller_MonthSign : ControllerBehavior
{
    public Transform FrontTransform; // 圖層解決

    public async void Init()
    {
        int day = App.system.myTime.MyTimeNow.Day;
        if (App.model.monthSign.SignIndexs[day - 1] == 0)
            App.system.openFlow.AddAction(Open);
        
        App.model.monthSign.MonthRewards = await LoadMonthRewardData(App.system.myTime.MyTimeNow.Month);
    }

    public void Open()
    {
        App.view.monthSign.Open();
        CheckCalender();
    }

    public void Close()
    {
        App.view.monthSign.Close();
        App.system.cloudSave.UpdateCloudSignData();
        App.system.openFlow.NextAction();
    }

    /// 檢查並創建calender
    private void CheckCalender()
    {
        var today = App.system.myTime.MyTimeNow;
        var last = App.model.monthSign.LastMonthSignDate;

        //Over a year or month
        if (today.Year != last.Year || today.Month != last.Month)
            CreateCalander(today.Year, today.Month);
        //Same year and month
        else
        {
            List<int> signs = App.model.monthSign.SignIndexs;
            App.model.monthSign.SignIndexs = signs;
            App.model.monthSign.Month = App.system.myTime.MyTimeNow.Month;
        }
    }

    /// 按鈕簽到
    public void Sign()
    {
        int day = App.system.myTime.MyTimeNow.Day;
        var signIndexs = App.model.monthSign.SignIndexs;

        ReceiveReward(day);

        signIndexs[day - 1] = 1;
        App.model.monthSign.SignIndexs = signIndexs;
        App.model.monthSign.LastMonthSignDate = App.system.myTime.MyTimeNow;
    }

    /// 按鈕補簽
    public void Resign()
    {
        if (App.model.monthSign.ResignCount <= 0) return;

        //TODO 廣告

        var signs = App.model.monthSign.SignIndexs;
        for (int i = 0; i < signs.Count; i++)
        {
            if (i == App.system.myTime.MyTimeNow.Day - 1) break; //不超過今天
            if (signs[i] == 1) continue; //簽到了

            ReceiveReward(i + 1);
            signs[i] = 1;
            App.model.monthSign.ResignCount--;
            break;
        }

        App.model.monthSign.SignIndexs = signs;
    }

    private void CreateCalander(int year, int month)
    {
        int days = DateTime.DaysInMonth(year, month);
        List<int> signs = new List<int>();

        for (int i = 0; i < days; i++)
            signs.Add(0);

        App.model.monthSign.SignIndexs = signs;
        App.model.monthSign.Month = month;
        App.model.monthSign.ResignCount = 7; //重設補簽次數
    }

    private void ReceiveReward(int day)
    {
        var rewards = new List<Reward>();
        var reward = App.model.monthSign.MonthRewards[day - 1];
        rewards.Add(reward);
        App.system.reward.Open(rewards.ToArray());
    }

    private async Task<List<Reward>> LoadMonthRewardData(int month)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        var colRef = db.Collection("MonthRewards");
        var docRef = await colRef.Document(month.ToString()).GetSnapshotAsync();
        var data = docRef.ConvertTo<MonthRewardData>();

        List<Reward> tmp = new List<Reward>();
        var rewards = data.Rewards;
        for (int i = 0; i < rewards.Count; i++)
        {
            Reward reward = new Reward();
            reward.item = App.factory.itemFactory.GetItem(rewards[i].Id);
            reward.count = rewards[i].Count;
            tmp.Add(reward);
        }

        return tmp;
    }
}

[FirestoreData]
public class MonthRewardData
{
    [FirestoreProperty] public List<MonthReward> Rewards { get; set; }
}

[FirestoreData]
public class MonthReward
{
    [FirestoreProperty] public string Id { get; set; }
    [FirestoreProperty] public int Count { get; set; }
}