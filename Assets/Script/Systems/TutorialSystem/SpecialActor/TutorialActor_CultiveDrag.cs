using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TutorialActor_CultiveDrag : TutorialActor
{
    [SerializeField] private int dragIndex;
    
    public override void Enter()
    {
        if (dragIndex == 0)
        {
            App.controller.cultive.OnFeedFood += Exit;
            App.controller.cultive.SelectType(1);
        }
        if (dragIndex == 1)
        {
            App.controller.cultive.OnFeedWater += Exit;
            App.controller.cultive.SelectType(1);
        }
        if (dragIndex == 2)
        {
            App.controller.cultive.OnChangeLitter += Exit;
            App.controller.cultive.SelectType(3);
        }
        
        base.Enter();
    }

    public override void Exit()
    {
        if (dragIndex == 0)
            App.controller.cultive.OnFeedFood -= Exit;
        if (dragIndex == 1)
            App.controller.cultive.OnFeedWater -= Exit;
        if (dragIndex == 2)
            App.controller.cultive.OnChangeLitter -= Exit;
        
        base.Exit();
    }
}
