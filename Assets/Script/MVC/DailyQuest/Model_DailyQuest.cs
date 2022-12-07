using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_DailyQuest : ModelBehavior
{
    List<Quest> quests = new List<Quest>();
    private Quest totalQuest;
    private int getRewardIndex;
    private int rewardExp;
    
    public List<Quest> Quests
    {
        get => quests;
        set
        {
            quests = value;
            OnQuestsChange?.Invoke(value);
        }
    }
    
    public Quest TotalQuest
    {
        get => totalQuest;
        set
        {
            totalQuest = value;
            OnTotalQuestsChange?.Invoke(value);
        }
    }

    public int GetRewardIndex
    {
        get => getRewardIndex;
        set => getRewardIndex = value;
    }

    public int RewardExp
    {
        get => rewardExp;
        set => rewardExp = value;
    }

    public ValueChange OnQuestsChange;
    public ValueChange OnTotalQuestsChange;
}
