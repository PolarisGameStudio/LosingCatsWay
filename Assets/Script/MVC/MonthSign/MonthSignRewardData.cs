using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonthRewards", menuName = "Data/Create MonthSignRewardsData")]
public class MonthSignRewardData : SerializedScriptableObject
{
    public Dictionary<int, Reward> signRewards = new Dictionary<int, Reward>();

    public Reward GetReward(int day)
    {
        return (signRewards.ContainsKey(day)) ? signRewards[day] : null;
    }
}
