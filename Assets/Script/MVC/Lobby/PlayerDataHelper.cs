using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

public class PlayerDataHelper
{
    private MyApplication app;

    public PlayerDataHelper(MyApplication application)
    {
        app = application;
    }

    #region Set

    public async Task SetData(CloudSaveData cloudSaveData)
    {
        if (cloudSaveData == null)
            cloudSaveData = new CloudSaveData();

        SetPlayerData(cloudSaveData);

        app.system.grid.Init(); // 生成格子

        await SetFriendData(cloudSaveData);
        SetTimeData(cloudSaveData);
        SetSignData(cloudSaveData);
        SetItemData(cloudSaveData);
        SetMissionData(cloudSaveData);
        SetMailReceivedData(cloudSaveData);
        SetExistRoomData(cloudSaveData);
        SetPurchaseData(cloudSaveData);
        SetGreenHouseDatas(cloudSaveData);

        await SetCatData();
    }

    private void SetPlayerData(CloudSaveData cloudSaveData)
    {
        var player = app.system.player;
        var playerData = cloudSaveData.PlayerData;

        player.PlayerName = String.IsNullOrEmpty(playerData.PlayerName) ? "-" : playerData.PlayerName;
        player.PlayerId = String.IsNullOrEmpty(playerData.PlayerName)
            ? CloudSaveExtension.CurrentUserId
            : playerData.PlayerId;
        player.Level = playerData.Level == 0 ? 1 : playerData.Level;
        player.Exp = playerData.Exp;
        player.DiamondCatSlot = playerData.DiamondCatSlot;
        player.GridSizeLevel = playerData.GridSizeLevel == 0 ? 1 : playerData.GridSizeLevel;
        player.PlayerGender = playerData.StartTutorialEnd == false ? -1 : playerData.PlayerGender;
        player.UsingIcon = string.IsNullOrEmpty(playerData.UsingIcon) ? string.Empty : playerData.UsingIcon;
        player.UsingAvatar = string.IsNullOrEmpty(playerData.UsingAvatar) ? "PAT001" : playerData.UsingAvatar;
        player.CatDeadCount = playerData.CatDeadCount;

        app.system.tutorial.startTutorialEnd = playerData.StartTutorialEnd;
        app.system.tutorial.shelterTutorialEnd = playerData.ShelterTutorialEnd;
    }

    private async Task SetFriendData(CloudSaveData cloudSaveData)
    {
        if (cloudSaveData.FriendData.FriendIds == null)
            cloudSaveData.FriendData.FriendIds = new List<string>();

        if (cloudSaveData.FriendData.FriendInvites == null)
            cloudSaveData.FriendData.FriendInvites = new List<string>();

        app.model.friend.Friends = await app.system.cloudSave.LoadFriends();
        app.model.friend.myInvites = cloudSaveData.FriendData.FriendInvites;
    }

    private void SetTimeData(CloudSaveData cloudSaveData)
    {
        var myTime = app.system.myTime;
        var timeData = cloudSaveData.TimeData;

        Timestamp nowTime = Timestamp.GetCurrentTimestamp();

        if (timeData.FirstLoginDateTime == new Timestamp())
            timeData.FirstLoginDateTime = nowTime;

        if (timeData.LastLoginDateTime == new Timestamp())
            timeData.LastLoginDateTime = nowTime;

        myTime.AccountCreateDateTime = timeData.FirstLoginDateTime.ToDateTime().ToLocalTime();
        myTime.PerDayLoginDateTime = timeData.PerDayLoginDateTime.ToDateTime().ToLocalTime();
        myTime.LastLoginDateTime = timeData.LastLoginDateTime.ToDateTime().ToLocalTime();
    }

    private void SetSignData(CloudSaveData cloudSaveData)
    {
        var monthSign = app.model.monthSign;
        var signData = cloudSaveData.SignData;

        monthSign.SignIndexs = signData.MonthSigns == null ? new List<int>(new int[31]) : signData.MonthSigns;
        monthSign.LastMonthSignDate = signData.LastMonthSignDate.ToDateTime().ToLocalTime();
    }

