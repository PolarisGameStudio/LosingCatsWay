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

    public float NaturalDeadPercent(int surviveDays)
    {
        if (surviveDays <= 27)
            return 0;
        if (surviveDays == 28)
            return 0.25f;
        if (surviveDays == 29)
            return 0.5f;
        if (surviveDays == 30)
            return 0.7f;
        if (surviveDays <= 39)
            return 0.9f;
        return 1;
    }
}
