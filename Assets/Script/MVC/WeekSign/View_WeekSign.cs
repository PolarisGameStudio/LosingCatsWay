using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_WeekSign : ViewBehaviour
{
    // [SerializeField] private Card_WeekSign[] cards;
    //
    // public override void Init()
    // {
    //     base.Init();
    //     App.model.weekSign.OnGetRewardIndexsChange += OnGetRewardIndexsChange;
    //     App.model.weekSign.OnWeekRewardChange += OnWeekRewardChange;
    // }
    //
    // private void OnGetRewardIndexsChange(object value)
    // {
    //     List<int> indexs = (List<int>)value;
    //
    //     for (int i = 0; i < indexs.Count; i++)
    //     {
    //         if (indexs[i] == 1)
    //         {
    //             cards[i].SetIsGet();
    //             continue;
    //         }
    //     }
    // }
    //
    // private void OnWeekRewardChange(object value)
    // {
    //     List<Item_Reward> rewards = (List<Item_Reward>)value;
    //
    //     for (int i = 0; i < rewards.Count; i++)
    //     {
    //         cards[i].SetData(rewards[i]);
    //     }
    // }
}
