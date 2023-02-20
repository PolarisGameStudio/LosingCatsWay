using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FriendCard_List : MvcBehaviour
{
    [SerializeField] private Button selectFriendButton;

    [SerializeField] private TextMeshProUGUI playerNameText;

    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private TextMeshProUGUI selectedLevelText;

    [SerializeField] private TextMeshProUGUI playerIdText;
    [SerializeField] private TextMeshProUGUI selectedIdText;

    [SerializeField] private GameObject selectedObject;

    [SerializeField] private Button goFriendHomeButton;

    [SerializeField] private Image avatarImage;
    [SerializeField] private Image iconImage;

    
    public void SetData(FriendData friendData, UnityAction selectFriendAction = null,
        UnityAction goFriendHomeAction = null)
    {
        playerNameText.text = friendData.PlayerName;

        playerLevelText.text = "LV." + friendData.Level;
        selectedLevelText.text = "LV." + friendData.Level;

        playerIdText.text = "ID:" + friendData.PlayerId;
        selectedIdText.text = "ID:" + friendData.PlayerId;
        
        iconImage.sprite = App.factory.itemFactory.GetItem(friendData.UsingIcon).icon;
        avatarImage.sprite = App.factory.itemFactory.GetItem(friendData.UsingAvatar).icon;

        if (App.factory.itemFactory.avatarEffects.ContainsKey(friendData.UsingAvatar))
        {
            GameObject effectObject = App.factory.itemFactory.avatarEffects[friendData.UsingAvatar];
            Instantiate(effectObject, avatarImage.transform);
        }
        
        if (selectFriendAction != null)
            selectFriendButton.onClick.AddListener(selectFriendAction);

        if (goFriendHomeAction == null)
            return;

        goFriendHomeButton.onClick.AddListener(goFriendHomeAction);
    }

    public void SetSelect(bool value)
    {
        selectedObject.SetActive(value);
    }
}