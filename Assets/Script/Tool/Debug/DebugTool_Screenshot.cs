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
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/";
        string fileName = DateTimeExtension.CurrentDateTime().ToString("dd-MM-yy-hh-mm-ss") + ".png";
        ScreenCapture.CaptureScreenshot(path + fileName);
        print($"Screenshot saved: {path + fileName}");
    }
}
