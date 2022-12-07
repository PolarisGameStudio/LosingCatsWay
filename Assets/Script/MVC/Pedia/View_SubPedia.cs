using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class View_SubPedia : ViewBehaviour
{
    [Title("UIView")] public View_ChoosePedia choosePedia;
    public View_ReadPedia readPedia;
    
    [Title("UI")]
    [SerializeField] private Card_PediaType[] cards;
    
    public override void Init()
    {
        base.Init();
        App.model.pedia.OnSelectedPediaTypeChange += OnSelectedPediaTypeChange;
    }

    private void OnSelectedPediaTypeChange(object value)
    {
        int index = (int)value;

        if (index < 0)
            return;
        
        for (int i = 0; i < cards.Length; i++)
        {
            if (i == index)
                cards[i].SetSelect(true);
            else
                cards[i].SetSelect(false);
        }
    }
}
