using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_WeekSign : ModelBehavior
{
    // private List<int> signIndexs = new List<int>(); //0未領 1已領
    // private List<Item_Reward> weekRewards = new List<Item_Reward>();
    // private int lastSignIndex = -1; //最後領的Index
    // private DateTime lastWeekSignDate; //最後領的日期 避免一天重複領取
    //
    // public List<int> SignIndexs
    // {
    //     get => signIndexs;
    //     set
    //     {
    //         signIndexs = value;
    //         OnGetRewardIndexsChange(value);
    //     }
    // }
    //
    // public List<Item_Reward> WeekRewards
    // {
    //     get => weekRewards;
    //     set
    //     {
    //         weekRewards = value;
    //         OnWeekRewardChange(value);
    //     }
    // }
    //
    // /// <summary>
    // /// 最後領取的Index
    // /// </summary>
    // public int LastSignIndex
    // {
    //     get => lastSignIndex;
    //     set
    //     {
    //         lastSignIndex = value;
    //     }
    // }
    //
    // /// <summary>
    // /// 避免一天內重複領取的日期記錄，要領取時對比
    // /// </summary>
    // public DateTime LastWeekSignDate
    // {
    //     get => lastWeekSignDate;
    //     set
    //     {
    //         lastWeekSignDate = value;
    //     }
    // }
    //
    // public ValueChange OnGetRewardIndexsChange;
    // public ValueChange OnWeekRewardChange; //顯示七天的獎勵ui
}
