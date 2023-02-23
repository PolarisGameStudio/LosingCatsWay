using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class OutSideSensor : MvcBehaviour
{
    public GameObject effect;
    
    public void OpenUnlockGrid()
    {
        if (SceneManager.GetActiveScene().name.Equals("FriendRoom"))
            return;
        
        App.system.unlockGrid.Active();
    }
}
