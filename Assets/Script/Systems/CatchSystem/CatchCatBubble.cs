using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatchCatBubble : MvcBehaviour
{
    [Title("Bubble")]
    [SerializeField] private GameObject talkBubble;
    [SerializeField] private GameObject hintBubble;
    [SerializeField] private GameObject[] smallBubbles;

    [Title("Sprite")]
    [SerializeField] private Sprite[] personalitySprites;
    [SerializeField] private Sprite loveSprite;
    [SerializeField] private Sprite hateSprite;

    [Title("UI")] [SerializeField] private Image iconImage;
    [SerializeField] private Image loveImage;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private CanvasGroup textCanvasGroup;

    private int hintIndex;
    private bool isTrigger;
    private bool isTweening;
    
    private List<int> _personalitys;
    private List<int> _levels;

    private Tween delayCall;

    public void Init(List<int> personalitys, List<int> levels)
    {
        hintIndex = -1;
        isTrigger = false;
        isTweening = false;

        _personalitys = personalitys;
        _levels = levels;

        talkBubble.transform.DOKill();
        hintBubble.transform.DOKill();
        for (int i = 0; i < smallBubbles.Length; i++)
            smallBubbles[i].transform.DOKill();
        delayCall?.Kill();

        talkBubble.transform.localScale = new Vector2(0, 1);
        hintBubble.transform.localScale = new Vector2(0, 1);
        for (int i = 0; i < smallBubbles.Length; i++)
            smallBubbles[i].transform.localScale = Vector2.zero;

        iconImage.DOFade(0, 0);
        loveImage.DOFade(0, 0);
        textCanvasGroup.DOFade(0, 0);
    }

    public void Open()
    {
        OpenTalk();
    }

    public void Active()
    {
        if (isTweening) // 動畫播一半的防呆
            return;
        
        hintIndex++;
        if (hintIndex >= _personalitys.Count)
            hintIndex = 0;
        
        if (!isTrigger)
            CloseTalk();
        else
            CloseHint();

        isTrigger = true;
        
        delayCall = DOVirtual.DelayedCall(0.23f, () =>
        {
            int personality = _personalitys[hintIndex];
            iconImage.sprite = personalitySprites[personality];

            int level = _levels[hintIndex];
            loveImage.sprite = level > 1 ? loveSprite : hateSprite;

            string id = $"{personality}{level}";
            hintText.text = App.factory.stringFactory.GetCatchCatHint(id);
            
            OpenHint();
        });
    }

    private void OpenTalk()
    {
        isTweening = true;
        
        for (int i = 0; i < smallBubbles.Length; i++)
            smallBubbles[i].transform.DOScale(Vector2.one, 0.15f).SetDelay(i * 0.1f);
        
        talkBubble.transform.DOScaleX(1, 0.18f).SetEase(Ease.OutBack)
            .SetDelay(0.3f)
            .OnComplete(() => isTweening = false);
    }

    private void CloseTalk()
    {
        isTweening = true;
        
        talkBubble.transform.DOScaleX(0, 0.12f).SetEase(Ease.Linear)
            .OnComplete(() => isTweening = false);
    }

    private void OpenHint()
    {
        isTweening = true;
        
        hintBubble.transform.DOScaleX(1, 0.18f).SetEase(Ease.OutBack)
            .OnStart(() =>
            {
                textCanvasGroup.DOFade(1, 0.18f);
                iconImage.DOFade(1, 0.18f);
                loveImage.DOFade(1, 0.18f);
            })
            .OnComplete(() => isTweening = false);
    }

    private void CloseHint()
    {
        isTweening = true;
        
        hintBubble.transform.DOScaleX(0, 0.12f).SetEase(Ease.Linear)
            .OnStart(() =>
            {
                textCanvasGroup.DOFade(0, 0.06f);
                iconImage.DOFade(0, 0.06f);
                loveImage.DOFade(0, 0.06f);
            })
            .OnComplete(() => isTweening = false);
    }
}
