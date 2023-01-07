using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_MonthSign : ModelBehavior
{
    private List<int> signIndexs;
    private int month;
    private DateTime lastMonthSignDate;
    private int resignCount;
    private List<Reward> monthRewards;

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

    public int ResignCount
    {
        get => resignCount;
        set
        {
            resignCount = value;
            OnResignCountChange(value);
        }
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

    public ValueChange OnSignIndexsChange;
    public ValueChange OnMonthChange;
    public ValueChange OnResignCountChange;
    public ValueChange OnMonthRewardsChange;
}
