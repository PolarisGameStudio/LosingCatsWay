using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public class Controller_MonthSign : ControllerBehavior
{
    [SerializeField] private MonthSignRewardData[] monthSignRewardDatas; //12個月的獎勵資料

    public Callback OnClose;

    public void Init()
    {
        //App.controller.events.OnClose += Open;
        
        if ((int)(App.model.monthSign.LastMonthSignDate - DateTime.Now).TotalDays != 0)
        {
            SetEnableFlow(0);
        }

        if(GetEnableFlow() == 0)
        {
            App.system.openFlow.AddAction(Open);
        }
    }

    [HorizontalGroup("Split", 0.5f)]
    [Button(30)]
    public void Open()
    {
        App.view.monthSign.Open();
        CheckCalender();
    }

    [HorizontalGroup("Split", 0.5f)]
    [Button(30)]
    public void Close()
    {
        App.view.monthSign.Close();

        //Remove callback
        //App.controller.events.OnClose -= Open;

        //OnClose?.Invoke();

        App.system.openFlow.NextAction();
    }

    /// <summary>
    /// 檢查並創建calender
    /// </summary>
    private void CheckCalender()
    {
        var today = DateTime.Now;
        var last = App.model.monthSign.LastMonthSignDate;

        //No last sign record
        if (last == null)
        {
            CreateCalander(today.Year, today.Month);
            return;
        }

        //Over a year or month
        if (today.Year != last.Year || today.Month != last.Month)
        {
            CreateCalander(today.Year, today.Month);
            return;
        }
        //Same year and month
        else
        {
            List<int> signs = App.model.monthSign.SignIndexs;
            App.model.monthSign.SignIndexs = signs;
            App.model.monthSign.Month = DateTime.Now.Month;
            App.model.monthSign.SelectedMonthSignRewardData = monthSignRewardDatas[DateTime.Now.Month - 1];
        }
    }

    /// <summary>
    /// 按鈕簽到
    /// </summary>
    public void Sign()
    {
        int day = DateTime.Now.Day;
        var signIndexs = App.model.monthSign.SignIndexs;

        ReceiveReward(DateTime.Now.Month, day);

        signIndexs[day - 1] = 1;
        App.model.monthSign.SignIndexs = signIndexs;
        App.model.monthSign.LastMonthSignDate = DateTime.Now;

        SetEnableFlow(1);
    }

    /// <summary>
    /// 按鈕補簽
    /// </summary>
    public void Resign()
    {
        if (App.model.monthSign.ResignCount <= 0) return;

        //TODO 廣告

        var signs = App.model.monthSign.SignIndexs;
        for (int i = 0; i < signs.Count; i++)
        {
            if (i == DateTime.Now.Day - 1) break; //不超過今天
            if (signs[i] == 1) continue; //簽到了

            ReceiveReward(DateTime.Now.Month, i + 1);
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
        {
            signs.Add(0);
        }

        App.model.monthSign.SignIndexs = signs;
        App.model.monthSign.Month = month;
        App.model.monthSign.SelectedMonthSignRewardData = monthSignRewardDatas[DateTime.Now.Month - 1];
        App.model.monthSign.ResignCount = 7; //重設補簽次數
    }

    private void ReceiveReward(int month, int day)
    {
        var data = monthSignRewardDatas[month - 1];
        var reward = data.GetReward(day);

        if (reward.item.itemType == ItemType.Coin) //貓掌幣加
        {
            App.system.player.Coin += reward.count;
            return;
        }

        if (reward.item.itemType == ItemType.Diamond) //鑽石加
        {
            App.system.player.Diamond += reward.count;
            return;
        }

        if (reward.item.itemType == ItemType.Room) //房間加
        {
            App.system.inventory.RoomData[reward.item.id] += reward.count;
            return;
        }

        //這邊是可以直接加Count的東西
        reward.item.Count += reward.count;
    }

    #region Prefs

    /// <summary>
    /// 設定下次是否自動加入流程的索引
    /// 0:加入
    /// 1:不加入
    /// </summary>
    /// <param name="value"></param>
    private void SetEnableFlow(int value)
    {
        PlayerPrefs.SetInt("MonthEnableFlow", value);
    }

    /// <summary>
    /// 取得是否自動加入流程的索引
    /// 0:加入
    /// 1:不加入
    /// </summary>
    /// <returns></returns>
    private int GetEnableFlow()
    {
        return PlayerPrefs.GetInt("MonthEnableFlow", 0);
    }

    #endregion
}