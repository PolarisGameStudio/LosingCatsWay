using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatNotifySystem : MvcBehaviour
{
    public Card_CatNotify[] card_CatNotifies;
    [SerializeField, ReadOnly] private List<Cat> cats = new List<Cat>();

    [Button(30)]
    public void Add(Cat cat)
    {
        cats.Add(cat);
        PopUp();
    }

    [Button(30)]
    public void Remove(Cat cat)
    {
        if (!cats.Contains(cat))
            return;
        cats.Remove(cat);
        PopUp();
    }

    [Button(30)]
    public void PopUp()
    {
        List<Cat> tmp = new List<Cat>();
        if (cats.Count <= 3)
            tmp = cats;
        else
            for (int i = 0; i < 3; i++)
                tmp.Add(cats[i]);

        for (int i = 0; i < 3; i++)
        {
            if (tmp.Count <= 0)
                break;

            var card = card_CatNotifies[i];
            if (card.isOpen)
                continue;
            
            var cat = tmp[0];
            
            card.Open(cat);
            cats.Remove(cat);
            card.OnClick = () =>
            {
                cat.FollowCat();
            };
        }
    }
}