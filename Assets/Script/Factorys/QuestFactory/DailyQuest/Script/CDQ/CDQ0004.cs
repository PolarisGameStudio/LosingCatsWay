using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CDQ0004", menuName = "Factory/Quests/CDQ/Create CDQ0004")]
public class CDQ0004 : DailyQuest
{
    public override void Init()
    {
        App.system.screenshot.OnScreenshotComplete += Bind;
    }

    private void Bind()
    {
        Progress++;

        if (Progress == TargetCount)
        {
            App.system.screenshot.OnScreenshotComplete -= Bind;
        }
    }
}