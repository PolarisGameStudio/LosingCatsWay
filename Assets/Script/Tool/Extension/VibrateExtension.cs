using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VibrateExtension
{
    public static void Vibrate(VibrateType vibrateType)
    {
        switch (vibrateType)
        {
            case VibrateType.Cancel:
                Vibration.Cancel();
                break;
            case VibrateType.Nope:
                Vibration.VibrateNope();
                break;
            case VibrateType.Pop:
                Vibration.VibratePop();
                break;
            case VibrateType.Peek:
                Vibration.VibratePeek();
                break;
        }
    }
}

public enum VibrateType
{
    Cancel,
    Nope,
    Pop,
    Peek,
}