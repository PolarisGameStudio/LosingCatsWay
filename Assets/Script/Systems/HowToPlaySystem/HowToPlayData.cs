using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HowToPlay", menuName = "Data/Create HowToPlayData")]
public class HowToPlayData : SerializedScriptableObject
{
    public Dictionary<string, string> titleData = new Dictionary<string, string>();
    public Dictionary<string, string[]> descriptData = new Dictionary<string, string[]>();
    public Sprite[] sprites;
}
