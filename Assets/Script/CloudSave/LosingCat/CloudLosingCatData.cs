using Firebase.Firestore;

[FirestoreData]
public class CloudLosingCatData
{
    [FirestoreProperty] public CloudSave_CatData CatData { get; set; }
    [FirestoreProperty] public CloudSave_CatSkinData CatSkinData { get; set; }
    [FirestoreProperty] public CloudSave_CatDiaryData CatDiaryData { get; set; }

    public bool IsExpired => CatDiaryData.FlowerExpiredTimestamp <= Timestamp.GetCurrentTimestamp();
}