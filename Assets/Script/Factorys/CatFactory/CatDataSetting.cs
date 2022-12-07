using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CatDataSetting",menuName = "Factory/Create CatDataSetting")]
public class CatDataSetting : SerializedScriptableObject
{
    public float basicMoisture;
    public float basicSatiety;
    public float basicFavorability;
    [Space(10)]

    [Title("Sick percent")]
    public Dictionary<int, float> sickPercentBySurviveDay_Natural = new Dictionary<int, float>();

    public float SatietyByLevel(int level)
    {
        float result = basicSatiety;

        switch (level)
        {
            case 0:
                return result *= 1.2f;

            case 1:
                return result *= 1.1f;

            case 2:
                return result *= 0.9f;

            case 3:
                return result *= 0.8f;

            default:
                return result *= 1f;
        }
    }

    public float FunByLevel(int level)
    {
        float result = basicFavorability;

        switch (level)
        {
            case 0:
                return result *= 1.2f;

            case 1:
                return result *= 1.1f;

            case 2:
                return result *= 0.9f;

            case 3:
                return result *= 0.8f;

            default:
                return result *= 1f;
        }
    }
}
