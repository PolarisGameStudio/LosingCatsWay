using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActor_RenamePlayer : TutorialActor
{
    private int siblingIndex;
    
    public override void Enter()
    {
        base.Enter();

        siblingIndex = App.system.playerRename.transform.GetSiblingIndex();
        App.system.playerRename.transform.SetAsLastSibling();
        
        App.system.playerRename.Open(true, false);
        App.system.playerRename.OnRenameComplete += Exit;
    }

    public override void Exit()
    {
        App.system.playerRename.OnRenameComplete -= Exit;
        App.system.playerRename.transform.SetSiblingIndex(siblingIndex);
        base.Exit();
    }
}
