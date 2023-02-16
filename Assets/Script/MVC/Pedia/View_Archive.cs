using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class View_Archive : ViewBehaviour
{
    [SerializeField] private Card_ArchiveType[] cards;
    [SerializeField] private Card_Archive[] cardArchives;

    [Title("Tween")]
    [SerializeField] private CanvasGroup[] cardCanvasGroups;
    [SerializeField] private RectTransform[] cardRects;

    private List<Vector2> cardOrigins = new List<Vector2>();

    public override void Open()
    {
        base.Open();

        for (int i = 0; i < cardArchives.Length; i++)
        {
            int index = i;
            DOVirtual.DelayedCall(0.4f, () => cardArchives[index].CheckRedActivate());
        }
        
        for (int i = 0; i < cardCanvasGroups.Length; i++)
        {
            CanvasGroup tmp = cardCanvasGroups[i];
            tmp.DOKill();
            tmp.alpha = 0;
        }

        DOVirtual.DelayedCall(0.2f, () =>
        {
            for (int i = 0; i < cardCanvasGroups.Length; i++)
            {
                CanvasGroup tmp = cardCanvasGroups[i];
                tmp.DOFade(1, 0.15f).SetDelay(i * 0.05f);
            }

            for (int i = 0; i < cardRects.Length; i++)
            {
                RectTransform tmp = cardRects[i];
                tmp.DOKill();
                Vector2 offset = cardOrigins[i];
                offset.y += 18f;
                tmp.DOAnchorPos(offset, 0.1f).From(cardOrigins[i]).SetLoops(2, LoopType.Yoyo).SetDelay(i * 0.05f);
            }
        });
    }

    public override void Init()
    {
        base.Init();

        for (int i = 0; i < cardRects.Length; i++)
        {
            RectTransform tmp = cardRects[i];
            cardOrigins.Add(tmp.anchoredPosition);
        }
        
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
