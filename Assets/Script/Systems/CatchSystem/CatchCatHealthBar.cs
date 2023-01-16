using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class CatchCatHealthBar : MvcBehaviour
{
    [Title("Bar")]
    [SerializeField] private RectTransform barRect;
    [SerializeField] private Image fillImage;
    
    [Title("UI")]
    [SerializeField] private Image[] personalityIcons;
    [SerializeField] private RectTransform heartRect;
    [SerializeField] private RectTransform titleRect;
    
    [Title("Sprite")]
    [SerializeField] private Sprite[] foodSprites;
    [SerializeField] private Sprite[] soundSprites;
    [SerializeField] private Sprite[] containerSprites;
    [SerializeField] private Sprite[] toySprites;

    public Callback OnOpenPersonalityComplete;
    
    public void Init()
    {
        barRect.localScale = Vector2.zero;
        fillImage.fillAmount = 0;
        heartRect.anchoredPosition = new Vector2(heartRect.anchoredPosition.x, 0);
        titleRect.anchoredPosition = new Vector2(titleRect.anchoredPosition.x, 0);
        for (int i = 0; i < personalityIcons.Length; i++)
        {
            personalityIcons[i].transform.localScale = Vector2.zero;
            personalityIcons[i].gameObject.SetActive(false);
        }
    }
    
    public void Open()
    {
        barRect.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack);
        fillImage.DOFillAmount(1, 0.35f).SetEase(Ease.OutExpo).SetDelay(0.25f);
        heartRect.DOAnchorPosY(52, 0.2f).SetEase(Ease.OutBack).SetDelay(0.3125f);
        titleRect.DOAnchorPosY(52, 0.2f).SetEase(Ease.OutBack).SetDelay(0.375f);
    }

    public void Close()
    {
        titleRect.DOAnchorPosY(52, 0.2f).SetEase(Ease.OutBack);
        heartRect.DOAnchorPosY(52, 0.2f).SetEase(Ease.OutBack).SetDelay(0.1f);
        barRect.DOScale(Vector2.zero, 0.25f).SetEase(Ease.InBack).SetDelay(0.25f);
    }

    public void OpenPersonality(int personality, int level)
    {
        Sprite[] sprites = null;
        switch (personality)
        {
            case 0:
                sprites = foodSprites;
                break;
            case 1:
                sprites = toySprites;
                break;
            case 2:
                sprites = containerSprites;
                break;
            case 3:
                sprites = soundSprites;
                break;
        }
        
        Image tmp = null;
        for (int i = 0; i < personalityIcons.Length; i++)
            if (!personalityIcons[i].gameObject.activeSelf)
            {
                tmp = personalityIcons[i];
                break;
            }

        tmp.sprite = sprites[level];
        tmp.gameObject.SetActive(true);
        tmp.transform.DOScale(Vector2.one, 0.15f).From(Vector2.zero).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                OnOpenPersonalityComplete?.Invoke();
                OnOpenPersonalityComplete = null;
            });
    }

    public void ChangeBarValue(float value)
    {
        fillImage.DOFillAmount(value, 0.5f).SetEase(Ease.OutExpo).SetSpeedBased();
    }
}
