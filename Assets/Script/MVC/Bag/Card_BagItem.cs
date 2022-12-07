using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_BagItem : MvcBehaviour
{
    [SerializeField] private GameObject focus;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI countText;

    public void SetData(Item item)
    {
        icon.sprite = item.icon;
        countText.text = item.Count.ToString();
    }

    public void SetFocus(bool flag)
    {
        focus.transform.DOScale(Vector3.one, 0.25f).From(Vector3.one * 1.25f).SetEase(Ease.OutBack);
        focus.SetActive(flag);
    }
}
