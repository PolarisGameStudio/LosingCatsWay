using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_CatGuide : ViewBehaviour
{
    [Title("TopLeft")] [SerializeField] private Card_CatGuide nowLevelCard;
    [SerializeField] private MyTween_Scale nowLevelGetTween;
    
    [Title("TopRight")] [SerializeField] private Card_CatGuide nextLevelCard;
    
    [Title("Bottom")] [SerializeField] private Card_CatGuide[] cards;
    [SerializeField] private Scrollbar scrollbar;

    [Title("Bg")] [SerializeField] private Transform bg;

    [Title("DoTween")] [SerializeField] private RectTransform topRect;
    [SerializeField] private CanvasGroup topCanvasGroup;
    
    public override void Init()
    {
        base.Init();
        App.system.player.OnLevelChange += OnLevelChange;
    }

    public override void Open()
    {
        scrollbar.value = 0;
        
        UIView.InstantShow();
        
        bg.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutExpo);

        topRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutExpo).SetDelay(0.1f);
        topCanvasGroup.DOFade(1, 0.25f).From(0).SetDelay(0.1f).SetEase(Ease.InCubic);

        for (int i = 0; i < cards.Length; i++)
            cards[i].transform.localScale = Vector2.zero;

        for (int i = 0; i < cards.Length; i++)
            cards[i].DoFlip(0.125f * (i + 1));
        
        nowLevelGetTween.Play();
    }

    private void OnLevelChange(object value)
    {
        int level = (int)value;
        List<Reward[]> rewards;
        MathfExtension.GetNumberRangeByTen(level, out int start, out int end);
        
        //封頂
        if (level >= 40)
            rewards = App.factory.itemFactory.GetRewardsByLevel(39);
        else
            rewards = App.factory.itemFactory.GetRewardsByLevel(App.system.player.Level);

        for (int i = 0; i < cards.Length; i++)
        {
            int tmpLevel = start + i;
            cards[i].SetLevel(tmpLevel);
        }
        
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].SetData(rewards[i]);
            if (level >= 40)
            {
                cards[i].SetSelect(false);
                cards[i].IsGet(true);
            }
            else
            {
                cards[i].SetSelect(false);
                cards[i].IsGet(false);
            }
        }
        
        //Find current level
        int index = level % 10;
        int now = (index == 0) ? 9 : index - 1;
        int next = (now == 9) ? 0 : now + 1;

        if (now != 9)
        {
            cards[now].IsGet(true);
            for (int i = 0; i < now; i++)
            {
                cards[i].IsGet(true);
            }
            nowLevelCard.SetLevel(level);
            nowLevelCard.SetData(rewards[now]);
        }
        else
        {
            var lastRewards = App.factory.itemFactory.GetRewardsByLevel(App.system.player.Level - 1);
            nowLevelCard.SetLevel(level);
            nowLevelCard.SetData(lastRewards.Last());
        }

        if (level >= 40)
        {
            cards[next].SetSelect(false);
            nextLevelCard.SetLevel(level + 1);
            nextLevelCard.SetData(null);
        }
        else
        {
            cards[next].SetSelect(true);
            nextLevelCard.SetLevel(level + 1);
            nextLevelCard.SetData(rewards[next]);
        }
    }
}
