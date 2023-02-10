using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class CloudSaveData
{
    public CloudSaveData()
    {
        PlayerData = new CloudSave_PlayerData();
        FriendData = new CloudSave_FriendData();
        TimeData = new CloudSave_TimeData();
        SignData = new CloudSave_SignData();
        ItemData = new CloudSave_ItemData();
        MissionData = new CloudSave_MissionData();
        MailReceivedDatas = new List<string>();
        MailReceivedDatas = new List<string>();
        ExistRoomDatas = new List<CloudSave_RoomData>();
        PurchaseRecords = new Dictionary<string, PurchaseRecord>();
    }

    [FirestoreProperty] public CloudSave_PlayerData PlayerData { get; set; }
    [FirestoreProperty] public CloudSave_FriendData FriendData { get; set; }
    [FirestoreProperty] public CloudSave_TimeData TimeData { get; set; }
    [FirestoreProperty] public CloudSave_SignData SignData { get; set; }
    [FirestoreProperty] public CloudSave_ItemData ItemData { get; set; }
    [FirestoreProperty] public CloudSave_MissionData MissionData { get; set; }
    [FirestoreProperty] public List<string> MailReceivedDatas { get; set; }
    [FirestoreProperty] public List<CloudSave_RoomData> ExistRoomDatas { get; set; }
    [FirestoreProperty] public Dictionary<string, PurchaseRecord> PurchaseRecords { get; set; }
    [FirestoreProperty] public List<GreenHouseData> GreenHouseDatas { get; set; }
}