using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class CatExtension
{
    #region MVC

    private static MyApplication app;

    static MyApplication App
    {
        get
        {
            if (app == null)
            {
                app = GameObject.FindObjectOfType<MyApplication>();
            }

            return app;
        }
    }
    
    #endregion

    #region Get

    public static int GetCatRealValue(float value)
    {
        int type = (int)(value / 20);

        if (value >= 100)
            type = 4;

        switch (type)
        {
            case 0:
                return -2;
            case 1:
                return -1;
            case 2:
                return 0;
            case 3:
                return 0;
            case 4:
                return 1;
        }

        return 0;
    }

    public static float GetCatRealValue(int ageLevel, float value)
    {
        if (ageLevel == 0 || ageLevel == 1)
        {
            if (value >= 90)
                return 0.2f;
            if (value >= 70)
                return 0.1f;
            if (value >= 50)
                return 0f;
            if (value >= 25)
                return -0.05f;

            return -0.06f;
        }
        
        if (value >= 90)
            return 0.2f;
        if (value >= 70)
            return 0.1f;
        if (value >= 50)
            return 0f;
        if (value >= 25)
            return 0.1f;

        return 0.15f;
    }

    /// 0: Happy
    /// 1: Normal
    /// 2: Awful
    /// 3: Sick
    public static int GetCatMood(CloudCatData cloudCatData)
    {
        if (!string.IsNullOrEmpty(cloudCatData.CatHealthData.SickId))
            return 3;
        
        float satiety = cloudCatData.CatSurviveData.Satiety;
        float favourability = cloudCatData.CatSurviveData.Favourbility;
        float moisture = cloudCatData.CatSurviveData.Moisture;

        float[] status = { satiety, favourability, moisture };
        float balance = status.Min();

        if (balance > 60) return 0;
        if (balance > 40) return 1;

        return 2;
    }

    /// 吃個性等級
    public static int CatEatLevel(CloudCatData cloudCatData)
    {
        for (int i = 0; i < cloudCatData.CatData.PersonalityTypes.Count; i++)
        {
            if (cloudCatData.CatData.PersonalityTypes[i] != 0) continue;
            return cloudCatData.CatData.PersonalityLevels[i];
        }

        return -1;
    }

    /// 玩個性等級
    public static int CatFunLevel(CloudCatData cloudCatData)
    {
        for (int i = 0; i < cloudCatData.CatData.PersonalityTypes.Count; i++)
        {
            if (cloudCatData.CatData.PersonalityTypes[i] != 3) continue;
            return cloudCatData.CatData.PersonalityLevels[i];
        }

        return -1;
    }

    // 0幼年 1成年 2老年
    public static int GetCatAgeLevel(int surviveDays)
    {
        if (MathfExtension.IsBetweenEqual(surviveDays, 1, 3))
        {
            return 0;
        }

        if (MathfExtension.IsBetweenEqual(surviveDays, 4, 23))
        {
            return 1;
        }

        if (surviveDays >= 24)
        {
            return 2;
        }

        return 0;
    }

    public static float GetCatRealSize(float bodyScale)
    {
        if (bodyScale > 1)
        {
            bodyScale -= 1f;
            bodyScale /= (1.1f - 0.9f); //預設隨機值
            bodyScale *= 1.5f;
            float result = bodyScale + 45f;
            return result;
        }
        if (bodyScale < 1)
        {
            bodyScale = Mathf.Abs(bodyScale - 1f);
            bodyScale /= (1.1f - 0.9f); //預設隨機值
            bodyScale *= 1.5f;
            float result = 45f - bodyScale;
            return result;
        }

        return 45f;
    }

    public static int ConvertPersonality(int type, int level)
    {
        switch (type)
        {
            case 1:
                return 8 - level;
            case 2:
                return 12 - level;
            case 3:
                return 16 - level;
        }

        return 4 - level;
    }

    public static bool IsPedigreeCat(string variety)
    {
        bool result = Enum.IsDefined(typeof(PurebredCatType), variety);
        return result;
    }

    #endregion
}