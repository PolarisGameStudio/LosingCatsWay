using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Just store all the quest.
/// </summary>
public class QuestFactory : SerializedMonoBehaviour
{
    #region MVC
    
    private MyApplication app;

    protected MyApplication App
    {
        get
        {
            if (app == null)
            {
                app = FindObjectOfType<MyApplication>();
            }

            return app;
        }
    }

    #endregion

    public Dictionary<string, Quest> DiamondDailyQuests = new Dictionary<string, Quest>();
    public Dictionary<string, Quest> CoinDailyQuests = new Dictionary<string, Quest>();
    public Dictionary<string, Quest> AchieveQuests = new Dictionary<string, Quest>();

    public Quest TotalDailyQuest;

    public List<Quest> GetDiamondDailyQuests()
    {
        List<Quest> result = new List<Quest>();

        for (int i = 0; i < DiamondDailyQuests.Count; i++)
        {
            Quest q = DiamondDailyQuests.ElementAt(i).Value;
            result.Add(q);
        }

        return result;
    }
    
    public List<Quest> GetCoinDailyQuests()
    {
        List<Quest> result = new List<Quest>();

        for (int i = 0; i < CoinDailyQuests.Count; i++)
        {
            Quest q = CoinDailyQuests.ElementAt(i).Value;
            result.Add(q);
        }

        return result;
    }

    public Quest GetQuestById(string id)
    {
        if (CoinDailyQuests.ContainsKey(id))
            return CoinDailyQuests[id];

        if (DiamondDailyQuests.ContainsKey(id))
            return DiamondDailyQuests[id];

        if (AchieveQuests.ContainsKey(id))
            return AchieveQuests[id];

        return TotalDailyQuest;
    }

    public Quest GetTotalDailyQuest()
    {
        return TotalDailyQuest;//TODO 沒有完成條件
    }
}