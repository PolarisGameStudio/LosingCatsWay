using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Doozy.Runtime.Common.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class Controller_Friend : ControllerBehavior
{
    public async void Init()
    {
        await RefreshData();
    }

    public async void Open()
    {
        await RefreshData();

        // SetMyPlayerUI
        FriendData friendData = new FriendData();

        friendData.PlayerId = App.system.player.PlayerId;
        friendData.PlayerName = App.system.player.PlayerName;
        friendData.Level = App.system.player.Level;
        friendData.UsingIcon = App.system.player.UsingIcon;
        friendData.UsingAvatar = App.system.player.UsingAvatar;

        App.view.friend.myPlayerCard.SetData(friendData);
        
        int favouriteCatIndex = App.system.cat.GetFavoriteCatIndex();
        if (favouriteCatIndex == -1)
            App.model.friend.NowFavouriteCat = null;
        else
            App.model.friend.NowFavouriteCat = App.system.cat.GetCats()[favouriteCatIndex];

        App.view.friend.Open();
        Select(0);
        SelectFriend(-1);
    }

    public void Close()
    {
        App.view.friend.Close();
    }

    public void CloseByOpenLobby()
    {
        Close();
        App.controller.lobby.Open();
    }

    public async Task RefreshData()
    {
        App.model.friend.Friends = await App.system.cloudSave.LoadFriends();
        App.model.friend.Invites = await App.system.cloudSave.LoadInvites();
    }

    public void Select(int index)
    {
        App.model.friend.SelectedContainer = index;

        if (index != 0)
            SelectFriend(-1);
    }

    public void CopyID()
    {
        string id = App.system.player.PlayerId;
        id.CopyToClipboard();
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.Copied);
    }

    public void SelectFriend(int index)
    {
        if (App.model.friend.SelectedFriendIndex == index)
            return;

        App.model.friend.SelectedFriendIndex = index;
    }

    #region ChooseCat

    public void OpenChooseCat()
    {
        App.model.friend.MyCats = App.system.cat.GetCats();
        App.view.friend.OpenChooseCat();

        if (App.model.friend.MyCats.Count <= 0)
        {
            ChooseCat(-1);
            return;
        }
        
        var favoriteCatIndex = App.system.cat.GetFavoriteCatIndex();
        
        if (favoriteCatIndex == -1)
            ChooseCat(0);
        else
            ChooseCat(favoriteCatIndex);

    }

    public void ChooseCat(int index)
    {
        App.model.friend.SelectedMyFavoriteCatIndex = index;
    }

    public void ChooseCatOk()
    {
        int index = App.model.friend.SelectedMyFavoriteCatIndex;
        
        var nowFavoriteCat = App.system.cat.GetCats()[index];
        App.model.friend.NowFavouriteCat = nowFavoriteCat;

        // 關掉原本的
        int prevFavoriteIndex = App.system.cat.GetFavoriteCatIndex();

        if (prevFavoriteIndex != -1)
        {
            var prevFavoriteCat = App.system.cat.GetCats()[prevFavoriteIndex];
            prevFavoriteCat.cloudCatData.CatData.IsFavorite = false;
            App.system.cloudSave.UpdateCloudCatData(prevFavoriteCat.cloudCatData);
        }

        // 改現在的
        nowFavoriteCat.cloudCatData.CatData.IsFavorite = true;
        App.system.cloudSave.UpdateCloudCatData(nowFavoriteCat.cloudCatData);

        App.view.friend.CloseChooseCat();

        if (App.model.friend.SelectedFriendIndex == -1)
        {
            App.view.friend.selectedCatSkin.gameObject.SetActive(true);
            App.view.friend.selectedCatSkin.ChangeSkin(nowFavoriteCat.cloudCatData);
        }
    }

    #endregion
}