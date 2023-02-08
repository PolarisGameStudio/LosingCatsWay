using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_MonthSign : ModelBehavior
{
    private List<int> signIndexs;
    private int month;
    private DateTime lastMonthSignDate;
    private List<Reward> monthRewards;
    private int todayIndex = -1;
    private bool isCanResign;
    
    public List<int> SignIndexs
    {
        get => signIndexs;
        set
        {
            signIndexs = value;
            OnSignIndexsChange(value);
        }
    }

    public int Month
    {
        get => month;
        set
        {
            month = value;
            OnMonthChange(value);
        }
    }

    public DateTime LastMonthSignDate
    {
        get => lastMonthSignDate;
        set => lastMonthSignDate = value;
    }

    public List<Reward> MonthRewards
    {
        get => monthRewards;
        set
        {
            monthRewards = value;
            OnMonthRewardsChange?.Invoke(value);
        }
    }

    public int TodayIndex
    {
        get => todayIndex;
        set
        {
            todayIndex = value;
            OnTodayIndexChange?.Invoke(value);
        }
    }

    public bool IsCanResign
    {
        get => isCanResign;
        set
        {
            isCanResign = value;
            OnIsCanResignChange?.Invoke(value);
        }
    }

    public ValueChange OnSignIndexsChange;
    public ValueChange OnMonthChange;
    public ValueChange OnMonthRewardsChange;
    public ValueChange OnTodayIndexChange;
    public ValueChange OnIsCanResignChange;
}
