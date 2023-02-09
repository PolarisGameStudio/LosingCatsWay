using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using I2.Parallax;
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

    [Title("Nav")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI diamondText;

    [Title("Spine")] 
    public GameObject npc;

    [Title("MoneyBar")]
    [SerializeField] private Transform shopMoneyBar;
    [SerializeField] private Transform shopBuyMoneyBar;

    [Title("Gyro")]
    [SerializeField] private I2Parallax_Layer[] ParallaxLayers;

    private List<Card_ShopItem> itemPool = new List<Card_ShopItem>();
    private List<GameObject> layerPool = new List<GameObject>();

    public override void Init()
    {
        base.Init();
        
        CreatePool();
        
        App.model.shop.OnSelectedTypeChange += OnSelectedTypeChange;
        App.model.shop.OnSelectedItemsChange += OnSelectedItemsChange;
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
    }

    public override void Open()
    {
        // base.Open();
        UIView.InstantShow();
        
        npc.SetActive(true);
        itemsScrollBar.value = 1;

        SetParallex(true);
    }

    public override void Close()
    {
        base.Close();
        npc.SetActive(false);

        for (int i = 0; i < layerPool.Count; i++)
            if (layerPool[i].activeSelf)
                layerPool[i].SetActive(false);
        
        for (int i = 0; i < itemPool.Count; i++)
            if (itemPool[i].gameObject.activeSelf)
            {
                itemPool[i].gameObject.SetActive(false);
                itemPool[i].transform.DOKill();
                itemPool[i].transform.localScale = Vector2.zero;
            }

        SetParallex(false);
    }

    public void OpenShopBuy()
    {
        shopBuy.Open();
        shopMoneyBar.DOScale(Vector2.zero, 0.25f).SetEase(Ease.InBack);
        shopBuyMoneyBar.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.2f);
        SetParallex(false);
    }

    public void CloseShopBuy()
    {
        shopBuy.Close();
        shopBuyMoneyBar.DOScale(Vector2.zero, 0.25f).SetEase(Ease.InBack);
        shopMoneyBar.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack);
        SetParallex(true);
    }

    private void OnSelectedTypeChange(object value)
    {
        int type = Convert.ToInt32(value);

        if (type == -1)
            return;

        for (int i = 0; i < tabs.Length; i++)
            tabs[i].SetActive(false);

        tabs[type].SetActive(true);
    }

    private void OnSelectedItemsChange(object value)
    {
        List<Item> items = (List<Item>) value;

        float f = items.Count / 3f;
        int layerCount = (int)Mathf.Ceil(f);

        for (int i = 0; i < layerPool.Count; i++)
        {
            if (i >= layerCount)
            {
                if (layerPool[i].activeSelf)
                    layerPool[i].SetActive(false);
                continue;
            }

            layerPool[i].SetActive(true);
        }
        
        for (int i = 0; i < itemPool.Count; i++)
            if (itemPool[i].gameObject.activeSelf)
            {
                itemPool[i].gameObject.SetActive(false);
                itemPool[i].transform.DOKill();
                itemPool[i].transform.localScale = Vector2.zero;
            }

        for (int i = 0; i < itemPool.Count; i++)
        {
            if (i >= items.Count)
                break;

            var card = itemPool[i];
            card.SetData(items[i]);
            card.gameObject.SetActive(true);
            card.transform.DOScale(Vector2.one, 0.15f).From(Vector2.zero).SetDelay(i * 0.06f);
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

    private void SetParallex(bool value)
    {
        for (int i = 0; i < ParallaxLayers.Length; i++)
        {
            ParallaxLayers[i].UpdateLayer(Vector2.zero);
            ParallaxLayers[i].enabled = value;
        }
    }

    private void CreatePool()
    {
        var items = App.factory.itemFactory.GetItemByType(0); // all

        for (int i = 0; i < items.Count; i++)
        {
            var newItem = Instantiate(shopItemPrefab, content);
            newItem.transform.localScale = Vector2.zero;
            newItem.gameObject.SetActive(false);
            itemPool.Add(newItem);
        }
        
        float f = items.Count / 3f;
        int layerCount = (int)Mathf.Ceil(f);

        for (int i = 0; i < layerCount; i++)
        {
            var newLayer = Instantiate(layerPrefab, layerContent);
            layerPool.Add(newLayer);
        }
    }
}