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
    public Transform BackTransform;

    [Title("Resign")]
    [SerializeField] private MSR001 freeResign;
    [SerializeField] private MSR002 adsResign;
    
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
        CheckCanResign();
    }

    public void Close()
    {
        App.view.monthSign.Close();
        App.system.cloudSave.UpdateCloudSignData();
        
        if (!App.system.openFlow.isEnd)
            App.system.openFlow.NextAction();
        else
            App.controller.lobby.SetBuffer();
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

        App.model.monthSign.TodayIndex = today.Day - 1;
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
        int todayIndex = App.model.monthSign.TodayIndex;
        var signs = App.model.monthSign.SignIndexs;
        
        if (!freeResign.IsReach)
        {
            for (int i = 0; i < signs.Count; i++)
            {
                if (i == todayIndex)
                    break;
                if (signs[i] == 1)
                    continue;

                ReceiveReward(i + 1);
                signs[i] = 1;
                freeResign.Progress++;
                CheckCanResign();
                App.model.monthSign.SignIndexs = signs;
                break;
            }

            return;
        }

        if (!adsResign.IsReach)
        {
            for (int i = 0; i < signs.Count; i++)
            {
                if (i == todayIndex)
                    break;
                if (signs[i] == 1)
                    continue;

                ReceiveReward(i + 1);
                signs[i] = 1;
                adsResign.Progress++;
                CheckCanResign();
                App.model.monthSign.SignIndexs = signs;
                break;
            }
        }
    }

    private void CreateCalander(int year, int month)
    {
        int days = DateTime.DaysInMonth(year, month);
        List<int> signs = new List<int>();

        for (int i = 0; i < days; i++)
            signs.Add(0);

        App.model.monthSign.SignIndexs = signs;
        App.model.monthSign.Month = month;
        //重設補簽次數
        freeResign.Progress = 0;
        adsResign.Progress = 0;
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

    public void SortDateObjects()
    {
        App.view.monthSign.SortDateObjects();
    }

    private void CheckCanResign()
    {
        if (freeResign.IsReach && adsResign.IsReach)
        {
            App.model.monthSign.IsCanResign = false;
            return;
        }

        List<int> canResignDays = new List<int>();
        List<int> currentSigns = App.model.monthSign.SignIndexs;
        int todayIndex = App.model.monthSign.TodayIndex;

        for (int i = 0; i < todayIndex; i++)
        {
            if (currentSigns[i] == 1) // 簽了
                continue;
            canResignDays.Add(currentSigns[i]);
        }

        App.model.monthSign.IsCanResign = canResignDays.Count > 0;
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