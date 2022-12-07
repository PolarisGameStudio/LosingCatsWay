using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FriendCard_List : MonoBehaviour
{
    [SerializeField] private Button selectFriendButton;

    [SerializeField] private TextMeshProUGUI playerNameText;

    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private TextMeshProUGUI selectedLevelText;

    [SerializeField] private TextMeshProUGUI playerIdText;
    [SerializeField] private TextMeshProUGUI selectedIdText;

    [SerializeField] private GameObject selectedObject;

    [SerializeField] private Button goFriendHomeButton;

    public void SetData(FriendData friendData, UnityAction selectFriendAction = null,
        UnityAction goFriendHomeAction = null)
    {
        playerNameText.text = friendData.PlayerName;

        playerLevelText.text = "LV." + friendData.Level;
        selectedLevelText.text = "LV." + friendData.Level;

        playerIdText.text = "ID:" + friendData.PlayerId;
        selectedIdText.text = "ID:" + friendData.PlayerId;

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