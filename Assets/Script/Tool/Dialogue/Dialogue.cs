using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string country = "en";
    public string speakerName;
    [Multiline(5)] public List<string> content = new List<string>();
}
