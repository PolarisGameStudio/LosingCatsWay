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

public class FriendRoom : MonoBehaviour
{
    private FriendRoom_GridSystem gridSystem;
    private FriendRoom_RoomSystem roomSystem;
    private FriendRoom_CatSystem catSystem;
    private TransitionsSystem transitionsSystem;

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
        transitionsSystem = FindObjectOfType<TransitionsSystem>();

        transitionsSystem.InstantShow();

        // 拿取好友資料
        var friendId = PlayerPrefs.GetString("FriendRoomId");
        CloudSaveData playerData = await LoadFriendData(friendId);

        // 生地板
        gridSystem.Init(31, 31, 5.12f);
        gridSystem.CreateGrid();

        // 生房間
        roomSystem.CreateRoom(playerData.ExistRoomDatas);

        // 生貓
        var cats = await LoadCloudCatDatas(friendId);
        catSystem.CreateCat(cats);

        SetUI(playerData, cats.Count);

        DOVirtual.DelayedCall(0.5f, () => { transitionsSystem.OnlyClose(); });
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
        transitionsSystem.OnlyOpen(() => { SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single); });
    }

    public void ScreenShot()
    {
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