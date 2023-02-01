using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DictSetValueHelper
{
    public void SetDict(Dictionary<string, int> from, Dictionary<string, int> to)
    {
        if (from == null)
            return;

        for (int i = 0; i < from.Count; i++)
        {
            string key = from.ElementAt(i).Key;
            if (to.ContainsKey(key))
                to[key] = from[key];
        }
    }

    public void SetDict(Dictionary<string, bool> from, Dictionary<string, bool> to)
    {
        if (from == null)
            return;

        for (int i = 0; i < from.Count; i++)
        {
            string key = from.ElementAt(i).Key;
            if (to.ContainsKey(key))
                to[key] = from[key];
        }
    }
}