using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_CatGuide : ControllerBehavior
{
    public Callback OnClose;
    
    public void Open()
    {
        App.view.catGuide.Open();
    }

    public void Close()
    {
        App.view.catGuide.Close();
        OnClose?.Invoke();
        OnClose = null;
    }

    public void OpenDailyQuest()
    {
        App.view.catGuide.Close();
        App.controller.dailyQuest.Open();
    }
}
