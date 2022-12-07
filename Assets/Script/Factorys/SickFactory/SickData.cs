using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SickData", menuName = "Factory/Create SickData")]
public class SickData : SerializedScriptableObject
{
    public string id;
    public bool isDead;
    [HideIf("isDead")] public int[] sickLevels;

    public int GetSickLevel()
    {
        return (isDead) ? -1 : sickLevels[Random.Range(0, sickLevels.Length)];
    }
}
