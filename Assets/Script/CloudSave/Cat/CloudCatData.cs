using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class CloudCatData
{
    [FirestoreProperty] public CloudSave_CatData CatData { get; set; }
    [FirestoreProperty] public CloudSave_CatSkinData CatSkinData { get; set; }
    [FirestoreProperty] public CloudSave_CatSurviveData CatSurviveData { get; set; }
    [FirestoreProperty] public CloudSave_CatHealthData CatHealthData { get; set; }
    [FirestoreProperty] public CloudSave_CatDiaryData CatDiaryData { get; set; }
    [FirestoreProperty] public CloudSave_CatServerData CatServerData { get; set; }

}
