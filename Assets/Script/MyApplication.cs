using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Firebase.Auth;
using Firebase.Firestore;
using Lean.Common;
using Lean.Touch;
using Sirenix.OdinInspector;
using UnityEngine;

public class MyApplication : MonoBehaviour
{
    public ModelContainer model;
    public ControllerContainer controller;
    public ViewContainer view;
    public FactoryContainer factory;
    public SystemContainer system;

    public LeanPlane leanPlane;
    public LeanPlane globalLeanPlane;
    
    private bool _canSave;

    [Button]
    public void Week()
    {
        print(system.myTime.GetWeekOfYear(DateTime.Now));
    }

    private async void Start()
    {
        Vibration.Init();

        controller.lobby.Close();
        system.transition.InstantShow();

        _canSave = false;

        system.tnr.Init(); // 觸發ValueChange

        // 讀取資料
        bool isCloudSaveDataExist = await system.cloudSave.IsCloudSaveDataExist();
        
        CloudSaveData cloudSaveData = await system.cloudSave.LoadCloudSaveData();
        PlayerDataHelper playerDataHelper = new PlayerDataHelper(this);
        await playerDataHelper.SetData(cloudSaveData);

        if (!isCloudSaveDataExist)// 第一次要先存
        {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference docRef = db.Collection("Players").Document(FirebaseAuth.DefaultInstance.CurrentUser.UserId);

            CloudSaveData tmp = new CloudSaveData();
            await docRef.SetAsync(tmp.ToDict());
        } 
        
        int backStatus = system.myTime.BackLobbyStatus();

        if (backStatus == 2)
            PlayerPrefs.DeleteKey("FriendRoomId");

        // 初始化系統
        system.player.Init(); // ValueChange
        controller.lobby.Init();
        controller.build.Init(); // 中心房要排序在myRooms的第0個
        controller.map.Init(); // 要高於貓咪判斷死亡時
        system.cat.Init();
        controller.pedia.Init();
        controller.shelter.Init();

        await system.post.Init();

        #region 啓動畫面順序

        system.choosePlayerGenderSystem.Init();
        system.tutorial.Init();
        controller.events.Init();
        controller.monthSign.Init();
        controller.entrance.Init();
        controller.dailyQuest.Init();

        #endregion

        system.bgm.Init();
        system.soundEffect.Init();

        controller.bag.Init();
        controller.settings.Init(); //BGM SE 之後
        controller.levelReward.Init();
        
        FindObjectOfType<LoadScene>()?.Close();

        _canSave = true;

        system.transition.OnlyClose();

        if (backStatus != 2)
            system.openFlow.Init();
        else
            system.openFlow.EndAction();

        system.settle.Init();
        await controller.cloister.Init();

        // MyTime 一定要放最後
        system.myTime.Init();
        controller.greenHouse.Init();
        
        controller.mall.Init();
        await system.mail.Init();
        
        system.cat.CheckAngelCat();
        system.unlockGrid.Init(); // 一定要在myGrid拿到sensor後

        int gridSizeLevel = system.player.GridSizeLevel;

        if (gridSizeLevel > 1)
            FindObjectOfType<LeanPinchCamera>().ClampMax = 15;
        
        DOVirtual.DelayedCall(0.35f, controller.lobby.Open);
    }

    #region ApplicationProcess

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveDataSync();
    }

    private void OnApplicationQuit()
    {
        SaveDataSync();
    }

    public void SaveData()
    {
        if (!_canSave)
            return;
        if (system.tutorial.isTutorial)
            return;
        
        controller.settings.SaveSettings();
        system.myTime.SetDateTime();

        system.cloudSave.SaveCloudSaveData();
        system.cloudSave.SaveCloudCatDatas();
    }

    private void SaveDataSync()
    {
        if (!_canSave)
            return;
        if (system.tutorial.isTutorial)
            return;
        
        controller.settings.SaveSettings();
        system.myTime.SetDateTime();

        system.cloudSave.SaveCloudSaveDataSync();
        system.cloudSave.SaveCloudCatDatasSync();
    }

    #endregion
}