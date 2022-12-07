using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_MonthSign : ModelBehavior
{
    private List<int> signIndexs = new List<int>();
    private int month;
    private DateTime lastMonthSignDate;
    private MonthSignRewardData selectedMonthSignRewardData;
    private int resignCount;

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
        set
        {
            lastMonthSignDate = value;
        }
    }

    public MonthSignRewardData SelectedMonthSignRewardData
    {
        get => selectedMonthSignRewardData;
        set
        {
            selectedMonthSignRewardData = value;
            OnSelectedMonthSignRewardDataChange(value);
        }
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

    public ValueChange OnSignIndexsChange;
    public ValueChange OnMonthChange;
    public ValueChange OnSelectedMonthSignRewardDataChange;
    public ValueChange OnResignCountChange;
}
