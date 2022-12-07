using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Random = UnityEngine.Random;
using Firebase.Firestore;

public class SickFactory : SerializedMonoBehaviour
{
    [Title("Kitty")]
    [SerializeField] private string[] KittySicks;
    
    [Title("Adult")]
    [SerializeField] private string[] WaterSicks;
    [SerializeField] private string[] FunSicks;
    [SerializeField] private string[] FoodSicks;
    
    [Title("Old")]
    [SerializeField] private string[] OldWaterSicks;
    [SerializeField] private string[] OldFunSicks;
    [SerializeField] private string[] OldFoodSicks;
    [SerializeField] private string[] OldBoySick;

    [Title("SickLevel")] [InfoBox("0:Light 1:Medium 2:Heavy")]
    [SerializeField] private Dictionary<string, int> SickLevels = new Dictionary<string, int>();
    [Title("MetCount")] [InfoBox("-1:Worm or MustDead")]
    [SerializeField] private Dictionary<string, int> SickMetCounts = new Dictionary<string, int>();

    public string GetSick(CloudCatData cloudCatData)
    {
        float realMoisture = cloudCatData.CatSurviveData.RealMoisture;
        float realFun = cloudCatData.CatSurviveData.RealFavourbility;
        int ageLevel = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays);
        bool isBoy = (cloudCatData.CatData.Sex == 0);

        //Kitty
        if (ageLevel == 0)
            return KittySicks.GetRandom();
        
        //Adult
        if (ageLevel == 1)
        {
            if (realMoisture <= 0)
                return WaterSicks.GetRandom();

            if (realFun <= 0)
                return FunSicks.GetRandom();

            return FoodSicks.GetRandom();
        }

        //Old
        if (isBoy && Random.value < 0.2f)
            return OldBoySick.GetRandom();

        if (realMoisture <= 0)
            return OldWaterSicks.GetRandom();

        if (realFun <= 0)
            return OldFunSicks.GetRandom();

        return OldFoodSicks.GetRandom();
    }

    public int GetSickLevel(string id)
    {
        return SickLevels[id];
    }

    public int GetMetCount(string id)
    {
        return SickMetCounts[id];
    }
}
