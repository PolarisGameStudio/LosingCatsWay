using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Firebase.Auth;
using Firebase.Firestore;
using Lean.Touch;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FriendRoom : MyApplication
{
    private FriendRoom_GridSystem gridSystem;
    private FriendRoom_RoomSystem roomSystem;
    private FriendRoom_CatSystem catSystem;

    [Title("UI")] 
    public UIView mainView;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI catCountText;

    private async void Start()
    {
        gridSystem = GetComponent<FriendRoom_GridSystem>();
        roomSystem = GetComponent<FriendRoom_RoomSystem>();
        catSystem = GetComponent<FriendRoom_CatSystem>();
        
        system.transition.InstantShow();

        // 拿取好友資料
        var friendId = PlayerPrefs.GetString("FriendRoomId");
        CloudSaveData playerData = await LoadFriendData(friendId); // todo Helper設定地板之類的 好了才Transition打開

        // 生地板
        int gridSizeLevel = playerData.PlayerData.GridSizeLevel;

        if (gridSizeLevel > 0)
            FindObjectOfType<LeanPinchCamera>().ClampMax = 15;
        
        gridSystem.Init(gridSizeLevel, 5.12f); // todo 朋友的GridSize
        gridSystem.CreateGrid();

        // 生房間
        roomSystem.CreateRoom(playerData.ExistRoomDatas);
        
        // 生貓
        var cats = await LoadCloudCatDatas(friendId);
        catSystem.CreateCat(cats);

        SetUI(playerData, cats.Count);

        DOVirtual.DelayedCall(1, () =>
        {
            system.transition.OnlyClose();
            system.bgm.Play("Lobby");
        });
    }

    public void Open()
    {
        mainView.Show();
    }

    public void Close()
    {
        mainView.InstantHide();
    }

    public void BackLobby()
    {
        system.confirm.Active(ConfirmTable.Hints_GoHome, () =>
        {
            system.transition.OnlyOpen(() => { SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single); });
        });
    }

    public void ScreenShot()
    {
        system.screenshot.Open();
    }

    private void SetUI(CloudSaveData playerData, int catCount)
    {
        playerNameText.text = playerData.PlayerData.PlayerName;
        playerLevelText.text = "LV." + playerData.PlayerData.Level;
        catCountText.text = catCount + "隻貓";
    }

    private async Task<CloudSaveData> LoadFriendData(string friendId)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Players").Document(friendId);

        DocumentSnapshot result = await docRef.GetSnapshotAsync();
        CloudSaveData cloudSaveData = result.ConvertTo<CloudSaveData>();

        return cloudSaveData;
    }

    private async Task<List<CloudCatData>> LoadCloudCatDatas(string friendId)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference catsRef = db.Collection("Cats");
        Query query = catsRef.WhereEqualTo("CatData.Owner", friendId);
        var documentSnapshots = await query.GetSnapshotAsync();
        List<CloudCatData> result = new List<CloudCatData>();
        foreach (var doc in documentSnapshots.Documents)
        {
            result.Add(doc.ConvertTo<CloudCatData>());
        }

        return result;
    }

    private void OnApplicationQuit()
    {
        // 離開遊戲要拔掉
        PlayerPrefs.DeleteKey("FriendRoomId");
    }
}