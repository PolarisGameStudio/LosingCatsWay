using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_ReadPedia : ViewBehaviour
{
    public override void Init()
    {
        base.Init();
        App.model.pedia.OnChoosePediaIndexChange += OnChoosePediaIndexChange;
    }

    private void OnChoosePediaIndexChange(object value)
    {
        int index = (int)value;
        //
    }
}
