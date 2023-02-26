using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class View_Events : ViewBehaviour
{
    [Title("Left")] [SerializeField] private GameObject[] sectionMasks;

    [SerializeField] private Animator[] sectionDotAnimatorss;
    public GameObject[] sectionDots;

    [Title("Right")] [SerializeField] private GameObject[] eventObjects;

    public override void Open()
    {
        base.Open();
        PlayRedActivate();
    }

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
        
        for (int i = 0; i < eventObjects.Length; i++)
        {
            if (i == index)
                eventObjects[i].SetActive(true);
            else
                eventObjects[i].SetActive(false);
        }
    }

    public void PlayRedActivate()
    {
        for (int i = 0; i < sectionDots.Length; i++)
        {
            var tmp = sectionDots[i];
            if (!tmp.activeSelf)
                continue;

            var animator = sectionDotAnimatorss[i];
            DOVirtual.DelayedCall(0.15f, () => animator.Play("Play"));
        }
    }
}