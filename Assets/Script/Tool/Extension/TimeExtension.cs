using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeExtension
{
    //TODO 記錄到伺服
    public static void SetQuitTime() //先用PlayerPrefs存時間 //之後再應需求改存法、時間的抓取
    {
        string quitTime = DateTime.Now.ToString();
        PlayerPrefs.SetString("QuitTime", quitTime);
    }

    /// <summary>
    /// How many seconds passed after play again?
    /// </summary>
    /// <returns></returns>
    public static int GetPassedSeconds() //經過幾秒
    {
        string quitTimeString = PlayerPrefs.GetString("QuitTime", "");

        if (!String.IsNullOrEmpty(quitTimeString))
        {
            DateTime quitTime = DateTime.Parse(quitTimeString);
            DateTime nowTime = DateTime.Now;

            PlayerPrefs.SetString("QuitTime", ""); //Clear Prefs

            TimeSpan timeSpan = nowTime - quitTime;
            return (int)timeSpan.TotalSeconds;
        }
        else
        {
            return 0;
        }
    }

    public static int GetPassedMinutes()
    {
        int seconds = GetPassedSeconds();
        return System.Convert.ToInt32(seconds / 60);
    }

    /// <summary>
    /// How many days passed after play again?
    /// </summary>
    /// <returns></returns>
    public static int GetPassedDays() //經過幾天
    {
        string quitTimeString = PlayerPrefs.GetString("QuitTime", "");

        if (!String.IsNullOrEmpty(quitTimeString))
        {
            DateTime quitTime = DateTime.Parse(quitTimeString);
            DateTime nowTime = DateTime.Now;

            PlayerPrefs.SetString("QuitTime", ""); //Clear Prefs

            TimeSpan timeSpan = nowTime - quitTime;
            return (int)timeSpan.TotalDays;
        }
        else
        {
            return 0;
        }
    }
}
