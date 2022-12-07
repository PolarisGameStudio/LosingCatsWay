using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TestMono : SerializedMonoBehaviour
{
    public Dictionary<string, int> testDictionary = new Dictionary<string, int>();

    private void Start()
    {
        testDictionary.Add("test", 1);

        print(testDictionary.Count);
        print(testDictionary.First());
        print(testDictionary.Last());
    }
}
