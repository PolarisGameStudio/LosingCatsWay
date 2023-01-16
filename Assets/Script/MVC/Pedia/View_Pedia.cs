using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_Pedia : ViewBehaviour
{
    public View_Archive archive;
    public View_PediaCats pediaCats;
    public View_SubPedia subPedia;

    [SerializeField] private GameObject[] tabMasks;

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
}
