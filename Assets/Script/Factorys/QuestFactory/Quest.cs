using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Factory/Quests/Create Quest")]
public class Quest : SerializedScriptableObject
{
    #region MVC

    protected MyApplication App
    {
        get => FindObjectOfType<MyApplication>();
    }

    #endregion

    public string id;
    public Sprite icon;
    [Space(10)]

    public int[] targetCountBasedOnLevel;
    public List<Reward[]> rewardsBasedOnLevel = new List<Reward[]>();
    public int exp;

    public string Content
    {
        get
        {
            return App.factory.stringFactory.GetQuestContent(id);
        }
    }

    public virtual int Progress
    {
        get
        {
            return App.system.quest.QuestProgressData[id];
        }
        set
        {
            App.system.quest.QuestProgressData[id] = value;
        }
    }
    
    public bool IsReceived
    {
        get
        {
            return App.system.quest.QuestIsReceivedData[id];
        }
        set
        {
            App.system.quest.QuestIsReceivedData[id] = value;
        }
    }

    //已達標
    public bool IsReach
    {
        get
        {
            return Progress >= TargetCount;
        }
    }

    public virtual int TargetCount
    {
        get
        {
            return targetCountBasedOnLevel[0];
        }
    }

    public virtual Reward[] Rewards
    {
        get
        {
            return rewardsBasedOnLevel[0];
        }
    }
    
    public virtual void Init()
    {
        //
    }
    
    public virtual void CancelBind()
    {
        //
    }


    //TODO 做已領取或固定等級（進度）
}