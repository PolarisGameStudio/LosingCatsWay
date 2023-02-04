using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutSideSensor : MvcBehaviour
{
    public bool IsFriendMode;
    
    public void OpenUnlockGrid()
    {
        if (IsFriendMode)
            return;
        App.system.unlockGrid.Active();
    }
}
