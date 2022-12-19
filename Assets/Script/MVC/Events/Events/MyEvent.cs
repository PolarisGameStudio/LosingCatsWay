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
    public abstract void Open();
    public abstract void Init();

}
