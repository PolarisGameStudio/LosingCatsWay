using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class View_ClinicChooseCat : ViewBehaviour
{
    [SerializeField] private Card_ClinicChooseCat[] cards;

    public override void Close()
    {
        base.Close();
        for (int i = 0; i < cards.Length; i++)
            DOTween.Kill(cards[i].transform, true);
    }

    public override void Init()
    {
        base.Init();
        App.model.clinic.OnFunctionIndexChange += OnFunctionIndexChange;
        App.model.clinic.OnMyCatsChange += OnMyCatsChange;
        App.model.clinic.OnCatIndexChange += OnCatIndexChange;
    }

    private void OnFunctionIndexChange(object value)
    {
        int index = (int)value;
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].FunctionIndex = index;
        }
    }

    private void OnMyCatsChange(object value)
    {
        var cats = (List<Cat>)value;

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].SetActive(false);
            cards[i].SetSelect(false);
        }

        for (int i = 0; i < cats.Count; i++)
        {
            cards[i].SetActive(true);
            cards[i].SetData(cats[i]);
            var tmp = cards[i].transform;
            tmp.DOScale(Vector2.one, 0.15f).From(Vector3.zero)
                .SetDelay(i * 0.1f);
        }
    }

    private void OnCatIndexChange(object value)
    {
        int index = (int)value;

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].SetSelect(false);
        }

        cards[index].SetSelect(true);
    }
}
