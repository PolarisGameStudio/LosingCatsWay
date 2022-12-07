using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TimeExtension
{
    //TODO �O������A
    public static void SetQuitTime() //����PlayerPrefs�s�ɶ� //����A���ݨD��s�k�B�ɶ������
    {
        string quitTime = DateTime.Now.ToString();
        PlayerPrefs.SetString("QuitTime", quitTime);
    }

    /// <summary>
    /// How many seconds passed after play again?
    /// </summary>
    /// <returns></returns>
    public static int GetPassedSeconds() //�g�L�X��
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
    public static int GetPassedDays() //�g�L�X��
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
