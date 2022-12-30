using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_BagChooseCat : ViewBehaviour
{
    [SerializeField] private Card_BagChooseCat[] cards;

    public override void Init()
    {
        base.Init();
        App.system.cat.OnCatsChange += OnCatsChange;
    }

    private void OnCatsChange(object value)
    {
        List<Cat> cats = (List<Cat>)value;
        for (int i = 0; i < cards.Length; i++)
        {
            if (i >= cats.Count)
                cards[i].SetActive(false);
            else
            {
                cards[i].SetActive(true);
                cards[i].SetData(cats[i].cloudCatData);
                cards[i].SetSelect(false);
            }
        }
    }
}
