using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MEQ0002_6", menuName = "Factory/Quests/MEQ/Create MEQ0002_6")]
public class MEQ0002_6 : Quest
{
    public override void Init()
    {
        base.Init();
        App.controller.dailyQuest.OnGetReward += Bind;
        App.controller.dailyQuest.OnGetAdsReward += Bind;
        App.controller.dailyQuest.OnGetAllAdsReward += OnGetAllAdsReward;
    }

    public void Bind()
    {
        Progress ++;

        if (Progress >= TargetCount)
        {
            App.controller.dailyQuest.OnGetReward -= Bind;
            App.controller.dailyQuest.OnGetAdsReward -= Bind;
            App.controller.dailyQuest.OnGetAllAdsReward -= OnGetAllAdsReward;
        }
    }

    private void OnGetAllAdsReward(object value)
    {
        int count = (int)value;
        Progress = Mathf.Clamp(Progress + count, 0, TargetCount);
        if (Progress >= TargetCount)
        {
            App.controller.dailyQuest.OnGetReward -= Bind;
            App.controller.dailyQuest.OnGetAdsReward -= Bind;
            App.controller.dailyQuest.OnGetAllAdsReward -= OnGetAllAdsReward;
        }
    }
}
