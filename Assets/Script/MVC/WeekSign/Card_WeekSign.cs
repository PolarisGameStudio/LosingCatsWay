using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_WeekSign : MvcBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject whiteMask;
    [SerializeField] private GameObject isGetObject;

    public void SetData(Reward reward)
    {
        countText.text = $"x{reward.count}";
        icon.sprite = reward.item.icon;
        whiteMask.SetActive(false);
        isGetObject.SetActive(false);
    }

    public void SetIsGet()
    {
        whiteMask.SetActive(true);
        isGetObject.SetActive(true);
        isGetObject.transform.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack);
    }
}
