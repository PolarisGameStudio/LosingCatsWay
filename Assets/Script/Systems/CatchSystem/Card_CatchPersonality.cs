using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Card_CatchPersonality : MvcBehaviour
{
    [SerializeField] private GameObject questionImage;
    [SerializeField] private GameObject arrowImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image levelImage;
    [SerializeField] private Image backImage;
    [SerializeField] private Image darkMask;

    private bool isFlip;
    [ReadOnly] public bool isCanFlip;
    [ReadOnly] public bool hasFlip;

    public void SetActive(bool value)
    {
        if (value)
        {
            questionImage.SetActive(false);
            arrowImage.SetActive(true);
            iconImage.gameObject.SetActive(true);
            levelImage.gameObject.SetActive(true);
            backImage.gameObject.SetActive(isFlip);
        }
        else
        {
            questionImage.SetActive(true);
            arrowImage.SetActive(false);
            iconImage.gameObject.SetActive(false);
            levelImage.gameObject.SetActive(false);
            backImage.gameObject.SetActive(false);
        }
    }

    public void SetData(int personality, int level)
    {
        iconImage.sprite = App.factory.catFactory.GetPersonalitySprite(personality);
        levelImage.sprite = App.factory.catFactory.GetPersonalityLevelSprite(level);
        backImage.sprite = App.factory.catFactory.GetPersonalityTipsSprite(personality);

        isFlip = false;
        hasFlip = false;
        
        backImage.gameObject.SetActive(isFlip);
        iconImage.gameObject.SetActive(!isFlip);
    }

    public void DoPopCard()
    {
        if (hasFlip) return;
        
        if (isCanFlip)
        {
            transform.DOKill();
            transform.localScale = Vector2.one;
            transform.DOScale(new Vector2(1.1f, 1.1f), 0.3f).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            transform.DOKill();
            transform.localScale = Vector2.one;
        }
    }

    public void DoFlipCard()
    {
        if (!isCanFlip) return;

        hasFlip = true;
        transform.DOKill();
        transform.localScale = Vector2.one;
        
        transform.DORotate(new Vector3(0, 90, 0), 0.25f).From(Vector3.zero).SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                isFlip = !isFlip;
                backImage.gameObject.SetActive(isFlip);
                iconImage.gameObject.SetActive(!isFlip);
            });

        transform.DORotate(new Vector3(0, 0, 0), 0.25f).From(new Vector3(0, 90, 0)).SetDelay(0.25f + 0.0625f);
    }

    public void DoDark()
    {
        darkMask.DOFade(0.5f, 0.25f);
    }

    public void DoLight()
    {
        darkMask.DOFade(0, 0.25f);
    }
}
