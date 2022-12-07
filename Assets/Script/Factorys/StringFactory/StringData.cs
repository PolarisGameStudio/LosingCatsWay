using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "StringData", menuName = "Factory/Create StringData")]
[Searchable]
public class StringData : SerializedScriptableObject
{
    public Dictionary<string, string> Contents = new Dictionary<string, string>();
}
