using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TutorialActor_CultiveDrag : TutorialActor
{
    [SerializeField] private int dragIndex;
    [SerializeField] private Drop_Cultive dropCultive;
    
    public override void Enter()
    {
        if (dragIndex == 0)
        {
            dropCultive.canFeedFood = true;
            dropCultive.canFeedWater = false;
            dropCultive.canChangeLitter = false;
            
            App.controller.cultive.OnFeedFood += Exit;
            App.controller.cultive.SelectType(1);
        }
        if (dragIndex == 1)
        {
            dropCultive.canFeedFood = false;
            dropCultive.canFeedWater = true;
            dropCultive.canChangeLitter = false;

            App.controller.cultive.OnFeedWater += Exit;
            App.controller.cultive.SelectType(1);
        }
        if (dragIndex == 2)
        {
            dropCultive.canFeedFood = false;
            dropCultive.canFeedWater = false;
            dropCultive.canChangeLitter = true;

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
