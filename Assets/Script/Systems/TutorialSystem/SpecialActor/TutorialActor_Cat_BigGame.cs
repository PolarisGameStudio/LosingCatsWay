using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActor_Cat_BigGame : TutorialActor
{
    public override void Exit()
    {
        var cat = App.system.cat.GetCats()[0];
        cat.StartBigGame();
        App.controller.followCat.CloseByOpenLobby();
        base.Exit();
    }
}
