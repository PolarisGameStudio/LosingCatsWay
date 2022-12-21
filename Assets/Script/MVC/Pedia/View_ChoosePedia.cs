using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_ChoosePedia : ViewBehaviour
{
    [SerializeField] private Card_ChoosePedia[] cards;
    
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
            string id = ids[i];
            
            if (id.Contains("WCI"))
                cards[i].ChangeColor(1);
            else if (id.Contains("WCH"))
                cards[i].ChangeColor(2);
            else if (id.Contains("WSK"))
                cards[i].ChangeColor(3);
            else
                cards[i].ChangeColor(0);
            
            Sprite sprite = App.factory.pediaFactory.GetPediaSprite(id);
            string title = App.factory.stringFactory.GetPediaTitle(id);
            bool unlock = App.factory.pediaFactory.GetPediaUnlock(id);
            cards[i].SetData(sprite, title, unlock);
        }
    }
}
