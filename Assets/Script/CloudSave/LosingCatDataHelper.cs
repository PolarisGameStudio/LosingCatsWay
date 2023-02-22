using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

public class LosingCatDataHelper
{
    private MyApplication app;

    public LosingCatDataHelper(MyApplication application)
    {
        app = application;
    }
    
    public CloudLosingCatData CreateLosingCatData(CloudCatData cloudCatData)
    {
        CloudLosingCatData result = new CloudLosingCatData();

        result.CatData = cloudCatData.CatData;
        result.CatSkinData = cloudCatData.CatSkinData;
        result.CatDiaryData = cloudCatData.CatDiaryData;
        result.ExpiredTimestamp = Timestamp.FromDateTime(app.system.myTime.MyTimeNow.AddDays(7));
        result.LosingCatStatus = new List<string>();
        
        if (app.model.cloister.LosingCatDatas.Count <= 0) // 在CatSystem的PerDay之前Cloister.Init已經下載了以前的日記貓
            result.LosingCatStatus.Add("First");

        return result;
    }

    public Dictionary<string, object> GetUpdate(CloudLosingCatData cloudLosingCatData)
    {
        Dictionary<string, object> result = new Dictionary<string, object>
        {
            { "CatData", cloudLosingCatData.CatData },
            { "CatDiaryData", cloudLosingCatData.CatDiaryData },
            { "CatSkinData", cloudLosingCatData.CatSkinData },
            { "ExpiredTimestamp", cloudLosingCatData.ExpiredTimestamp },
            { "IsGetMemory", cloudLosingCatData.IsGetMemory },
            { "LosingCatStatus", cloudLosingCatData.LosingCatStatus },
        };

        return result;
    }
}
