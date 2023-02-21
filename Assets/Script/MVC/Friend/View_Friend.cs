using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using System;
using Doozy.Runtime.UIManager.Containers;

public class View_Friend : ViewBehaviour
{
    [Title("Tabs")] public Button[] tabButtons;
    public GameObject[] tabMasks;

    [Title("Container")] public ViewBehaviour[] containers;

    [Space(10)] [Title("Copy")] public TextMeshProUGUI[] copyIdTexts;

    [Title("Nav")] [SerializeField] private TextMeshProUGUI coinText;

    [SerializeField] private TextMeshProUGUI diamondText;

    [Title("Card")] public FriendCard_List myPlayerCard;

    [Title("ButtonGroup")] public GameObject buttonGroup;

    [Title("SelectedCat")] public CatSkin selectedCatSkin;

    [Title("ChooseCat")] 
    public UIView chooseCatView;
    public Item_ChooseCat[] chooseCatObjects;
    [SerializeField] private GameObject chooseCatButton;

    [Title("Favourite")] [SerializeField] private TextMeshProUGUI favouriteCatNameText;
    [SerializeField] private TextMeshProUGUI favouriteVarietyText;
    [SerializeField] private TextMeshProUGUI favouriteGenderText;
    [SerializeField] private TextMeshProUGUI favouriteAgeText;

    public override void Init()
    {
        base.Init();

        // todo 好友
        // App.system.player.onPlayerFriendsChange += OnPlayerFriendsChange;
        // App.system.player.onReceiveFriendInvitesChange += OnReceiveFriendInvitesChange;
        App.system.player.OnPlayerIdChange += OnPlayerIdChange;
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;

        App.model.friend.OnSelectedContainerChange += OnSelectedContainerChange;
        App.model.friend.OnMyCatsChange += OnMyCatsChange;
        App.model.friend.OnSelectedFriendIndexChange += OnSelectedFriendIndexChange;
        App.model.friend.OnSelectedMyFavoriteCatIndexChange += OnSelectedMyFavoriteCatIndexChange;
        App.model.friend.OnNowFavouriteCatChange += OnNowFavouriteCatChange;
    }

    private void OnNowFavouriteCatChange(object value)
    {
        if (value == null)
        {
            favouriteCatNameText.text = string.Empty;
            favouriteVarietyText.text = "-";
            favouriteGenderText.text = "-";
            favouriteAgeText.text = "-";
            return;
        }
        
        Cat cat = (Cat)value;
        favouriteCatNameText.text = cat.cloudCatData.CatData.CatName;
        favouriteVarietyText.text = cat.cloudCatData.CatData.SurviveDays <= 3
            ? App.factory.stringFactory.GetKittyName()
            : App.factory.stringFactory.GetCatVariety(cat.cloudCatData.CatData.Variety);
        favouriteGenderText.text = (cat.cloudCatData.CatData.Sex == 0) ? App.factory.stringFactory.GetBoyString() : App.factory.stringFactory.GetGirlString();
        favouriteAgeText.text = cat.cloudCatData.CatData.CatAge.ToString();
    }

    public void OnSelectedContainerChange(object value)
    {
        int index = (int)value;

        for (int i = 0; i < containers.Length; i++)
        {
            tabMasks[i].SetActive(false);
            tabButtons[i].gameObject.SetActive(true);

            containers[i].Close();
        }

        if (index == 0)
            buttonGroup.SetActive(true);
        else
            buttonGroup.SetActive(false);

        tabMasks[index].SetActive(true);
        tabButtons[index].gameObject.SetActive(false);

        containers[index].Open();
    }

    public void OpenChooseCat()
    {
        chooseCatView.Show();
    }

    public void CloseChooseCat()
    {
        chooseCatView.InstantHide();
    }

    private void OnPlayerIdChange(object value)
    {
        string result = value.ToString();

        for (int i = 0; i < copyIdTexts.Length; i++)
        {
            copyIdTexts[i].text = result;
        }
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

    private void OnMyCatsChange(object value)
    {
        var cats = (List<Cat>)value;
        int catsCount = cats.Count;
        
        chooseCatButton.SetActive(catsCount > 0);

        for (int i = 1; i <= chooseCatObjects.Length; i++)
        {
            if (i > catsCount)
            {
                chooseCatObjects[i - 1].gameObject.SetActive(false);
                continue;
            }

            var itemChooseCat = chooseCatObjects[i - 1];

            itemChooseCat.gameObject.SetActive(true);
            itemChooseCat.SetData(cats[i - 1]);
        }
    }

    private async void OnSelectedFriendIndexChange(object value)
    {
        int index = (int)value;
        
        CloudCatData cloudCatData = null;

        if (index == -1) // 自己的愛貓
        {
            int myFavoriteCatIndex = App.system.cat.GetFavoriteCatIndex();
            
            if (myFavoriteCatIndex != -1)
                cloudCatData = App.system.cat.GetCats()[myFavoriteCatIndex].cloudCatData;
        }
        else // 好友的
        {
            var friendId = App.model.friend.Friends[index].PlayerId;
            cloudCatData = await App.system.cloudSave.GetFriendFavoriteCat(friendId);
        }

        if (cloudCatData == null)
        {
            selectedCatSkin.gameObject.SetActive(false);
            return;
        }
            
        selectedCatSkin.gameObject.SetActive(true);
        selectedCatSkin.ChangeSkin(cloudCatData);
    }

    private void OnSelectedMyFavoriteCatIndexChange(object value)
    {
        int selectIndex = (int)value;

        if (selectIndex == -1)
            return;
        
        for (int i = 0; i < chooseCatObjects.Length; i++)
            chooseCatObjects[i].SetSelect(false);
        
        chooseCatObjects[selectIndex].SetSelect(true);
    }
}