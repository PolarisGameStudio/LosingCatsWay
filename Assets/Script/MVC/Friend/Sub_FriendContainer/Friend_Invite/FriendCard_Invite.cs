using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FriendCard_Invite : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerLevelText;
    [SerializeField] private TextMeshProUGUI playerIdText;

    [SerializeField] private Button acceptButton; 
    [SerializeField] private Button rejectButton; 
    
    public void SetData(FriendData friendData, UnityAction acceptAction, UnityAction rejectAction)
    {
        playerNameText.text = friendData.PlayerName;
        playerLevelText.text = "LV." + friendData.Level;
        playerIdText.text = "ID:" + friendData.PlayerId;
        
        acceptButton.onClick.AddListener(acceptAction);
        rejectButton.onClick.AddListener(rejectAction);
    }
}