    private void SetItemData(CloudSaveData cloudSaveData)
    {
        // ItemData
        var inventory = app.system.inventory;
        var itemData = cloudSaveData.ItemData;

        DictSetValueHelper dictSetValueHelper = new DictSetValueHelper();
        
        if (itemData.CommonData == null)
            itemData.CommonData = app.system.inventory.CommonData;
        else
            dictSetValueHelper.SetDict(itemData.CommonData, inventory.CommonData);

        if (itemData.RoomData == null)
            itemData.RoomData = app.system.inventory.RoomData;
        else
            dictSetValueHelper.SetDict(itemData.RoomData, inventory.RoomData);

        if (itemData.FoodData == null)
            itemData.FoodData = app.system.inventory.FoodData;
        else
            dictSetValueHelper.SetDict(itemData.FoodData, inventory.FoodData);

        if (itemData.ToolData == null)
            itemData.ToolData = app.system.inventory.ToolData;
        else
            dictSetValueHelper.SetDict(itemData.ToolData, inventory.ToolData);

        if (itemData.LitterData == null)
            itemData.LitterData = app.system.inventory.LitterData;
        else
            dictSetValueHelper.SetDict(itemData.LitterData, inventory.LitterData);

        if (itemData.SkinData == null)
            itemData.SkinData = app.system.inventory.SkinData;
        else
            dictSetValueHelper.SetDict(itemData.SkinData, inventory.SkinData);
        
        if (itemData.PlayerIconData == null)
            itemData.PlayerIconData = app.system.inventory.PlayerIconData;
        else
            dictSetValueHelper.SetDict(itemData.PlayerIconData, inventory.PlayerIconData);

        if (itemData.PlayerAvatarData == null)
            itemData.PlayerAvatarData = app.system.inventory.PlayerAvatarData;
        else
            dictSetValueHelper.SetDict(itemData.PlayerAvatarData, inventory.PlayerAvatarData);
    }

    private void SetMissionData(CloudSaveData cloudSaveData)
    {
        // var quest = app.system.quest;
        var missionData = cloudSaveData.MissionData;

        DictSetValueHelper dictSetValueHelper = new DictSetValueHelper();

        // quest.QuestProgressData = missionData.QuestProgressData == null
        //     ? app.system.quest.QuestProgressData
        //     : missionData.QuestProgressData;
        // quest.QuestReceivedStatusData = missionData.QuestReceivedStatusData == null
        //     ? app.system.quest.QuestReceivedStatusData
        //     : missionData.QuestReceivedStatusData;

        if (missionData.QuestProgressData == null)
            missionData.QuestProgressData = app.system.quest.QuestProgressData;
        else
            dictSetValueHelper.SetDict(missionData.QuestProgressData, app.system.quest.QuestProgressData);
        
        if (missionData.QuestReceivedStatusData == null)
            missionData.QuestReceivedStatusData = app.system.quest.QuestReceivedStatusData;
        else
            dictSetValueHelper.SetDict(missionData.QuestReceivedStatusData, app.system.quest.QuestReceivedStatusData);

        if (missionData.KnowledgeCardData == null)
            missionData.KnowledgeCardData = app.system.quest.KnowledgeCardData;
        else
            dictSetValueHelper.SetDict(missionData.KnowledgeCardData, app.system.quest.KnowledgeCardData);
        
        if (missionData.KnowledgeCardStatus == null)
            missionData.KnowledgeCardStatus = app.system.quest.KnowledgeCardStatus;
        else
            dictSetValueHelper.SetDict(missionData.KnowledgeCardStatus, app.system.quest.KnowledgeCardStatus);

        if (missionData.MyQuests == null)
            missionData.MyQuests = new List<string>();

        app.model.dailyQuest.Quests = new List<Quest>();
        for (int i = 0; i < missionData.MyQuests.Count; i++)
        {
            Quest q = app.factory.questFactory.GetQuestById(missionData.MyQuests[i]);
            app.model.dailyQuest.Quests.Add(q);
        }
    }

