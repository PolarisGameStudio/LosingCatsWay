using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class CloudLosingCatData
{
    [FirestoreProperty] public CloudSave_CatData CatData { get; set; }
    [FirestoreProperty] public CloudSave_CatSkinData CatSkinData { get; set; }
    [FirestoreProperty] public CloudSave_CatDiaryData CatDiaryData { get; set; }
    
    [FirestoreProperty] public List<string> LosingCatStatus { get; set; }
    [FirestoreProperty] public Timestamp ExpiredTimestamp { get; set; }
    [FirestoreProperty] public bool IsGetMemory { get; set; }
    
    public bool IsExpired => ExpiredTimestamp <= Timestamp.GetCurrentTimestamp();
}