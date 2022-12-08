using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyVibrateTool : MonoBehaviour
{
    public VibrateType VibrateType;

    public void MyVibrate()
    {
        VibrateExtension.Vibrate(VibrateType);
    }
}
