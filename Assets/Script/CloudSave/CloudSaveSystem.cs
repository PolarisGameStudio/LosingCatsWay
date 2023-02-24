using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

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

    public async void SaveCloudSaveData()
    {
        Dictionary<string, object> saveData = new PlayerDataHelper(App).GetSaveData();

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Players").Document(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        await docRef.UpdateAsync(saveData);
    }
    
    public void SaveCloudSaveDataSync()
    {
        Dictionary<string, object> saveData = new PlayerDataHelper(App).GetSaveData();

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Players").Document(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
        docRef.UpdateAsync(saveData);
    }

    #endregion

    #region Cats

    public async void SaveCloudCatDatas()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        WriteBatch batch = db.StartBatch();
        
        var cats = App.system.cat.GetCats();
        CatDataHelper catDataHelper = new CatDataHelper();

        foreach (var cat in cats)
        {
            string catId = cat.cloudCatData.CatData.CatId;
            Dictionary<string, object> updates = catDataHelper.GetUpdate(cat.cloudCatData);
            DocumentReference docRef = db.Collection("Cats").Document(catId);
            batch.Set(docRef, updates, SetOptions.MergeAll);
        }
        
        // Wait for all tasks to complete
        try
        {
            await batch.CommitAsync();
        }
        catch (Exception e)
        {
            print("An error occurred while updating cat data: " + e.Message);
        }
    }
    
    public void SaveCloudCatDatasSync()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        WriteBatch batch = db.StartBatch();
        
        var cats = App.system.cat.GetCats();
        CatDataHelper catDataHelper = new CatDataHelper();

        foreach (var cat in cats)
        {
            string catId = cat.cloudCatData.CatData.CatId;
            Dictionary<string, object> updates = catDataHelper.GetUpdate(cat.cloudCatData);
            DocumentReference docRef = db.Collection("Cats").Document(catId);
            batch.Set(docRef, updates, SetOptions.MergeAll);
        }
        
        // Wait for all tasks to complete
        try
        {
            batch.CommitAsync();
        }
        catch (Exception e)
        {
            print("An error occurred while updating cat data: " + e.Message);
        }
    }

    public async void SaveCloudCatData(CloudCatData cloudCatData)
    {
        CatDataHelper catDataHelper = new CatDataHelper();
        var updates = catDataHelper.GetUpdate(cloudCatData);
        
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Cats").Document(cloudCatData.CatData.CatId);
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

    // public async Task<CloudCatData> LoadCloudCatDataById(string catId)
    // {
    //     FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
    //     var catsRef = db.Collection("Cats").Document(catId);
    //     var result = await catsRef.GetSnapshotAsync();
    //     if (!result.Exists)
    //         return null;
    //     return result.ConvertTo<CloudCatData>();
    // }

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
        // CloudLosingCatData losingCatData = new CloudLosingCatData();
        // losingCatData.CatData = cloudCatData.CatData;
        // losingCatData.CatSkinData = cloudCatData.CatSkinData;
        // losingCatData.CatDiaryData = cloudCatData.CatDiaryData;
        //
        // var losingCats = await App.system.cloudSave.LoadCloudLosingCatDatas(App.system.player.PlayerId);
        // losingCatData.LosingCatStatus = new List<string>();
        // if (losingCats.Count <= 0)
        //     losingCatData.LosingCatStatus.Add("First");
        //
        // losingCatData.ExpiredTimestamp = Timestamp.FromDateTime(App.system.myTime.MyTimeNow.AddDays(7));

        LosingCatDataHelper helper = new LosingCatDataHelper(App);
        var data = helper.CreateLosingCatData(cloudCatData);
        
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        var losingCatRef = db.Collection("LosingCats").Document(data.CatData.CatId);
        await losingCatRef.SetAsync(data);

        return data;
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

    public async void SaveLosingCatData(CloudLosingCatData cloudLosingCatData)
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("LosingCats").Document(cloudLosingCatData.CatData.CatId);

        LosingCatDataHelper helper = new LosingCatDataHelper(App);
        var update = helper.GetUpdate(cloudLosingCatData);
        
        await docRef.UpdateAsync(update);
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

        CloudSave_FriendData friendData = new PlayerDataHelper(App).GetFriendData();
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
            friendData.UsingIcon = cloudSaveData.PlayerData.UsingIcon;
            friendData.UsingAvatar = cloudSaveData.PlayerData.UsingAvatar;

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
        friendData.UsingIcon = cloudSaveData.PlayerData.UsingIcon;
        friendData.UsingAvatar = cloudSaveData.PlayerData.UsingAvatar;
        
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
            friendData.UsingIcon = cloudSaveData.PlayerData.UsingIcon;
            friendData.UsingAvatar = cloudSaveData.PlayerData.UsingAvatar;
            
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