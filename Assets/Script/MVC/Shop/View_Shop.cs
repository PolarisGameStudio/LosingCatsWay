using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_Shop : ViewBehaviour
{
    [SerializeField] private GameObject[] tabs;
    
    [Title("Items")]
    [SerializeField] private Transform layerContent;
    [SerializeField] private GameObject layerPrefab;
    [SerializeField] private Card_ShopItem shopItemPrefab;
    [SerializeField] private Transform content;
    
    [Title("UI")]
    [SerializeField] private View_ShopBuy shopBuy;
    [SerializeField] private Scrollbar itemsScrollBar;

    [Title("Nav")] [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI diamondText;

    [Title("Spine")] 
    public GameObject npc;
    
    public override void Init()
    {
        base.Init();
        App.model.shop.OnSelectedTypeChange += OnSelectedTypeChange;
        App.model.shop.OnSelectedItemsChange += OnSelectedItemsChange;
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
    }

    public override void Open()
    {
        base.Open();
        npc.SetActive(true);
        itemsScrollBar.value = 1;
    }

    public override void Close()
    {
        base.Close();
        npc.SetActive(false);
    }

    public void OpenShopBuy()
    {
        shopBuy.Open();
    }

    public void CloseShopBuy()
    {
        shopBuy.Close();
    }

    private void OnSelectedTypeChange(object value)
    {
        int type = Convert.ToInt32(value);

        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(false);
        }

        tabs[type].SetActive(true);
    }

    private void OnSelectedItemsChange(object value)
    {
        List<Item> items = (List<Item>) value;

        #region 生架子層數

        for (int i = 0; i < layerContent.childCount; i++)
            Destroy(layerContent.GetChild(i).gameObject);
        
        float f = items.Count / 3f;
        int layerCount = (int)Mathf.Ceil(f);

        for (int i = 0; i < layerCount; i++)
            Instantiate(layerPrefab, layerContent);

        #endregion
        
        for (int i = 1; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        for (int i = 0; i < items.Count; i++)
        {
            Card_ShopItem card = Instantiate(shopItemPrefab, content);
            card.SetData(items[i]);
        }
        
        itemsScrollBar.value = 1;
    }

    private void OnCoinChange(object value)
    {
        int coin = Convert.ToInt32(value);
        coinText.text = coin.ToString();
    }

    private void OnDiamondChange(object value)
    {
        int diamond = Convert.ToInt32(value);
        diamondText.text = diamond.ToString();
    }
}