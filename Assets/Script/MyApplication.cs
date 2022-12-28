using System.Threading.Tasks;
using System.Collections.Generic;
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

        system.tnr.Init(); // Init(ValueChange) -> SetData (Diamond Coin) //TODO App.system.player.Diamond = App.system.player.Diamond 就好
        system.grid.Init(); // 生成格子 // TODO 讀玩家擁有格生格子
        // 初始化系統
        controller.build.Init(); // 中心房要排序在myRooms的第0個

        // 讀取資料
        bool isCloudSaveDataExist = await system.cloudSave.IsCloudSaveDataExist();
        CloudSaveData cloudSaveData = null;

        if (isCloudSaveDataExist)
            cloudSaveData = await system.cloudSave.LoadCloudSaveData();
        else
            cloudSaveData = await system.cloudSave.CreateCloudSaveData();

        await SetData(cloudSaveData);

        int backStatus = system.myTime.BackLobbyStatus();

        if (backStatus == 2)
            PlayerPrefs.DeleteKey("FriendRoomId");
        
        system.cat.Init();
        controller.dailyQuest.Init();
        controller.pedia.Init();

        // 啓動的流程順序(OpenFlow.Init)
        if (system.tutorial.directorIndex < 0)
        {
            system.openFlow.AddAction(system.choosePlayerGenderSystem.Init);
            system.openFlow.AddAction(system.tutorial.Init);
            system.openFlow.AddAction(controller.events.Init);
            system.openFlow.AddAction(controller.monthSign.Init);
            system.openFlow.AddAction(controller.entrance.Init);
        }
        else
        {
            system.openFlow.AddAction(controller.events.Init);
            system.openFlow.AddAction(controller.monthSign.Init);
            system.openFlow.AddAction(controller.entrance.Init);
        }

        system.bgm.Init();
        system.soundEffect.Init();
        // system.quest.Init();

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
        
        DOVirtual.DelayedCall(0.35f, controller.lobby.Open);
    }

    private async Task SetData(CloudSaveData cloudSaveData)
    {
        // Player
        var player = system.player;
        var playerData = cloudSaveData.PlayerData;

        player.PlayerName = playerData.PlayerName;
        player.PlayerId = playerData.PlayerId;
        player.Level = playerData.Level;
        player.Exp = playerData.Exp;
        player.Coin = playerData.Coin;
        player.Diamond = playerData.Diamond;
        player.DiamondCatSlot = playerData.DiamondCatSlot;
        player.GridSize = playerData.GridSize;
        player.PlayerGender = playerData.PlayerGender;
        player.UsingIcon = playerData.UsingIcon;
        player.UsingAvatar = playerData.UsingAvatar;

        system.tutorial.directorIndex = playerData.TutorialIndex;

        // FriendData
        model.friend.Friends = await system.cloudSave.LoadFriends();
        model.friend.myInvites = cloudSaveData.FriendData.FriendInvites;

        // Time
        var myTime = system.myTime;
        var timeData = cloudSaveData.TimeData;
        
        myTime.FirstLoginDateTime = timeData.FirstLoginDateTime.ToDateTime().ToLocalTime();
        myTime.PerDayLoginDateTime = timeData.PerDayLoginDateTime.ToDateTime().ToLocalTime();
        myTime.LastLoginDateTime = timeData.LastLoginDateTime.ToDateTime().ToLocalTime();

        // SignData
        var monthSign = model.monthSign;
        var signData = cloudSaveData.SignData;

        monthSign.SignIndexs = signData.MonthSigns;
        monthSign.ResignCount = signData.MonthResignCount;
        monthSign.LastMonthSignDate = signData.LastMonthSignDate.ToDateTime().ToLocalTime();

        // ItemData
        var inventory = system.inventory;
        var itemData = cloudSaveData.ItemData;

        inventory.RoomData = itemData.RoomData;
        inventory.FoodData = itemData.FoodData;
        inventory.ToolData = itemData.ToolData;
        inventory.LitterData = itemData.LitterData;
        inventory.SkinData = itemData.SkinData;
        inventory.itemsCanBuyAtStore = itemData.ItemsCanBuyAtStore;
        inventory.PlayerIconData = itemData.PlayerIconData;
        inventory.PlayerAvatarData = itemData.PlayerAvatarData;

        // MissionDatas
        var quest = system.quest;
        var missionData = cloudSaveData.MissionData;

        quest.QuestProgressData = missionData.QuestProgressData;
        quest.QuestReceivedStatusData = missionData.QuestReceivedStatusData;
        model.dailyQuest.Quests = new List<Quest>();
        for (int i = 0; i < missionData.MyQuests.Count; i++)
        {
            Quest q = factory.questFactory.GetQuestById(missionData.MyQuests[i]);
            model.dailyQuest.Quests.Add(q);
        }
        
        // MailReceivedDatas
        system.mail.mailReceivedDatas = cloudSaveData.MailReceivedDatas;
        
        // ExistRoomDatas
        var build = controller.build;
        var existRoomDatas = cloudSaveData.ExistRoomDatas;

        for (int i = 0; i < existRoomDatas.Count; i++)
        {
            var roomData = existRoomDatas[i];
        
            if (roomData.Id == factory.roomFactory.originRoomId) continue; //迴避中心房
            build.FirestoreBuild(roomData.Id, roomData.X, roomData.Y);
        }

        system.map.GenerateMap();
        
        // Purchase
        model.mall.PurchaseRecords = cloudSaveData.PurchaseRecords;

        // Cats
        var myCats = await system.cloudSave.LoadCloudCatDatas();
        
        for (int i = 0; i < myCats.Count; i++)
            system.cat.CreateCatObject(myCats[i]);
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
        controller.cultive.LocalSaveCultiveLitter();

        system.myTime.SetDateTime();
        system.cloudSave.SaveCloudSaveData();
    }

    #endregion
}