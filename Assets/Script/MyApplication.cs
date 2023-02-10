using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Firebase.Auth;
using Firebase.Firestore;
using Lean.Common;
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
    
    private bool canSave;

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

        canSave = false;

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
        controller.build.Init(); // 中心房要排序在myRooms的第0個
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
        controller.friend.Init();
        
        FindObjectOfType<LoadScene>()?.Close();

        canSave = true;

        system.transition.OnlyClose();

        if (backStatus != 2)
            system.openFlow.Init();

        system.settle.Init();

        // MyTime 一定要放最後
        system.myTime.Init();

        // myTime 弄死貓之後再叫
        await controller.cloister.Init();
        controller.greenHouse.Init();
        
        controller.mall.Init();
        await system.mail.Init();
        
        system.cat.CheckAngelCat();

        DOVirtual.DelayedCall(0.35f, controller.lobby.Open);
    }

    #region ApplicationProcess

    private void OnApplicationFocus(bool focus)
    {
        if (!canSave)
            return;
        if (!focus)
            SaveData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (!canSave)
            return;
        if (pause)
            SaveData();
    }

    private void OnApplicationQuit()
    {
        if (!canSave)
            return;
        SaveData();
    }

    private void SaveData()
    {
        controller.settings.SaveSettings();
        controller.cultive.SaveCleanLitterData();

        system.myTime.SetDateTime();
        system.cloudSave.SaveCloudSaveData();
    }

    #endregion
}