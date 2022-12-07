using Firebase.Firestore;

[FirestoreData]
public class CloudLosingCatData
{
    [FirestoreProperty] public CloudSave_CatData CatData { get; set; }
    [FirestoreProperty] public CloudSave_CatSkinData CatSkinData { get; set; }
    [FirestoreProperty] public CloudSave_CatDiaryData CatDiaryData { get; set; }

    [FirestoreProperty] public CloudSave_CatServerData CatServerData { get; set; }
}