    private void SetMailReceivedData(CloudSaveData cloudSaveData)
    {
        app.system.mail.mailReceivedDatas = cloudSaveData.MailReceivedDatas == null
            ? new List<string>()
            : cloudSaveData.MailReceivedDatas;
    }

    private void SetExistRoomData(CloudSaveData cloudSaveData)
    {
        var build = app.controller.build;
        var existRoomDatas = cloudSaveData.ExistRoomDatas == null
            ? new List<CloudSave_RoomData>()
            : cloudSaveData.ExistRoomDatas;

        for (int i = 0; i < existRoomDatas.Count; i++)
        {
            var roomData = existRoomDatas[i];
            build.FirestoreBuild(roomData.Id, roomData.X, roomData.Y);
        }

        if (app.system.room.MyRooms.Count > 0)
            app.system.map.GenerateMap();
    }

    private void SetPurchaseData(CloudSaveData cloudSaveData)
    {
        app.model.mall.PurchaseRecords = cloudSaveData.PurchaseRecords == null
            ? new Dictionary<string, PurchaseRecord>()
            : cloudSaveData.PurchaseRecords;
    }

    private void SetGreenHouseDatas(CloudSaveData cloudSaveData)
    {
        app.model.greenHouse.GreenHouseDatas = cloudSaveData.GreenHouseDatas == null
            ? new List<GreenHouseData>()
            : cloudSaveData.GreenHouseDatas;
    }

    private async Task SetCatData()
    {
        var myCats = await app.system.cloudSave.LoadCloudCatDatas();
        for (int i = 0; i < myCats.Count; i++)
            app.system.cat.CreateCatObject(myCats[i]);
    }

    #endregion

    #region Save

    public Dictionary<string, object> GetSaveData()
    {
        CloudSaveData cloudSaveData = new CloudSaveData();

        CloudSave_PlayerData playerData = GetPlayerData();
        CloudSave_FriendData friendData = GetFriendData();
        CloudSave_TimeData timeData = GetTimeData();
        CloudSave_SignData signData = GetSignData();
        CloudSave_ItemData itemData = GetItemData();
        CloudSave_MissionData missionData = GetMissionData();

        cloudSaveData.PlayerData = playerData;
        cloudSaveData.FriendData = friendData;
        cloudSaveData.TimeData = timeData;
        cloudSaveData.SignData = signData;
        cloudSaveData.ItemData = itemData;
        cloudSaveData.MissionData = missionData;

        cloudSaveData.ExistRoomDatas = GetExistRoomDatas();
        cloudSaveData.PurchaseRecords = app.model.mall.PurchaseRecords;

        cloudSaveData.MailReceivedDatas = app.system.mail.mailReceivedDatas;

        cloudSaveData.GreenHouseDatas = app.model.greenHouse.GreenHouseDatas;

        Dictionary<string, object> result = new Dictionary<string, object>
        {
            { "PlayerData", cloudSaveData.PlayerData },
            { "FriendData", cloudSaveData.FriendData },
            { "TimeData", cloudSaveData.TimeData },
            { "SignData", cloudSaveData.SignData },
            { "ItemData", cloudSaveData.ItemData },
            { "MissionData", cloudSaveData.MissionData },
            { "ExistRoomDatas", cloudSaveData.ExistRoomDatas },
            { "PurchaseRecords", cloudSaveData.PurchaseRecords },
            { "MailReceivedDatas", cloudSaveData.MailReceivedDatas },
            { "GreenHouseDatas", cloudSaveData.GreenHouseDatas }
        };

        return result;
    }

