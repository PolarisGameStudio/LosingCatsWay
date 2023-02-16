using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CatNotifySystem : MvcBehaviour
{
    [SerializeField] private Card_CatNotify cardCatNotify;
    private Queue<Cat> waitingCats = new Queue<Cat>();
    private List<Card_CatNotify> displayedNotifies = new List<Card_CatNotify>();

    public void Add(Cat cat)
    {
        waitingCats.Enqueue(cat);
        RefreshNotify();
    }

    public void Remove(Cat cat)
    {
        if (waitingCats.Contains(cat))
        {
            waitingCats = new Queue<Cat>(waitingCats.Where(x => x != cat));
            return;
        }

        for (int i = 0; i < displayedNotifies.Count; i++)
        {
            if (displayedNotifies[i].notifyCat != cat)
                continue;
            Destroy(displayedNotifies[i].gameObject);
            displayedNotifies.RemoveAt(i);
        }
        
        RefreshNotify();
    }

    private void RefreshNotify() // 刷新通知
    {
        if (App.view.lobby.cardCatNotifyContent.childCount >= 3)
            return;

        if (waitingCats.Count <= 0)
            return;

        int emptyCount = 3 - App.view.lobby.cardCatNotifyContent.childCount; // 剩餘的通知數量

        for (int i = 0; i < emptyCount; i++)
        {
            if (i >= waitingCats.Count)
                break;
            
            var card = Instantiate(cardCatNotify, App.view.lobby.cardCatNotifyContent);
            var cat = waitingCats.Dequeue();
            card.SetData(cat);
            card.Open();
            displayedNotifies.Add(card);
        }
    }

    public Card_CatNotify GetNotify(Cat cat)
    {
        for (int i = 0; i < displayedNotifies.Count; i++)
        {
            var tmp = displayedNotifies[i];
            if (tmp.notifyCat != cat)
                continue;
            return tmp;
        }

        return null;
    }

    public void CheckRedActivate()
    {
        for (int i = 0; i < displayedNotifies.Count; i++)
            displayedNotifies[i].CheckRedActivate();
    }
}