using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class CloudSaveSystem : MvcBehaviour
{
    #region Player

    public async Task<CloudSaveData> LoadCloudSaveData(string playerId = null)
    {
        if (playerId == null)
            playerId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Players").Document(playerId);

        DocumentSnapshot result = await docRef.GetSnapshotAsync();
        CloudSaveData cloudSaveData = result.ConvertTo<CloudSaveData>();

        return cloudSaveData;
    }

    public async Task<CloudCatData> LoadCloudCatData(string catId)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Cats").Document(catId);

        DocumentSnapshot result = await docRef.GetSnapshotAsync();
        CloudCatData cloudCatData = result.ConvertTo<CloudCatData>();

        return cloudCatData;
    }

    public async void SaveCloudSaveData()
    {
        CloudSaveData cloudSaveData = new CloudSaveData();

        // CloudSave_PlayerData
        CloudSave_PlayerData playerData = new CloudSave_PlayerData();

        playerData.PlayerStatus = App.system.player.playerStatus;
        
        playerData.PlayerName = App.system.player.PlayerName;
        playerData.PlayerId = App.system.player.PlayerId;
        playerData.Level = App.system.player.Level;
        playerData.Exp = App.system.player.Exp;
        playerData.Coin = App.system.player.Coin;
        playerData.Diamond = App.system.player.Diamond;
        playerData.CatMemory = App.system.player.CatMemory;
        playerData.DiamondCatSlot = App.system.player.DiamondCatSlot;
        playerData.GridSizeLevel = App.system.player.GridSizeLevel;
        playerData.PlayerGender = App.system.player.PlayerGender;
        playerData.UsingIcon = App.system.player.UsingIcon;
        playerData.UsingAvatar = App.system.player.UsingAvatar;
        playerData.TutorialIndex = App.system.tutorial.directorIndex;
        playerData.CatDeadCount = App.system.player.CatDeadCount;

        // CloudSave_FriendData
        CloudSave_FriendData friendData = new CloudSave_FriendData();

        friendData.FriendIds = new List<string>();
        friendData.FriendInvites = new List<string>();

        var friends = App.model.friend.Friends;

        for (int i = 0; i < friends.Count; i++)
            friendData.FriendIds.Add(friends[i].PlayerId);

        friendData.FriendInvites = App.model.friend.myInvites;

        // CloudSave_TimeData
        CloudSave_TimeData timeData = new CloudSave_TimeData();

        timeData.FirstLoginDateTime = Timestamp.FromDateTime(App.system.myTime.AccountCreateDateTime);
        timeData.PerDayLoginDateTime = Timestamp.FromDateTime(App.system.myTime.PerDayLoginDateTime);
        timeData.LastLoginDateTime = Timestamp.FromDateTime(App.system.myTime.LastLoginDateTime);
        
        // CloudSave_SignData
        CloudSave_SignData signData = new CloudSave_SignData();
        
        signData.MonthSigns = App.model.monthSign.SignIndexs;
        signData.MonthResignCount = App.model.monthSign.ResignCount;
        signData.LastMonthSignDate = Timestamp.FromDateTime(App.model.monthSign.LastMonthSignDate);

        // CloudSave_ItemData
        CloudSave_ItemData itemData = new CloudSave_ItemData();
        itemData.RoomData = App.system.inventory.RoomData;
        itemData.FoodData = App.system.inventory.FoodData;
        itemData.ToolData = App.system.inventory.ToolData;
        itemData.LitterData = App.system.inventory.LitterData;
        itemData.SkinData = App.system.inventory.SkinData;
        itemData.ItemsCanBuyAtStore = App.system.inventory.itemsCanBuyAtStore;
        itemData.PlayerIconData = App.system.inventory.PlayerIconData;
        itemData.PlayerAvatarData = App.system.inventory.PlayerAvatarData;

        // MissionDatas
        CloudSave_MissionData missionData = new CloudSave_MissionData();

        missionData.QuestProgressData = App.system.quest.QuestProgressData;
        missionData.QuestReceivedStatusData = App.system.quest.QuestReceivedStatusData;
        missionData.MyQuests = new List<string>();
        for (int i = 0; i < App.model.dailyQuest.Quests.Count; i++)
        {
            missionData.MyQuests.Add(App.model.dailyQuest.Quests[i].id);
        }

        cloudSaveData.PlayerData = playerData;
        cloudSaveData.FriendData = friendData;
        cloudSaveData.TimeData = timeData;
        cloudSaveData.SignData = signData;
        cloudSaveData.ItemData = itemData;
        cloudSaveData.MissionData = missionData;

        // ExistRoomDatas
        var rooms = App.system.room.MyRooms;
        cloudSaveData.ExistRoomDatas = new List<CloudSave_RoomData>();

        for (int i = 0; i < rooms.Count; i++)
        {
            var room = rooms[i];
            var result = new CloudSave_RoomData();
            result.Id = room.roomData.id;
            result.X = room.x;
            result.Y = room.y;
            cloudSaveData.ExistRoomDatas.Add(result);
        }
        
        // Purchase
        cloudSaveData.PurchaseRecords = App.model.mall.PurchaseRecords;

        // MailReceivedDatas
        cloudSaveData.MailReceivedDatas = new List<string>();
        
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Players").Document(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        await docRef.UpdateAsync(cloudSaveData.ToDict());
    }

    public async Task<CloudSaveData> CreateCloudSaveData()
    {
        CloudSaveData cloudSaveData = new CloudSaveData();

        // CloudSave_PlayerData
        CloudSave_PlayerData playerData = new CloudSave_PlayerData();

        playerData.PlayerStatus = App.system.player.playerStatus;

        playerData.PlayerName = "-";
        playerData.PlayerId = CloudSaveExtension.CurrentUserId;
        playerData.Level = 1;
        playerData.Exp = 0;
        playerData.Coin = 1000;
        playerData.Diamond = 0;
        playerData.CatMemory = 0;
        playerData.DiamondCatSlot = 0;
        playerData.GridSizeLevel = 1;
        playerData.PlayerGender = -1;
        playerData.UsingIcon = string.Empty;
        playerData.UsingAvatar = "PAT001";
        playerData.TutorialIndex = App.system.tutorial.directorIndex;
        playerData.CatDeadCount = 0;

        // CloudSave_FriendData
        CloudSave_FriendData friendData = new CloudSave_FriendData();

        friendData.FriendIds = new List<string>();
        friendData.FriendInvites = new List<string>();

        // CloudSave_TimeData
        CloudSave_TimeData timeData = new CloudSave_TimeData();

        Timestamp nowTime = Timestamp.GetCurrentTimestamp();
        
        timeData.LastLoginDateTime = nowTime;
        timeData.PerDayLoginDateTime = Timestamp.FromDateTime(DateTime.MinValue); // 第一次登入 要可以簽到
        timeData.FirstLoginDateTime = nowTime;

        // CloudSave_SignData
        CloudSave_SignData signData = new CloudSave_SignData();

        signData.MonthSigns = new List<int>(new int[31]);
        signData.MonthResignCount = 0;
        signData.LastMonthSignDate = Timestamp.FromDateTime(DateTime.MinValue) ;

        // CloudSave_ItemData
        CloudSave_ItemData itemData = new CloudSave_ItemData();
        itemData.RoomData = App.system.inventory.RoomData;
        itemData.FoodData = App.system.inventory.FoodData;
        itemData.ToolData = App.system.inventory.ToolData;
        itemData.LitterData = App.system.inventory.LitterData;
        itemData.SkinData = App.system.inventory.SkinData;
        itemData.ItemsCanBuyAtStore = App.system.inventory.itemsCanBuyAtStore;
        itemData.PlayerIconData = App.system.inventory.PlayerIconData;
        itemData.PlayerAvatarData = App.system.inventory.PlayerAvatarData;

        // MissionDatas
        CloudSave_MissionData missionData = new CloudSave_MissionData();

        missionData.QuestProgressData = App.system.quest.QuestProgressData;
        missionData.QuestReceivedStatusData = App.system.quest.QuestReceivedStatusData;
        missionData.MyQuests = new List<string>();

        // MailReceivedDatas
        cloudSaveData.MailReceivedDatas = new List<string>();
        
        cloudSaveData.PlayerData = playerData;
        cloudSaveData.FriendData = friendData;
        cloudSaveData.TimeData = timeData;
        cloudSaveData.SignData = signData;
        cloudSaveData.ItemData = itemData;
        cloudSaveData.MissionData = missionData;

        // ExistRoomDatas
        cloudSaveData.ExistRoomDatas = new List<CloudSave_RoomData>();

        // Purchase
        cloudSaveData.PurchaseRecords = new Dictionary<string, PurchaseRecord>();

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Players").Document(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        await docRef.SetAsync(cloudSaveData.ToDict());

        return cloudSaveData;
    }

    public async void UpdateCloudPlayerData()
    {
        CloudSave_PlayerData playerData = new CloudSave_PlayerData();

        playerData.PlayerStatus = App.system.player.playerStatus;

        playerData.PlayerName = App.system.player.PlayerName;
        playerData.PlayerId = App.system.player.PlayerId;
        playerData.Level = App.system.player.Level;
        playerData.Exp = App.system.player.Exp;
        playerData.Coin = App.system.player.Coin;
        playerData.Diamond = App.system.player.Diamond;
        playerData.CatMemory = App.system.player.CatMemory;
        playerData.DiamondCatSlot = App.system.player.DiamondCatSlot;
        playerData.GridSizeLevel = App.system.player.GridSizeLevel;
        playerData.PlayerGender = App.system.player.PlayerGender;
        playerData.UsingIcon = App.system.player.UsingIcon;
        playerData.UsingAvatar = App.system.player.UsingAvatar;
        playerData.TutorialIndex = App.system.tutorial.directorIndex;
        playerData.CatDeadCount = App.system.player.CatDeadCount;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Players").Document(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "PlayerData", playerData }
        };
        await docRef.UpdateAsync(updates);
    }

    public async void UpdateCloudItemData()
    {
        CloudSave_ItemData itemData = new CloudSave_ItemData();
        itemData.RoomData = App.system.inventory.RoomData;
        itemData.FoodData = App.system.inventory.FoodData;
        itemData.ToolData = App.system.inventory.ToolData;
        itemData.LitterData = App.system.inventory.LitterData;
        itemData.SkinData = App.system.inventory.SkinData;
        itemData.ItemsCanBuyAtStore = App.system.inventory.itemsCanBuyAtStore;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Players").Document(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "ItemData", itemData }
        };
        await docRef.UpdateAsync(updates);
    }

    public async void UpdateCloudTimeData()
    {
        CloudSave_TimeData timeData = new CloudSave_TimeData();
        timeData.LastLoginDateTime = Timestamp.FromDateTime(App.system.myTime.LastLoginDateTime);
        timeData.PerDayLoginDateTime = Timestamp.FromDateTime(App.system.myTime.PerDayLoginDateTime);
        timeData.FirstLoginDateTime = Timestamp.FromDateTime(App.system.myTime.AccountCreateDateTime);

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Players").Document(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "TimeData", timeData }
        };
        await docRef.UpdateAsync(updates);
    }

    public async void UpdateCloudMissionData()
    {
        CloudSave_MissionData missionData = new CloudSave_MissionData();
        missionData.QuestProgressData = App.system.quest.QuestProgressData;
        missionData.QuestReceivedStatusData = App.system.quest.QuestReceivedStatusData;
        missionData.MyQuests = new List<string>();
        for (int i = 0; i < App.model.dailyQuest.Quests.Count; i++)
        {
            missionData.MyQuests.Add(App.model.dailyQuest.Quests[i].id);
        }
        
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Players").Document(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "MissionData", missionData }
        };
        await docRef.UpdateAsync(updates);
    }
    
    public async void UpdateCloudSignData()
    {
        CloudSave_SignData signData = new CloudSave_SignData();
        signData.MonthSigns = App.model.monthSign.SignIndexs;
        signData.MonthResignCount = App.model.monthSign.ResignCount;
        signData.LastMonthSignDate = Timestamp.FromDateTime(App.model.monthSign.LastMonthSignDate);
        
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Players").Document(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "SignData", signData }
        };
        await docRef.UpdateAsync(updates);
    }

    #endregion

    #region Cats

    public async void UpdateCloudCatData(CloudCatData cloudCatData)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Cats").Document(cloudCatData.CatData.CatId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "CatData", cloudCatData.CatData }
        };
        await docRef.UpdateAsync(updates);
    }

    public async void UpdateCloudCatSurviveData(CloudCatData cloudCatData)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Cats").Document(cloudCatData.CatData.CatId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "CatSurviveData", cloudCatData.CatSurviveData }
        };
        await docRef.UpdateAsync(updates);
    }

    public async void UpdateCloudCatHealthData(CloudCatData cloudCatData)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Cats").Document(cloudCatData.CatData.CatId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "CatHealthData", cloudCatData.CatHealthData }
        };
        await docRef.UpdateAsync(updates);
    }

    public async void UpdateCloudCatDiaryData(CloudCatData cloudCatData)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Cats").Document(cloudCatData.CatData.CatId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "CatDiaryData", cloudCatData.CatDiaryData }
        };
        await docRef.UpdateAsync(updates);
    }

    public async void UpdateCloudCatSkinData(CloudCatData cloudCatData)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Cats").Document(cloudCatData.CatData.CatId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "CatSkinData", cloudCatData.CatSkinData }
        };
        await docRef.UpdateAsync(updates);
    }
    
    public async void UpdateCloudCatServerData(CloudCatData cloudCatData)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Cats").Document(cloudCatData.CatData.CatId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "CatServerData", cloudCatData.CatServerData }
        };
        await docRef.UpdateAsync(updates);
    }

    public async Task<List<CloudCatData>> LoadCloudCatDatas()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference catsRef = db.Collection("Cats");
        Query query = catsRef.WhereEqualTo("CatData.Owner", FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        var documentSnapshots = await query.GetSnapshotAsync();
        List<CloudCatData> result = new List<CloudCatData>();
        foreach (var doc in documentSnapshots.Documents)
        {
            result.Add(doc.ConvertTo<CloudCatData>());
        }

        return result;
    }

    public async Task<List<CloudCatData>> LoadCloudCatDatasByOwner(string owner, int limit)
    {
        string randomKey = FirebaseFirestore.DefaultInstance.Collection("Cats").Document().Id;
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference catsRef = db.Collection("Cats");
        Query query = catsRef.WhereEqualTo("CatData.Owner", owner).WhereGreaterThanOrEqualTo("CatData.CatId", randomKey)
            .Limit(limit);
        var documentSnapshots = await query.GetSnapshotAsync();
        List<CloudCatData> result = new List<CloudCatData>();
        foreach (var doc in documentSnapshots.Documents)
        {
            result.Add(doc.ConvertTo<CloudCatData>());
        }

        return result;
    }

    public async Task<CloudCatData> LoadCloudCatDataById(string catId)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        var catsRef = db.Collection("Cats").Document(catId);
        var result = await catsRef.GetSnapshotAsync();
        if (!result.Exists) return null;
        return result.ConvertTo<CloudCatData>();
    }

    public async void DeleteCloudCatData(CloudCatData cloudCatData)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        var docRef = db.Collection("Cats").Document(cloudCatData.CatData.CatId);
        await docRef.DeleteAsync();
    }

    #endregion

    #region LosingCats

    public async Task<CloudLosingCatData> CreateCloudLosingCatData(CloudCatData cloudCatData)
    {
        CloudLosingCatData losingCatData = new CloudLosingCatData();
        losingCatData.CatData = cloudCatData.CatData;
        losingCatData.CatSkinData = cloudCatData.CatSkinData;
        losingCatData.CatDiaryData = cloudCatData.CatDiaryData;
        
        var losingCats = await App.system.cloudSave.LoadCloudLosingCatDatas(App.system.player.PlayerId);
        losingCatData.LosingCatStatus = new List<string>();
        if (losingCats.Count <= 0)
            losingCatData.LosingCatStatus.Add("First");
        
        losingCatData.ExpiredTimestamp = Timestamp.FromDateTime(App.system.myTime.MyTimeNow.AddDays(7));
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        var losingCatRef = db.Collection("LosingCats").Document(losingCatData.CatData.CatId);
        await losingCatRef.SetAsync(losingCatData);

        return losingCatData;
    }

    public async Task<List<CloudLosingCatData>> LoadCloudLosingCatDatas(string owner)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        var losingCatRef = db.Collection("LosingCats");
        Query query = losingCatRef.WhereEqualTo("CatData.Owner", owner);
        var documentSnapshots = await query.GetSnapshotAsync();
        List<CloudLosingCatData> result = new List<CloudLosingCatData>();
        foreach (var doc in documentSnapshots)
        {
            result.Add(doc.ConvertTo<CloudLosingCatData>());
        }

        return result;
    }

    public async void UpdateLosingCatDiaryData(CloudLosingCatData cloudLosingCatData)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("LosingCats").Document(cloudLosingCatData.CatData.CatId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "CatDiaryData", cloudLosingCatData.CatDiaryData }
        };
        await docRef.UpdateAsync(updates);
    }
    
    public void UpdateLosingCatStatusData(CloudLosingCatData cloudLosingCatData)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("LosingCats").Document(cloudLosingCatData.CatData.CatId);
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "LosingCatStatus", cloudLosingCatData.LosingCatStatus }
        };
        docRef.UpdateAsync(updates);
    }

    public async void DeleteLosingCatData(CloudLosingCatData cloudLosingCatData)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        var docRef = db.Collection("LosingCats").Document(cloudLosingCatData.CatData.CatId);
        await docRef.DeleteAsync();
    }

    #endregion

    #region OtherPlayer

    public async Task<string> LoadOtherPlayerName(string playerId)
    {
        if (playerId == App.system.player.PlayerId)
            return App.system.player.PlayerName;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Player").Document(playerId);

        DocumentSnapshot result = await docRef.GetSnapshotAsync();

        if (!result.Exists)
            return string.Empty;

        var data = result.ConvertTo<CloudSaveData>();
        var playerName = data.PlayerData.PlayerName;

        return playerName;
    }

    #endregion

    #region Friend

    public async Task UpdateFriendData()
    {
        var myId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Players").Document(myId);

        CloudSave_FriendData friendData = new CloudSave_FriendData();

        friendData.FriendIds = new List<string>();
        friendData.FriendInvites = new List<string>();

        var friends = App.model.friend.Friends;

        for (int i = 0; i < friends.Count; i++)
            friendData.FriendIds.Add(friends[i].PlayerId);

        friendData.FriendInvites = App.model.friend.myInvites;

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "FriendData", friendData }
        };

        await docRef.UpdateAsync(updates);
    }

    public async Task<List<FriendData>> LoadFriends()
    {
        var playerId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        var docRef = db.Collection("Players");

        Query query = docRef.WhereArrayContains("FriendData.FriendIds", playerId);
        var documentSnapshots = await query.GetSnapshotAsync();

        List<FriendData> result = new List<FriendData>();

        foreach (var doc in documentSnapshots)
        {
            CloudSaveData cloudSaveData = doc.ConvertTo<CloudSaveData>();
            FriendData friendData = new FriendData();

            friendData.PlayerId = cloudSaveData.PlayerData.PlayerId;
            friendData.PlayerName = cloudSaveData.PlayerData.PlayerName;
            friendData.Level = cloudSaveData.PlayerData.Level;

            result.Add(friendData);
        }

        return result;
    }

    public async Task<FriendData> LoadFriend(string friendId)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        var docRef = db.Collection("Players").Document(friendId);

        DocumentSnapshot result = await docRef.GetSnapshotAsync();
        CloudSaveData cloudSaveData = result.ConvertTo<CloudSaveData>();

        FriendData friendData = new FriendData();

        friendData.PlayerId = cloudSaveData.PlayerData.PlayerId;
        friendData.PlayerName = cloudSaveData.PlayerData.PlayerName;
        friendData.Level = cloudSaveData.PlayerData.Level;
        
        return friendData;
    }

    public async Task<List<FriendData>> LoadInvites()
    {
        var playerId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        var docRef = db.Collection("Players");

        Query query = docRef.WhereArrayContains("FriendData.FriendInvites", playerId);
        var documentSnapshots = await query.GetSnapshotAsync();

        List<FriendData> result = new List<FriendData>();

        foreach (var doc in documentSnapshots)
        {
            CloudSaveData cloudSaveData = doc.ConvertTo<CloudSaveData>();
            FriendData friendData = new FriendData();

            friendData.PlayerId = cloudSaveData.PlayerData.PlayerId;
            friendData.PlayerName = cloudSaveData.PlayerData.PlayerName;
            friendData.Level = cloudSaveData.PlayerData.Level;

            result.Add(friendData);
        }

        return result;
    }

    public async Task AddFriendAndDeleteInvite(string friendId)
    {
        var myId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Players").Document(friendId);
        DocumentSnapshot result = await docRef.GetSnapshotAsync();

        var data = result.ConvertTo<CloudSaveData>();
        data.FriendData.FriendIds.Add(myId);
        data.FriendData.FriendInvites.Remove(myId);

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "FriendData", data.FriendData }
        };

        await docRef.UpdateAsync(updates);
    }

    public async Task DeleteInvite(string friendId)
    {
        var myId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Players").Document(friendId);
        DocumentSnapshot result = await docRef.GetSnapshotAsync();

        var data = result.ConvertTo<CloudSaveData>();
        data.FriendData.FriendInvites.Remove(myId);

        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            { "FriendData", data.FriendData }
        };

        await docRef.UpdateAsync(updates);
    }

    public async Task<CloudCatData> GetFriendFavoriteCat(string friendId)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        CollectionReference catsRef = db.Collection("Cats");
        Query query = catsRef.WhereEqualTo("CatData.Owner", friendId).WhereEqualTo("CatData.IsFavorite", true);
        var documentSnapshots = await query.GetSnapshotAsync();
        
        List<CloudCatData> result = new List<CloudCatData>();
        foreach (var doc in documentSnapshots.Documents)
            result.Add(doc.ConvertTo<CloudCatData>());

        if (result.Count == 0)
            return null;

        return result[0];
    }

    #endregion

    #region Exist

    public async Task<bool> IsCloudSaveDataExist()
    {
        var playerId = CloudSaveExtension.CurrentUserId;
        var snapshot = await FirebaseFirestore.DefaultInstance.Collection("Players").Document(playerId)
            .GetSnapshotAsync();
        return snapshot.Exists;
    }

    public async Task<bool> IsPlayerIdExist(string playerId)
    {
        var snapshot = await FirebaseFirestore.DefaultInstance.Collection("Players").Document(playerId)
            .GetSnapshotAsync();
        return snapshot.Exists;
    }

    #endregion
}