using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutSideSensor : MvcBehaviour
{
    public void OpenUnlockGrid()
    {
        App.system.unlockGrid.Active();
    }
}
