using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class View_CatGuide : ViewBehaviour
{
    [Title("TopLeft")]
    [SerializeField] private Card_LevelReward_Top nowLevelCard;
    [SerializeField] private MyTween_Scale nowLevelGetTween;
    
    [Title("TopRight")]
    [SerializeField] private Card_LevelReward_Top nextLevelCard;
    
    [Title("Bottom")]
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private CardLevelReward[] cards;

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

        nowLevelGetTween.Play();
    }

    private void OnLevelChange(object value)
    {
        int level = (int)value;
        
        if (level >= 40)
        {
            for (int i = 0; i < cards.Length; i++)
                cards[i].gameObject.SetActive(false);
            nowLevelCard.SetData(level, true);
            nextLevelCard.SetData(level + 1, true);
            return;
        }
        
        MathfExtension.GetNumberRangeByTen(level, out int start, out int end);

        // 上面顯示的等級獎勵
        nowLevelCard.SetData(level);
        nextLevelCard.SetData(level + 1);
        
        // 下面一列的等級獎勵
        List<int> levels = new List<int>();
        for (int i = start; i <= end; i++)
            levels.Add(i);
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].SetData(levels[i]);
            cards[i].gameObject.SetActive(true);
        }
    }
}
