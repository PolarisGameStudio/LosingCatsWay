using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialActor_CatNotify_BigGame : TutorialActor_Unmask
{
    private Card_CatNotify card;
    
    public override void Enter()
    {
        var cat = App.system.cat.GetCats()[0];
        App.system.catNotify.Remove(cat);
        cat.isPauseGame = true;
        cat.OpenBigGame();
        card = App.system.catNotify.GetNotify(cat);
        targetRect = card.transform as RectTransform;
        base.Enter();
    }

    public override void Exit()
    {
        card.Click();
        base.Exit();
    }
}
