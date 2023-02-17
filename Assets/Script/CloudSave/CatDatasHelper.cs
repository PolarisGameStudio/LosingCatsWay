using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatDatasHelper
{
    #region Set

    //

    #endregion

    #region Save

    public Dictionary<string, object> GetCloudCatUpdate(CloudCatData cloudCatData)
    {
        Dictionary<string, object> result = new Dictionary<string, object>
        {
            {"CatData", cloudCatData.CatData},
            {"CatDiaryData", cloudCatData.CatDiaryData},
            {"CatHealthData", cloudCatData.CatHealthData},
            {"CatSkinData", cloudCatData.CatSkinData},
            {"CatSurviveData", cloudCatData.CatSurviveData},
        };

        return result;
    }

    #endregion
}