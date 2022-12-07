using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "ConfirmData", menuName = "Factory/Create ConfirmData")]
public class ConfirmData : SerializedScriptableObject
{
    public Dictionary<string, string> NormalContents;
    public Dictionary<string, string> AddEndWordContents;
    
    public Dictionary<string, string> NormalTitles;
    public Dictionary<string, string> AddEndWordTitles;
}