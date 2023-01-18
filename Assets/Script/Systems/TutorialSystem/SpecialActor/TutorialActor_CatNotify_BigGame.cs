using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TutorialActor_CatNotify_BigGame : TutorialActor_Unmask
{
    [Title("CatNotify_BigGame")]
    [SerializeField] private Card_CatNotify CardCatNotify;
    
    public override void Enter()
    {
        var cat = App.system.cat.GetCats()[0];
        cat.isPauseGame = true;
        cat.OpenBigGame();
        base.Enter();
    }

    public override void Exit()
    {
        CardCatNotify.Click();
        base.Exit();
    }
}
