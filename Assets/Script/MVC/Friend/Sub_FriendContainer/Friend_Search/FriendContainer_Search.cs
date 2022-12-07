using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using Firebase.Auth;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class FriendContainer_Search : ViewBehaviour
{
    [Title("UI")] 
    public TMP_InputField inputField;
    
    [Title("SubUI")] 
    public FriendCard_List friendCard;
    public UIView serchFriendSubView;

    public override void Open()
    {
        base.Open();
    }


    public async void SearchFriend()
    {
        var inputValue = inputField.text;
        
        if (String.IsNullOrEmpty(inputValue))
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.NotAllowBlank);
            return;
        }

        var isPlayerIdExist = await App.system.cloudSave.IsPlayerIdExist(inputValue);
        
        if (!isPlayerIdExist)
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix);
            return;
        }
        
        var myId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        if (inputValue == myId)
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix);
            return;
        }
        
        // 是否已經有好友
        if (App.model.friend.Friends.Exists(x => x.PlayerId.Equals(inputValue)))
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix);
            return;
        }
        
        // 我是否有發Invite

        if (App.model.friend.myInvites.Contains(inputValue))
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix);
            return;
        }
        
        // 對方是否有發Invite
        if (App.model.friend.Invites.Exists(x => x.PlayerId.Equals(inputValue)))
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix);
            return;
        }
        
        OpenSubView();
    }

    // Sub

    public async void SendInvite()
    {
        App.model.friend.myInvites.Add(inputField.text);
        await App.system.cloudSave.UpdateFriendData();
        
        CloseSubView();
    }

    public void CloseSubView()
    {
        inputField.text = String.Empty;
        serchFriendSubView.InstantHide();
    }

    private async void OpenSubView()
    {
        // SetMyPlayerUI
        FriendData friendData = await App.system.cloudSave.LoadFriend(inputField.text);
        friendCard.SetData(friendData);

        serchFriendSubView.Show();
    }
}