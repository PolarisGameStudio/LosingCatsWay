using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class MyEvent : SerializedMonoBehaviour
{
    private MyApplication app;

    protected MyApplication App
    {
        get 
        {
            if (app == null)
            {
                app = FindObjectOfType<MyApplication>();
            }

            return app; 
        }
    }
    
    public string id;
    public int endYear = 1997;
    public int endMonth = 1;
    public int endDay = 1;
    public abstract void Open();
    public abstract void Init();
    public abstract bool CheckRedPoint();
}
