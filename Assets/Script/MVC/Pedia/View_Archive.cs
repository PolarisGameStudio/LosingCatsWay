using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_Archive : ViewBehaviour
{
    [SerializeField] private Card_ArchiveType[] cards;
    [SerializeField] private Card_Archive[] cardArchives;
    
    public override void Init()
    {
        base.Init();
        App.model.pedia.OnSelectedArchiveTypeChange += OnSelectedArchiveTypeChange;
        App.model.pedia.OnArchiveQuestsChange += OnArchiveQuestsChange;
    }

    private void OnSelectedArchiveTypeChange(object value)
    {
        int index = (int)value;

        if (index < 0)
            return;

        for (int i = 0; i < cards.Length; i++)
        {
            if (i == index)
                cards[i].SetSelect(true);
            else
                cards[i].SetSelect(false);
        }
    }

    private void OnArchiveQuestsChange(object value)
    {
        List<Quest> quests = (List<Quest>)value;
        for (int i = 0; i < cardArchives.Length; i++)
            cardArchives[i].SetData(quests[i]);
    }
}
