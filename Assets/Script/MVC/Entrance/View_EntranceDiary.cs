using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class View_EntranceDiary : ViewBehaviour
{
    [Title("Book")] [SerializeField] private Card_EntranceDiary leftBook;
    [SerializeField] private Card_EntranceDiary rightBook;
    [SerializeField] private Card_EntranceDiary centerBook;

    public override void Init()
    {
        base.Init();
        App.model.entrance.OnLeftCatDataChange += OnLeftCatDataChange;
        App.model.entrance.OnRightCatDataChange += OnRightCatDataChange;
        App.model.entrance.OnCenterCatDataChange += OnCenterCatDataChange;
    }

    private void OnCenterCatDataChange(object value)
    {
        var losingCat = (CloudLosingCatData)value;
        centerBook.SetData(losingCat);
        centerBook.SetActive(true);
        centerBook.SetSelect(true);
    }

    private void OnRightCatDataChange(object value)
    {
        if (value == null)
        {
            rightBook.SetActive(false);
            return;
        }
        
        var losingCat = (CloudLosingCatData)value;
        rightBook.SetData(losingCat);
        rightBook.SetSelect(false);
    }

    private void OnLeftCatDataChange(object value)
    {
        if (value == null)
        {
            leftBook.SetActive(false);
            return;
        }

        var losingCat = (CloudLosingCatData)value;
        leftBook.SetData(losingCat);
        leftBook.SetSelect(true);
    }

    private void OnLosingCatsChange(object value)
    {
        var cats = (List<CloudLosingCatData>)value;
        
        if (cats.Count <= 1)
        {
            centerBook.SetData(cats[0]);
            return;
        }

        if (cats.Count == 2)
        {
            centerBook.SetData(cats[0]);
            rightBook.SetData(cats[1]);
            return;
        }

        int lastIndex = cats.Count - 1;
        leftBook.SetData(cats[lastIndex]);
        
        //TODO Descript
    }
}
