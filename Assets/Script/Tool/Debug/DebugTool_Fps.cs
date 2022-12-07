using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DebugTool_Fps : MonoBehaviour
{
    [Title("Editor")]
    [SerializeField] private int targetFps = 120;
    
    private void Awake()
    {
        Application.targetFrameRate = targetFps;
    }
}
