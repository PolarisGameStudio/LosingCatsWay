using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class FriendContainer_Invite : ViewBehaviour
{
    [Title("Card")] public GameObject card;

    [Title("UI")] public Transform content;

    public override void Init()
    {
        base.Init();

        App.model.friend.OnInvitesChange += OnInvitesChange;
    }

    private void OnInvitesChange(object value)
    {
        List<FriendData> invites = (List<FriendData>)value;

        int contentCount = content.childCount;

        for (int i = 0; i < contentCount; i++)
            Destroy(content.GetChild(i).gameObject);

        for (int i = 0; i < invites.Count; i++)
        {
            var index = i;

            FriendCard_Invite friendCardInvite = Instantiate(card, content).GetComponent<FriendCard_Invite>();
            friendCardInvite.SetData(invites[i], () => { Accept(index); }, () => { Reject(index); });
        }
    }

    private async void Accept(int index)
    {
        var friend = App.model.friend.Invites[index];

        // 加好友並刪除
        await App.system.cloudSave.AddFriendAndDeleteInvite(friend.PlayerId);
        
        // 加入我的好友
        App.model.friend.Friends.Add(friend);
        await App.system.cloudSave.UpdateFriendData();

        await App.controller.friend.RefreshData();
    }

    private async void Reject(int index)
    {
        var friend = App.model.friend.Invites[index];
        
        // 刪除好友邀請
        await App.system.cloudSave.DeleteInvite(friend.PlayerId);
        await App.controller.friend.RefreshData();
    }
}