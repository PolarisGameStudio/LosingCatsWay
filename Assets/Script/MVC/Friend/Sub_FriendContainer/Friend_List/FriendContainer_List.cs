using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FriendContainer_List : ViewBehaviour
{
    [Title("Card")] public GameObject card;

    [Title("UI")] public Transform content;
    [SerializeField] private TextMeshProUGUI friendCountText; //(N/99)

    private List<FriendCard_List> friendCardLists = new List<FriendCard_List>();
    
    public override void Init()
    {
        base.Init();

        App.model.friend.OnFriendsChange += OnFriendsChange;
        App.model.friend.OnSelectedFriendIndexChange += OnSelectedFriendIndexChange;
    }

    public void GoFriendHome(int index)
    {
        App.system.confirm.Active(ConfirmTable.Hints_GoFriendsHome, () =>
        {
            App.SaveData();
            
            App.system.transition.OnlyOpen(() =>
            {
                PlayerPrefs.SetString("FriendRoomId", App.model.friend.Friends[index].PlayerId);
                SceneManager.LoadSceneAsync("FriendRoom", LoadSceneMode.Single);
            });
        });
    }

    private void OnFriendsChange(object value)
    {
        List<FriendData> friends = (List<FriendData>)value;

        int contentCount = content.childCount;

        friendCountText.text = $"({contentCount}/99)";

        for (int i = 0; i < contentCount; i++)
            Destroy(content.GetChild(i).gameObject);

        friendCardLists.Clear();
        
        for (int i = 0; i < friends.Count; i++)
        {
            var index = i;
            
            FriendCard_List friendCardList = Instantiate(card, content).GetComponent<FriendCard_List>();
            friendCardList.SetData(friends[i], () => { App.controller.friend.SelectFriend(index); },
                () => { GoFriendHome(index); });
            
            friendCardLists.Add(friendCardList);
        }
    }

    private void OnSelectedFriendIndexChange(object value)
    {
        int index = (int)value;

        for (int i = 0; i < friendCardLists.Count; i++)
            friendCardLists[i].SetSelect(false);

        if (index != -1)
            friendCardLists[index].SetSelect(true);
    }
}