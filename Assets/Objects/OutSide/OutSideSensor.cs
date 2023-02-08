using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class OutSideSensor : MvcBehaviour
{
    public bool isFriendMode;

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        isFriendMode = sceneName == "FriendRoom";
    }

    public void OpenUnlockGrid()
    {
        if (isFriendMode)
            return;
        
        App.system.unlockGrid.Active();
    }
}
