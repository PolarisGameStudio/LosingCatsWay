using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class LobbySetDataHelper
{
    private SystemContainer system;
    private ModelContainer model;
    private ControllerContainer controller;
    private FactoryContainer factory;

    public LobbySetDataHelper(MyApplication application)
    {
        system = application.system;
        model = application.model;
        controller = application.controller;
        factory = application.factory;
    }

    public async Task SetData(CloudSaveData cloudSaveData)
    {
        SetPlayerData(cloudSaveData);

        system.grid.Init(); // 生成格子

        await SetFriendData(cloudSaveData);
        SetTimeData(cloudSaveData);
        SetSignData(cloudSaveData);
        SetItemData(cloudSaveData);
        SetMissionData(cloudSaveData);
        SetMailReceivedData(cloudSaveData);
        SetExistRoomData(cloudSaveData);
        SetPurchaseData(cloudSaveData);

        await SetCatData();
    }

    private void SetPlayerData(CloudSaveData cloudSaveData)
    {
        var player = system.player;
        var playerData = cloudSaveData.PlayerData;

        if (playerData.PlayerStatus != null)
            // 避免舊的Dict覆蓋掉擁有新資料的Dict
            for (int i = 0; i < playerData.PlayerStatus.Count; i++)
            {
                string key = playerData.PlayerStatus.ElementAt(i).Key;
                if (player.playerStatus.ContainsKey(key))
                    player.playerStatus[key] = playerData.PlayerStatus[key];
            }

        player.PlayerName = playerData.PlayerName;
        player.PlayerId = playerData.PlayerId;
        player.Level = playerData.Level;
        player.Exp = playerData.Exp;
        // player.Coin = playerData.Coin;
        // player.Diamond = playerData.Diamond;
        // player.CatMemory = playerData.CatMemory;
        player.DiamondCatSlot = playerData.DiamondCatSlot;
        player.GridSizeLevel = playerData.GridSizeLevel;
        player.PlayerGender = playerData.PlayerGender;
        player.UsingIcon = playerData.UsingIcon;
        player.UsingAvatar = playerData.UsingAvatar;
        player.CatDeadCount = playerData.CatDeadCount;

        system.tutorial.directorIndex = playerData.TutorialIndex;
    }

    private async Task SetFriendData(CloudSaveData cloudSaveData)
    {
        model.friend.Friends = await system.cloudSave.LoadFriends();
        model.friend.myInvites = cloudSaveData.FriendData.FriendInvites;
    }

    private void SetTimeData(CloudSaveData cloudSaveData)
    {
        var myTime = system.myTime;
        var timeData = cloudSaveData.TimeData;

        myTime.AccountCreateDateTime = timeData.FirstLoginDateTime.ToDateTime().ToLocalTime();
        myTime.PerDayLoginDateTime = timeData.PerDayLoginDateTime.ToDateTime().ToLocalTime();
        myTime.LastLoginDateTime = timeData.LastLoginDateTime.ToDateTime().ToLocalTime();
    }

    private void SetSignData(CloudSaveData cloudSaveData)
    {
        var monthSign = model.monthSign;
        var signData = cloudSaveData.SignData;

        monthSign.SignIndexs = signData.MonthSigns;
        monthSign.ResignCount = signData.MonthResignCount;
        monthSign.LastMonthSignDate = signData.LastMonthSignDate.ToDateTime().ToLocalTime();
    }

    private void SetItemData(CloudSaveData cloudSaveData)
    {
        // ItemData
        var inventory = system.inventory;
        var itemData = cloudSaveData.ItemData;

        DictSetValueHelper dictSetValueHelper = new DictSetValueHelper();

        dictSetValueHelper.SetDict(itemData.CommonData, inventory.CommonData);
        dictSetValueHelper.SetDict(itemData.RoomData, inventory.RoomData);
        dictSetValueHelper.SetDict(itemData.FoodData, inventory.FoodData);
        dictSetValueHelper.SetDict(itemData.ToolData, inventory.ToolData);
        dictSetValueHelper.SetDict(itemData.LitterData, inventory.LitterData);
        dictSetValueHelper.SetDict(itemData.SkinData, inventory.SkinData);
        dictSetValueHelper.SetDict(itemData.KnowledgeCardDatas, inventory.KnowledgeCardDatas);
        dictSetValueHelper.SetDict(itemData.ItemsCanBuyAtStore, inventory.itemsCanBuyAtStore);
        dictSetValueHelper.SetDict(itemData.PlayerIconData, inventory.PlayerIconData);
        dictSetValueHelper.SetDict(itemData.PlayerAvatarData, inventory.PlayerAvatarData);
    }

    private void SetMissionData(CloudSaveData cloudSaveData)
    {
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
    }

    private void SetMailReceivedData(CloudSaveData cloudSaveData)
    {
        system.mail.mailReceivedDatas = cloudSaveData.MailReceivedDatas;
    }

    private void SetExistRoomData(CloudSaveData cloudSaveData)
    {
        var build = controller.build;
        var existRoomDatas = cloudSaveData.ExistRoomDatas;

        for (int i = 0; i < existRoomDatas.Count; i++)
        {
            var roomData = existRoomDatas[i];
            // if (roomData.Id == factory.roomFactory.originRoomId) continue; //迴避中心房
            build.FirestoreBuild(roomData.Id, roomData.X, roomData.Y);
        }

        if (system.room.MyRooms.Count > 0)
            system.map.GenerateMap();
    }

    private void SetPurchaseData(CloudSaveData cloudSaveData)
    {
        model.mall.PurchaseRecords = cloudSaveData.PurchaseRecords;
    }

    private async Task SetCatData()
    {
        var myCats = await system.cloudSave.LoadCloudCatDatas();
        for (int i = 0; i < myCats.Count; i++)
            system.cat.CreateCatObject(myCats[i]);
    }
}