using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class Card_MonthSign : MvcBehaviour
{
    [SerializeField] private GameObject signMark;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image icon;
    [SerializeField] private Transform getIconTransform;
    [SerializeField] private GameObject doubleBg;

    [Title("Color")] [SerializeField] private Color32 normalTextColor;
    [SerializeField] private Color32 doubleTextColor;

    bool isSign;

    public void SetDate(int date)
    {
        dateText.text = date.ToString("00");
    }

    public void SetReward(Reward reward)
    {
        if (reward == null) return;

        icon.sprite = reward.item.icon;
        countText.text = reward.count.ToString("00");
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void SetDouble(bool value)
    {
        doubleBg.SetActive(value);
        countText.color = value ? doubleTextColor : normalTextColor;
    }

    public bool IsSign
    {
        get => isSign;
        set
        {
            if (!isSign && value)
                DoGetIconJump();
            isSign = value;
            signMark.SetActive(value);
        }
    }
    
    private void DoGetIconJump()
    {
        Transform fromParent = transform.parent;
        Transform toParent = App.controller.monthSign.FrontTransform;
        int siblingIndex = transform.GetSiblingIndex();

        transform.SetParent(toParent);
        getIconTransform.DOScale(new Vector2(2, 2), 0.15f).From(new Vector2(1.5f, 1.5f)).SetEase(Ease.OutExpo);
        getIconTransform.DOScale(new Vector2(0.8f, 0.8f), 0.1f).SetEase(Ease.OutExpo).SetDelay(0.15f);
        getIconTransform.DOScale(Vector2.one, 0.05f).SetDelay(0.25f)
            .OnComplete(() =>
            {
                transform.SetParent(fromParent);
                transform.SetSiblingIndex(siblingIndex);
            });
    }
}
