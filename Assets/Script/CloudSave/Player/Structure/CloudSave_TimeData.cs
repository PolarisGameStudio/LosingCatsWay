using Firebase.Firestore;

[FirestoreData]
public class CloudSave_TimeData
{
    //FirstLoginTime 只要創角時記錄
    [FirestoreProperty] public Timestamp FirstLoginDateTime { get; set; }
    [FirestoreProperty] public Timestamp PerDayLoginDateTime { get; set; }
    [FirestoreProperty] public Timestamp LastLoginDateTime { get; set; } 
}