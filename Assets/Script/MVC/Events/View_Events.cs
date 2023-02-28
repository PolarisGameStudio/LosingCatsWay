using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class View_Events : ViewBehaviour
{
    [Title("Left")] [SerializeField] private GameObject[] sectionMasks;

    [SerializeField] private Animator[] sectionDotAnimatorss;
    public GameObject[] sectionDots;

    [Title("Right")] [SerializeField] private MyEvent[] eventObjects;
    public TextMeshProUGUI endTimeText;
    public GameObject endTimeObject;
    public GameObject noEndTimeObject;

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

        MyEvent tmp = null;
        
        for (int i = 0; i < eventObjects.Length; i++)
        {
            if (i == index)
            {
                eventObjects[i].gameObject.SetActive(true);
                tmp = eventObjects[i];
            }
            else
                eventObjects[i].gameObject.SetActive(false);
        }

        string endTime = string.Empty;
        DateTime now = DateTime.Now.ToLocalTime();
        DateTime end = new DateTime(tmp.endYear, tmp.endMonth, tmp.endDay);
        int leftDays = (end - now).Days;
        
        endTimeObject.SetActive(leftDays >= 0);
        noEndTimeObject.SetActive(leftDays < 0);
        
        if (leftDays < 0)
            endTime = "-";
        else
            endTime = leftDays.ToString();
        
        endTimeText.text = endTime;
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