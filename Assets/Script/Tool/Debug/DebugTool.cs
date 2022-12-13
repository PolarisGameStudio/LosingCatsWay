using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;
using I2.Loc;

public class DebugTool : MonoBehaviour
{
    DebugTool_Cat cat = new DebugTool_Cat();

    [Button]
    public void CreateCatAtShelter()
    {
        cat.CreateCat("Shelter");
    }

    [Button]
    public void CreateCatAtMap()
    {
        int index = Random.Range(0, 2);
        cat.CreateCat($"Location{index}");
    }

    [Button]
    public void Test()
    {
        print(LocalizationManager.CurrentLanguage);
        print(LocalizationManager.CurrentRegionCode);
        print(LocalizationManager.CurrentLanguageCode);
    }
}