    public CloudSave_PlayerData GetPlayerData()
    {
        CloudSave_PlayerData playerData = new CloudSave_PlayerData();

        playerData.PlayerName = app.system.player.PlayerName;
        playerData.PlayerId = app.system.player.PlayerId;
        playerData.Level = app.system.player.Level;
        playerData.Exp = app.system.player.Exp;
        playerData.DiamondCatSlot = app.system.player.DiamondCatSlot;
        playerData.GridSizeLevel = app.system.player.GridSizeLevel;
        playerData.PlayerGender = app.system.player.PlayerGender;
        playerData.UsingIcon = app.system.player.UsingIcon;
        playerData.UsingAvatar = app.system.player.UsingAvatar;
        playerData.CatDeadCount = app.system.player.CatDeadCount;
        playerData.StartTutorialEnd = app.system.tutorial.startTutorialEnd;
        playerData.ShelterTutorialEnd = app.system.tutorial.shelterTutorialEnd;

        return playerData;
    }

    public CloudSave_FriendData GetFriendData()
    {
        CloudSave_FriendData friendData = new CloudSave_FriendData();

        friendData.FriendIds = new List<string>();
        friendData.FriendInvites = new List<string>();

        var friends = app.model.friend.Friends;

        for (int i = 0; i < friends.Count; i++)
            friendData.FriendIds.Add(friends[i].PlayerId);

        friendData.FriendInvites = app.model.friend.myInvites;

        return friendData;
    }

    public CloudSave_TimeData GetTimeData()
    {
        CloudSave_TimeData timeData = new CloudSave_TimeData();

        timeData.FirstLoginDateTime = Timestamp.FromDateTime(app.system.myTime.AccountCreateDateTime);
        timeData.PerDayLoginDateTime = Timestamp.FromDateTime(app.system.myTime.PerDayLoginDateTime);
        timeData.LastLoginDateTime = Timestamp.FromDateTime(app.system.myTime.LastLoginDateTime);

        return timeData;
    }

    public CloudSave_SignData GetSignData()
    {
        CloudSave_SignData signData = new CloudSave_SignData();

        signData.MonthSigns = app.model.monthSign.SignIndexs;
        signData.LastMonthSignDate = Timestamp.FromDateTime(app.model.monthSign.LastMonthSignDate);

        return signData;
    }

    public CloudSave_ItemData GetItemData()
    {
        CloudSave_ItemData itemData = new CloudSave_ItemData();
        itemData.CommonData = app.system.inventory.CommonData;
        itemData.RoomData = app.system.inventory.RoomData;
        itemData.FoodData = app.system.inventory.FoodData;
        itemData.ToolData = app.system.inventory.ToolData;
        itemData.LitterData = app.system.inventory.LitterData;
        itemData.SkinData = app.system.inventory.SkinData;
        itemData.PlayerIconData = app.system.inventory.PlayerIconData;
        itemData.PlayerAvatarData = app.system.inventory.PlayerAvatarData;

        return itemData;
    }

    public CloudSave_MissionData GetMissionData()
    {
        CloudSave_MissionData missionData = new CloudSave_MissionData();

        missionData.QuestProgressData = app.system.quest.QuestProgressData;
        missionData.QuestReceivedStatusData = app.system.quest.QuestReceivedStatusData;
        missionData.KnowledgeCardData = app.system.quest.KnowledgeCardData;
        missionData.KnowledgeCardStatus = app.system.quest.KnowledgeCardStatus;
        
        missionData.MyQuests = new List<string>();
        for (int i = 0; i < app.model.dailyQuest.Quests.Count; i++)
        {
            missionData.MyQuests.Add(app.model.dailyQuest.Quests[i].id);
        }

        return missionData;
    }

    public List<CloudSave_RoomData> GetExistRoomDatas()
    {
        var result = new List<CloudSave_RoomData>();

        var rooms = app.system.room.MyRooms;

        for (int i = 0; i < rooms.Count; i++)
        {
            var room = rooms[i];
            var roomData = new CloudSave_RoomData();
            roomData.Id = room.roomData.id;
            roomData.X = room.x;
            roomData.Y = room.y;
            result.Add(roomData);
        }

        return result;
    }

    #endregion
}