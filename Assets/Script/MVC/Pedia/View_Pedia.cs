using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class View_Pedia : ViewBehaviour
{
    public View_Archive archive;
    public View_PediaCats pediaCats;
    public View_SubPedia subPedia;

    [SerializeField] private GameObject[] tabMasks;

    public GameObject archiveRedPoint;
    public GameObject catRedPoint;

    public override void Open()
    {
        base.Open();
        CheckRedActivate();
    }

    public override void Init()
    {
        base.Init();
        App.model.pedia.OnTabIndexChange += OnTabIndexChange;
    }

    private void OnTabIndexChange(object value)
    {
        int index = (int)value;
        for (int i = 0; i < tabMasks.Length; i++)
            tabMasks[i].SetActive(i == index);
    }

    private void CheckRedActivate()
    {
        if (archiveRedPoint.activeSelf)
        {
            archiveRedPoint.SetActive(false);
            DOVirtual.DelayedCall(0.5f, () => archiveRedPoint.SetActive(true));
        }
        
        if (catRedPoint.activeSelf)
        {
            catRedPoint.SetActive(false);
            DOVirtual.DelayedCall(0.5f, () => catRedPoint.SetActive(true));
        }
    }
}
