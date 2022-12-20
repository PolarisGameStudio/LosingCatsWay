using System;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_Bag : ViewBehaviour
{
    [Title("View")] [SerializeField] private View_BagChooseCat chooseCat;
    
    [Title("Tab")]
    [SerializeField] private GameObject[] showButtons;
    [SerializeField] private GameObject item;
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

    private List<Card_BagItem> cardBagItems = new List<Card_BagItem>();

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
        cardBagItems.Clear();

        List<Item> items = (List<Item>) value;

        for (int i = 0; i < content.childCount; i++)
        {
            GameObject tmp = content.GetChild(i).gameObject;
            tmp.transform.DOKill();
            Destroy(tmp);
        }

        for (int i = 0; i < items.Count; i++)
        {
            GameObject tmp = Instantiate(item, content);
            Card_BagItem card = tmp.GetComponent<Card_BagItem>();
            cardBagItems.Add(card);

            card.SetData(items[i]);

            Button button = tmp.GetComponent<Button>();

            var i1 = i;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { App.controller.bag.ChooseItem(i1); });

            tmp.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutExpo).SetDelay(i * 0.0625f);
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
            return;
        }

        Item item = (Item) value;

        itemName.text = item.Name;
        itemCount.text = item.Count.ToString();
        itemDescription.text = item.Description;

        itemImage.enabled = true;
        itemImage.sprite = item.icon;

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

        showButtons[value2].SetActive(true);
    }

    public void SetItemFocus(int index)
    {
        for (int i = 0; i < cardBagItems.Count; i++)
            cardBagItems[i].SetFocus(false);

        cardBagItems[index].SetFocus(true);
    }

    public void OpenChooseCat()
    {
        chooseCat.Open();
    }

    public void CloseChooseCat()
    {
        chooseCat.Close();
    }
}