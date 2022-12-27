using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TutorialActor_CultiveDrag : TutorialActor
{
    [SerializeField] private int dragIndex;
    [SerializeField] private Drop_Cultive dropCat;
    [SerializeField] private Drop_Cultive dropLitter;
    
    public override void Enter()
    {
        if (dragIndex == 0)
        {
            dropCat.canFeedFood = true;
            dropCat.canFeedWater = false;
            dropLitter.canChangeLitter = false;
            
            App.controller.cultive.OnFeedFood += Exit;
            App.controller.cultive.SelectType(1);
        }
        if (dragIndex == 1)
        {
            dropCat.canFeedFood = false;
            dropCat.canFeedWater = true;
            dropLitter.canChangeLitter = false;

            App.controller.cultive.OnFeedWater += Exit;
            App.controller.cultive.SelectType(1);
        }
        if (dragIndex == 2)
        {
            dropCat.canFeedFood = false;
            dropCat.canFeedWater = false;
            dropLitter.canChangeLitter = true;

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
        
        dropCat.canFeedFood = true;
        dropCat.canFeedWater = true;
        dropLitter.canChangeLitter = true;
        
        base.Exit();
    }
}
