using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using UnityEngine;

public class DebugTool_MonthRewards : MvcBehaviour
{
    public int month;
    public List<Reward> rewards;

    [Button]
    private void UploadReward()
    {
        MonthRewardData tmp = new MonthRewardData();
        tmp.Rewards = new List<MonthReward>();

        for (int i = 0; i < rewards.Count; i++)
        {
            MonthReward reward = new MonthReward();
            reward.Id = rewards[i].item.id;
            reward.Count = rewards[i].count;
            tmp.Rewards.Add(reward);
        }
        
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        var colRef = db.Collection("MonthRewards");
        colRef.Document(month.ToString()).SetAsync(tmp);
    }

    [Button]
    private async void DownloadReward()
    {
        rewards = new List<Reward>();
        
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        var colRef = db.Collection("MonthRewards");
        var result = await colRef.Document(month.ToString()).GetSnapshotAsync();
        var data = result.ConvertTo<MonthRewardData>();

        for (int i = 0; i < data.Rewards.Count; i++)
        {
            Reward reward = new Reward();
            reward.item = App.factory.itemFactory.GetItem(data.Rewards[i].Id);
            reward.count = data.Rewards[i].Count;
            rewards.Add(reward);
        }
    }
}
