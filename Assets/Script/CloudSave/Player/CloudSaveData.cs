using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class CloudSaveData
{
    [FirestoreProperty] public CloudSave_PlayerData PlayerData { get; set; }
    [FirestoreProperty] public CloudSave_FriendData FriendData { get; set; }
    [FirestoreProperty] public CloudSave_TimeData TimeData { get; set; }
    [FirestoreProperty] public CloudSave_SignData SignData { get; set; }
    [FirestoreProperty] public CloudSave_ItemData ItemData { get; set; }
    [FirestoreProperty] public CloudSave_MissionData MissionData { get; set; }
    [FirestoreProperty] public List<string> MailReceivedDatas { get; set; }
    [FirestoreProperty] public List<CloudSave_RoomData> ExistRoomDatas { get; set; }
    [FirestoreProperty] public Dictionary<string, PurchaseRecord> PurchaseRecords { get; set; }
    
}