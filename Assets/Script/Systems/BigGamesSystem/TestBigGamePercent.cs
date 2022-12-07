using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestBigGamePercent : MonoBehaviour
{
    [SerializeField] private List<string> names = new List<string>();
    [SerializeField] private List<int> counts = new List<int>();
    [SerializeField, Range(0, 1)] private float additionPercent;

    [Button]
    public void TestPercent()
    {
        Dictionary<int, float> dict = new Dictionary<int, float>();
        List<int> indexs = new List<int>();

        for (int i = 0; i < names.Count; i++)
        {
            float f = 1f / names.Count;

            if (counts[i] > 0)
            {
                int count = counts[i];
                f += (additionPercent * count);
            }

            dict.Add(i, f);
        }

        //dict.OrderByDescending(x => x.Value);

        for (int i = 0; i < dict.Count; i++)
        {
            indexs.Add(dict.ElementAt(i).Key);
        }

        indexs.Shuffle();

        for (int i = 0; i < indexs.Count; i++)
        {
            int index = indexs[i];
            float f = dict[index];

            if (i >= indexs.Count - 1)
            {
                print($"{index}:{names[index]}");
                return;
            }

            if (Random.value > f) continue;
            print($"{index}:{names[index]}");
            break;
        }
    }
}
