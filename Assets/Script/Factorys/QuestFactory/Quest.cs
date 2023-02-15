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

    public string id;
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
    
    public virtual bool IsReceived
    {
        get
        {
            int receivedStatus = App.system.quest.QuestReceivedStatusData[id];
            return receivedStatus > 0;
        }
        set
        {
            App.system.quest.QuestReceivedStatusData[id] = value ? 1 : 0;
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
        if (IsReach)
            return;
    }
    
    public virtual void CancelBind()
    {
        //
    }


    //TODO 做已領取或固定等級（進度）
}