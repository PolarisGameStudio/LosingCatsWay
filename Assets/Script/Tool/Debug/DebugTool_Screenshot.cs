using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DebugTool_Screenshot : MonoBehaviour
{
    [Button]
    private void TakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot(DateTime.Now.ToLocalTime().ToLongDateString() + ".png");
    }
}
