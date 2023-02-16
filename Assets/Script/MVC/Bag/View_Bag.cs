using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_Bag : ViewBehaviour
{
    [Title("Tab")]
    [SerializeField] private GameObject[] showButtons;
    [SerializeField] private Card_BagItem item;
    [SerializeField] private Transform content;

    [Title("UI")] 
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private GameObject useButton;

    [Title("Nav")] 
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI diamondText;

    [Title("Position")]
    [SerializeField] private RectTransform iconRect;
    [SerializeField] private Vector2[] iconPositions;
    
    [Title("Red")]
    public GameObject[] redPoints;

    private List<Card_BagItem> cardBagItems = new List<Card_BagItem>();

    public override void Open()
    {
        base.Open();
        CheckRedActivate();
    }

    public override void Close()
    {
        base.Close();

        for (int i = 0; i < cardBagItems.Count; i++)
            Destroy(cardBagItems[i].gameObject);
        
        cardBagItems.Clear();
    }

    public override void Init()
    {
        base.Init();

        App.model.bag.onSelectedItemsChange += OnSelectedItemsChange;
        App.model.bag.onSelectedItemChange += OnSelectedItemChange;
        App.model.bag.onTypeChange += OnTypeChange;
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
    }

    private void OnDiamondChange(object value)
    {
        int diamond = Convert.ToInt32(value);
        diamondText.text = diamond.ToString();
    }

    private void OnCoinChange(object value)
    {
        int coin = Convert.ToInt32(value);
        coinText.text = coin.ToString();
    }

    public void OnSelectedItemsChange(object value)
    {
        List<Item> items = (List<Item>) value;

        // Pooling
        if (items.Count > cardBagItems.Count)
        {
            int refillCount = items.Count - cardBagItems.Count;
            for (int i = 0; i < refillCount; i++)
            {
                Card_BagItem newCard = Instantiate(item, content);
                newCard.gameObject.SetActive(false);
                cardBagItems.Add(newCard);
            }
        }

        for (int i = 0; i < cardBagItems.Count; i++)
            cardBagItems[i].gameObject.SetActive(false);

        for (int i = 0; i < cardBagItems.Count; i++)
        {
            if (i >= items.Count)
            {
                cardBagItems[i].gameObject.SetActive(false);
                continue;
            }
            
            Card_BagItem card = cardBagItems[i];
            card.SetData(items[i]);
            card.gameObject.SetActive(true);
            card.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutExpo).SetDelay(i * 0.0625f);
        }
    }

    public void OnSelectedItemChange(object value)
    {
        if (value == null)
        {
            itemName.text = "";
            itemCount.text = "";
            itemDescription.text = "";
            itemImage.enabled = false;
            useButton.SetActive(false);
            return;
        }

        Item item = (Item) value;

        itemName.text = item.Name;
        itemCount.text = item.Count.ToString();
        itemDescription.text = item.Description;

        itemImage.enabled = true;
        itemImage.sprite = item.icon;

        Vector2 tmpPos = Vector2.zero;
        switch (item.itemType)
        {
            case ItemType.Feed:
                tmpPos = iconPositions[0];
                break;
            case ItemType.Tool:
                tmpPos = iconPositions[1];
                break;
            case ItemType.Litter:
                tmpPos = iconPositions[2];
                break;
            case ItemType.Room:
                tmpPos = iconPositions[3];
                break;
            case ItemType.Special:
                tmpPos = iconPositions[5];
                break;
            case ItemType.CatSkin:
                tmpPos = iconPositions[4];
                break;
            default:
                break;
        }

        iconRect.anchoredPosition = tmpPos;
        itemImage.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBack);
        
        // 判斷是否能使用
        if (item.canUse)
        {
            useButton.SetActive(true);
            useButton.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBack);
        }
        else
        {
            useButton.SetActive(false);
        }
    }

    public void OnTypeChange(object from, object to)
    {
        int value1 = Convert.ToInt32(from);
        int value2 = Convert.ToInt32(to);

        if (value1 != -1)
            showButtons[value1].SetActive(false);
        if (value2 != -1)
            showButtons[value2].SetActive(true);
    }

    public void SetItemFocus(int index)
    {
        for (int i = 0; i < cardBagItems.Count; i++)
            cardBagItems[i].SetFocus(false);

        cardBagItems[index].SetFocus(true);
    }

    private void CheckRedActivate()
    {
        for (int i = 0; i < redPoints.Length; i++)
        {
            if (!redPoints[i].activeSelf)
                continue;
            var tmp = redPoints[i];
            tmp.SetActive(false);
            DOVirtual.DelayedCall(0.35f, () => tmp.gameObject.SetActive(true));
        }
    }
}