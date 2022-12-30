using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatNotifySystem : MvcBehaviour
{
    public Card_CatNotify[] card_CatNotifies;
    List<Cat> cats = new List<Cat>();

    [Button(30)]
    public void Add(Cat cat)
    {
        cats.Add(cat);
        PopUp();
    }

    [Button(30)]
    public void Remove(Cat cat)
    {
        if (!cats.Contains(cat)) return;
        cats.Remove(cat);
        PopUp();
    }

    [Button(30)]
    public void PopUp()
    {
        for (int i = 0; i < card_CatNotifies.Length; i++)
            card_CatNotifies[i].Init();

        for (int i = 0; i < card_CatNotifies.Length; i++)
        {
            if (i >= cats.Count) break;

            var index = i;
            var card = card_CatNotifies[index];
            var cat = cats[index];

            card.Open(cat);
            card.OnClick = () =>
            {
                cat.FollowCat();
                if (cats.Contains(cat))
                    cats.Remove(cat);
            };
        }
    }
}