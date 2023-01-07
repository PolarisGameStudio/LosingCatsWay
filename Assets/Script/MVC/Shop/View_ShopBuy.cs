using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_ShopBuy : ViewBehaviour
{
    [Title("ItemInfo")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptText;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI bagCountText;
    [SerializeField] private TextMeshProUGUI buyCountText;
    [SerializeField] private GameObject satietyIcon;
    [SerializeField] private GameObject moistureIcon;
    [SerializeField] private GameObject funIcon;
    [SerializeField] private Slider slider;

    [Title("Price")] [SerializeField] private Sprite diamondSprite;
    [SerializeField] private Sprite moneySprite;
    [SerializeField] private Image priceIcon;
    [SerializeField] private TextMeshProUGUI totalPriceText;

    [Title("Nav")] [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI diamondText;

    public override void Open()
    {
        base.Open();
        slider.value = slider.minValue;
    }

    public override void Init()
    {
        base.Init();
        App.model.shop.OnBuyCountChange += OnBuyCountChange;
        App.model.shop.OnTotalAmountChange += OnTotalAmountChange;
        App.model.shop.OnSelectedItemChange += OnSelectedItemChange;
        
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
    }

    private void OnDiamondChange(object value)
    {
        int diamond = (int)value;
        diamondText.text = diamond.ToString();
    }

    private void OnCoinChange(object value)
    {
        int coin = (int)value;
        coinText.text = coin.ToString();
    }

    private void OnSelectedItemChange(object value)
    {
        Item item = (Item)value;
        itemIcon.sprite = item.icon;
        nameText.text = item.Name;
        descriptText.text = item.Description;
        bagCountText.text = item.Count.ToString("00");
        
        satietyIcon.SetActive(item.ForSatiety);
        moistureIcon.SetActive(item.ForMoisture);
        funIcon.SetActive(item.ForFun);

        switch (item.itemBoughtType)
        {
            case ItemBoughtType.Coin:
                priceIcon.sprite = moneySprite;
                break;
            case ItemBoughtType.Diamond:
                priceIcon.sprite = diamondSprite;
                break;
        }
    }

    private void OnTotalAmountChange(object value)
    {
        int total = (int)value;
        totalPriceText.text = total.ToString("N0");
    }

    private void OnBuyCountChange(object value)
    {
        int count = (int)value;
        buyCountText.text = count.ToString();
        slider.value = count;
    }
}
