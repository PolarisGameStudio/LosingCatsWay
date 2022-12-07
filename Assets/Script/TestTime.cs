using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTime : MonoBehaviour
{
    float preTime = 0;

    private void Start()
    {
        preTime = Time.realtimeSinceStartup;
        print($"{nameof(preTime)}:{preTime:00}");
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            float nowTime = Time.realtimeSinceStartup;
            print($"{nameof(nowTime)}:{nowTime:00}");
            float total = nowTime - preTime;
            print($"{nameof(total)}:{total:00}");
        }
        else
        {
            preTime = Time.realtimeSinceStartup;
            print($"{nameof(preTime)}:{preTime:00}");
        }
    }
}
