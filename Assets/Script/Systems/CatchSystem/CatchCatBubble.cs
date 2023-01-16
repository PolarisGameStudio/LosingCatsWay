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
    [SerializeField] private TextMeshProUGUI talkText;
    [SerializeField] private TextMeshProUGUI hintText;

    public Callback OnCloseTalk;
    public Callback OnCloseHint;

    public void Init()
    {
        talkBubble.transform.localScale = Vector2.zero;
        hintBubble.transform.localScale = Vector2.zero;
        for (int i = 0; i < smallBubbles.Length; i++)
            smallBubbles[i].transform.localScale = Vector2.zero;
    }

    public void OpenTalk() // 心裡話
    {
        //Dotween
        for (int i = 0; i < smallBubbles.Length; i++)
            smallBubbles[i].transform.DOScale(Vector2.one, 0.15f).SetDelay(i * 0.1f);
        talkBubble.transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack).SetDelay(0.3f);

        //TODO 內心話
    }

    public void CloseTalk()
    {
        talkBubble.transform.DOScale(Vector2.zero, 0.25f).SetEase(Ease.InBack);
        for (int i = smallBubbles.Length - 1; i >= 0; i--)
            smallBubbles[i].transform.DOScale(Vector2.zero, 0.15f).SetDelay(i * 0.125f);

        DOVirtual.DelayedCall(0.3f, () =>
        {
            OnCloseTalk?.Invoke();
            OnCloseTalk = null;
        });
    }

    public void OpenHint(int personality, int level)
    {
        //Dotween
        for (int i = 0; i < smallBubbles.Length; i++)
            smallBubbles[i].transform.DOScale(Vector2.one, 0.15f).SetDelay(i * 0.1f);
        hintBubble.transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack).SetDelay(0.3f);
        
        iconImage.sprite = personalitySprites[personality];
        loveImage.sprite = level > 1 ? loveSprite : hateSprite;
        //TODO HintText
    }

    public void CloseHint()
    {
        hintBubble.transform.DOScale(Vector2.zero, 0.25f).SetEase(Ease.InBack);
        for (int i = smallBubbles.Length - 1; i >= 0; i--)
            smallBubbles[i].transform.DOScale(Vector2.zero, 0.15f).SetDelay(i * 0.125f);

        DOVirtual.DelayedCall(0.3f, () =>
        {
            OnCloseHint?.Invoke();
            OnCloseHint = null;
        });
    }
}
