using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MvcBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
