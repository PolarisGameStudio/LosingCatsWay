using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class View_Events : ViewBehaviour
{
    [Title("Left")] [SerializeField] private GameObject[] sectionMasks;
    [SerializeField] private GameObject[] sectionDots;

    [Title("Right")] [SerializeField] private GameObject[] eventObjects;
    
    public override void Init()
    {
        base.Init();
        App.model.events.OnSelectIndexChange += OnSelectIndexChange;
    }

    private void OnSelectIndexChange(object value)
    {
        int index = (int)value;

        for (int i = 0; i < sectionMasks.Length; i++)
        {
            if (i == index)
                sectionMasks[i].SetActive(true);
            else
                sectionMasks[i].SetActive(false);
        }

        sectionDots[index].SetActive(false);

        for (int i = 0; i < eventObjects.Length; i++)
        {
            if (i == index)
                eventObjects[i].SetActive(true);
            else
                eventObjects[i].SetActive(false);
        }
    }
}
