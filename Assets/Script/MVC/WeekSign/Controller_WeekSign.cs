using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Controller_WeekSign : ControllerBehavior
{
    // [SerializeField] private Item_Reward[] weekRewards;
    //
    // public void Init()
    // {
    //     if ((int)(App.model.weekSign.LastWeekSignDate - DateTime.Now).TotalDays != 0)
    //     {
    //         SetEnableFlow(0);
    //     }
    //
    //     if (GetEnableFlow() == 0)
    //     {
    //         App.system.openFlow.AddAction(Open);
    //     }
    // }
    //
    // [Button]
    // public void Open()
    // {
    //     App.view.weekSign.Open();
    //
    //     //更新獎勵内容
    //     App.model.weekSign.WeekRewards = new List<Item_Reward>(weekRewards);
    //
    //     //7天領取進度
    //     if (App.model.weekSign.SignIndexs.Count <= 0) //沒記錄
    //     {
    //         List<int> result = new List<int>();
    //         for (int i = 0; i < 7; i++)
    //         {
    //             result.Add(0);
    //         }
    //
    //         App.model.weekSign.SignIndexs = result;
    //     }
    //     else
    //     {
    //         var indexs = App.model.weekSign.SignIndexs;
    //         App.model.weekSign.SignIndexs = indexs;
    //     }
    //
    //     //領取
    //     DOVirtual.DelayedCall(0.175f, () => GetReward(App.model.weekSign.LastSignIndex + 1));
    // }
    //
    // public void Close()
    // {
    //     App.view.weekSign.Close();
    //     App.system.openFlow.NextAction();
    // }
    //
    // private void GetReward(int index)
    // {
    //     var lastDate = App.model.weekSign.LastWeekSignDate;
    //     if (lastDate != null)
    //     {
    //         if (DateTime.Compare(lastDate, DateTime.Today) == 0) //今日已領
    //             return;
    //     }
    //
    //     int lastIndex = App.model.weekSign.LastSignIndex;
    //     if (index == lastIndex) return; //今日已領
    //     if (index > lastIndex + 1) return; //不能領後面的獎勵
    //
    //     var indexs = App.model.weekSign.SignIndexs;
    //
    //     ReceiveReward(index);
    //
    //     indexs[index] = 1;
    //     App.model.weekSign.SignIndexs = indexs;
    //     App.model.weekSign.LastSignIndex = index;
    //     App.model.weekSign.LastWeekSignDate = DateTime.Today;
    //
    //     SetEnableFlow(1);
    // }
    //
    // private void ReceiveReward(int index)
    // {
    //     var reward = weekRewards[index];
    //
    //     if (reward.itemType == ItemType.Coin) //貓掌幣加
    //     {
    //         App.system.player.Coin += reward.ReceiveCount;
    //         return;
    //     }
    //
    //     if (reward.itemType == ItemType.Diamond) //鑽石加
    //     {
    //         App.system.player.Diamond += reward.ReceiveCount;
    //         return;
    //     }
    //
    //     if (reward.itemType == ItemType.Room) //房間加
    //     {
    //         App.system.inventory.RoomData[reward.id] += reward.ReceiveCount;
    //         return;
    //     }
    //
    //     //這邊是可以直接加Count的東西
    //     reward.Count += reward.ReceiveCount;
    // }
    //
    // #region Prefs
    //
    // /// <summary>
    // /// 設定下次是否自動加入流程的索引
    // /// 0:加入
    // /// 1:不加入
    // /// </summary>
    // /// <param name="value"></param>
    // private void SetEnableFlow(int value)
    // {
    //     PlayerPrefs.SetInt("WeekEnableFlow", value);
    // }
    //
    // /// <summary>
    // /// 取得是否自動加入流程的索引
    // /// 0:加入
    // /// 1:不加入
    // /// </summary>
    // /// <returns></returns>
    // private int GetEnableFlow()
    // {
    //     return PlayerPrefs.GetInt("WeekEnableFlow", 0);
    // }
    //
    // #endregion
}
