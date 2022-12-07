using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_Entrance : ViewBehaviour
{
    [SerializeField] private Cat_Entrance[] frontCats;
    [SerializeField] private Cat_Entrance[] backCats;
    [SerializeField] private Cat_Entrance deadCat;
    [SerializeField] private GameObject closeButton;

    public override void Init()
    {
        base.Init();
        App.model.entrance.OnCatsChange += OnCatsChange;
        App.model.entrance.OnDeadCatChange += OnDeadCatChange;
        App.model.entrance.OnOpenTypeChange += OnOpenTypeChange;
    }

    public override void Close()
    {
        base.Close();
        HideAllCats();
    }

    private void OnCatsChange(object value)
    {
        List<Cat> cats = (List<Cat>)value;
        backCats.Shuffle();

        closeButton.SetActive(true);
        HideAllCats();

        for (int i = 0; i < cats.Count; i++)
        {
            var catData = cats[i].cloudCatData;

            if (i < 8)
            {
                frontCats[i].SetCatData(catData);
                frontCats[i].SetActive(true);
            }
            else
            {
                backCats[i].SetCatData(catData);
                backCats[i].SetActive(true);
            }
        }
    }

    private void OnDeadCatChange(object value)
    {
        if (value == null) return;
        HideAllCats();
        
        Cat cat = (Cat)value;
        deadCat.SetCatData(cat.cloudCatData);
        deadCat.SetActive(true);
        deadCat.StartDead();

        closeButton.SetActive(false);
    }

    private void OnOpenTypeChange(object value)
    {
        int index = Convert.ToInt32(value);

        if (index == 0)
        {
            //正常
            closeButton.SetActive(true);
            return;
        }

        if (index == 1)
        {
            //死
            closeButton.SetActive(false);
            return;
        }
    }

    #region Method

    private void HideAllCats()
    {
        for (int i = 0; i < frontCats.Length; i++)
        {
            frontCats[i].SetActive(false);
        }

        for (int i = 0; i < backCats.Length; i++)
        {
            backCats[i].SetActive(false);
        }

        deadCat.SetActive(false);
    }

    #endregion
}
