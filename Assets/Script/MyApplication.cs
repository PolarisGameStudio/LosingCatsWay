using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class MyApplication : MonoBehaviour
{
    public ModelContainer model;
    public ControllerContainer controller;
    public ViewContainer view;
    public FactoryContainer factory;
    public SystemContainer system;

    private bool canSave;

    private async void Start()
    {
        Vibration.Init();
        
        controller.lobby.Close();
        system.transition.InstantShow();

        canSave = false;

        system.tnr.Init(); // 觸發ValueChange

        // 讀取資料
        bool isCloudSaveDataExist = await system.cloudSave.IsCloudSaveDataExist();
        CloudSaveData cloudSaveData = null;

        if (isCloudSaveDataExist)
            cloudSaveData = await system.cloudSave.LoadCloudSaveData();
        else
            cloudSaveData = await system.cloudSave.CreateCloudSaveData();

        LobbySetDataHelper lobbySetDataHelper = new LobbySetDataHelper(this);
        await lobbySetDataHelper.SetData(cloudSaveData);

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
        controller.dailyQuest.Init(); //todo 把迴廊改成進入條件式，如果是OpenFlow呼叫就日記返回不打開迴廊，並且OnClose加入NextAction，移除Entrance原本的NextAction
        controller.entrance.Init();
        
        #endregion
        
        system.bgm.Init();
        system.soundEffect.Init();

        controller.settings.Init(); //BGM SE 之後
        
        FindObjectOfType<LoadScene>()?.Close();

        canSave = true;

        system.transition.OnlyClose();

        if (backStatus != 2)
            system.openFlow.Init();

        // MyTime 一定要放最後
        system.myTime.Init();
        
        // myTime 弄死貓之後再叫
        controller.cloister.Init();
        
        await system.mail.Init();
        
        system.cat.CheckAngelCat();
        
        DOVirtual.DelayedCall(0.35f, controller.lobby.Open);
    }
    
    #region ApplicationProcess

    private void OnApplicationFocus(bool focus)
    {
        if (!canSave) return;

        if (!focus)
            SaveData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (!canSave) return;

        if (pause)
            SaveData();
    }

    private void OnApplicationQuit()
    {
        if (!canSave) return;
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