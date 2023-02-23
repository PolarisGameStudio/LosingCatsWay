using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_ChoosePedia : ViewBehaviour
{
    public Card_ChoosePedia[] cards;
    
    public override void Init()
    {
        base.Init();
        App.model.pedia.OnUsingPediaIdsChange += OnUsingPediaIdsChange;
    }

    private void OnUsingPediaIdsChange(object value)
    {
        var ids = (List<string>)value;
        for (int i = 0; i < cards.Length; i++)
        {
            if (i >= ids.Count)
            {
                cards[i].gameObject.SetActive(false);
                continue;
            }

            cards[i].gameObject.SetActive(true);
            cards[i].SetData(ids[i]);
        }
    }
}
