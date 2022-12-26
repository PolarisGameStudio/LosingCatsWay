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
        ScreenCapture.CaptureScreenshot(DateTimeExtension.CurrentDateTime().ToString("dd-MM-yy-hh-mm-ss") + ".png");
    }
}
