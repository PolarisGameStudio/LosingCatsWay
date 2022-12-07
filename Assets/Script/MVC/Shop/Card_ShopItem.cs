using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_ShopItem : MvcBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image priceIcon;
    [SerializeField] private Sprite moneySprite;
    [SerializeField] private Sprite diamondSprite;
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockMask;

    public void SetData(Item item)
    {
        nameText.text = item.Name;
        icon.sprite = item.icon;
        priceText.text = item.price.ToString();

        switch (item.itemBoughtType)
        {
            case ItemBoughtType.Coin:
                priceIcon.sprite = moneySprite;
                break;
            case ItemBoughtType.Diamond:
                priceIcon.sprite = diamondSprite;
                break;
        }

        button.interactable = item.CanBuyAtStore;
        lockMask.SetActive(!item.CanBuyAtStore);
    }

    public void Select()
    {
        int index = transform.GetSiblingIndex();
        App.controller.shop.SelectItem(index - 1); //1 是架子層
    }
}
