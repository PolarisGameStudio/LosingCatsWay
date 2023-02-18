using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MyAnimationTable
{
    public static string GetClickAnimationName(CloudCatData cloudCatData)
    {
        String result = "Rearing_Cat/";

        if (!String.IsNullOrEmpty(cloudCatData.CatHealthData.SickId))
            return result + "Rearing_Rub_Sit(Only_For_Sick)";

        string[] catAnimationNames = new[]
        {
            "Rearing_Licking_IDLE",
            "Rearing_LookFeet_IDLE",
            "Rearing_LookFeet_Sit",
            "Rearing_Look_IDLE",
            "Rearing_Look_Sit",
            "Rearing_Rub_IDLE",
            "Rearing_Rub_Sit",
            "Rearing_Smile_IDLE",
            "Rearing_Smile_Sit"
        };

        string[] kittyCatAnimationNames = new[]
        {
            "Rearing_Licking_IDLE",
            "Rearing_LookFeet_IDLE",
            "Rearing_Rub_Sit",
            "Rearing_Smile_Sit"
        };
        
        if (cloudCatData.CatData.SurviveDays > 3)
            return result + catAnimationNames[Random.Range(0, catAnimationNames.Length)];
        return result + kittyCatAnimationNames[Random.Range(0, kittyCatAnimationNames.Length)];
    }
}