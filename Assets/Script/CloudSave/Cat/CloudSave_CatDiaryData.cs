using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public class CloudSave_CatDiaryData
{
    [FirestoreProperty] public List<CloudSave_DiaryData> DiaryDatas { get; set; }
    [FirestoreProperty] public Timestamp AdoptTimestamp { get; set; }
    [FirestoreProperty] public string AdoptLocation { get; set; }
    [FirestoreProperty] public int DiarySatietyScore { get; set; }
    [FirestoreProperty] public int DiaryLitterScore { get; set; }
    [FirestoreProperty] public int DiaryMoistureScore { get; set; }
    [FirestoreProperty] public int DiaryFavourbilityScore { get; set; }
    [FirestoreProperty] public bool UsedFlower { get; set; }
    [FirestoreProperty] public Timestamp FlowerExpiredTimestamp { get; set; }
}
