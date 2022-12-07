using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 所有得到的Quest加進來
/// </summary>
public class QuestSystem : SerializedMonoBehaviour
{
    #region MVC

    protected MyApplication App
    {
        get => FindObjectOfType<MyApplication>();
    }

    #endregion

    public Dictionary<string, int> QuestProgressData;
    public Dictionary<string, bool> QuestIsReceivedData;

    public void Init()
    {
        App.system.myTime.OnFirstLogin += ResetQuests;
    }

    private void ResetQuests()
    {
        var tmp = QuestProgressData.ToList();

        for (int i = 0; i < tmp.Count; i++)
        {
            var key = tmp[i].Key;
            
            QuestProgressData[key] = 0;
            QuestIsReceivedData[key] = false;

        }
    }
}