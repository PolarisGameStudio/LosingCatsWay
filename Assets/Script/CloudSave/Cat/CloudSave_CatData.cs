using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public class CloudSave_CatData
{
    [FirestoreProperty] public string CatId { get; set; }
    [FirestoreProperty] public string CatName { get; set; }
    [FirestoreProperty] public byte Sex { get; set; } //0:Male 1:Female
    [FirestoreProperty] public string Variety { get; set; }
    [FirestoreProperty] public string Owner { get; set; }
    [FirestoreProperty] public float BodyScale { get; set; }
    [FirestoreProperty] public List<int> PersonalityTypes { get; set; }
    [FirestoreProperty] public List<int> PersonalityLevels { get; set; }
    [FirestoreProperty] public string Trait { get; set; }
    [FirestoreProperty] public Timestamp BornTime { get; set; }
    [FirestoreProperty] public Timestamp DeathTime { get; set; }
    [FirestoreProperty] public string ChipId { get; set; } //晶片主人ID
    [FirestoreProperty] public bool IsFavorite { get; set; }
    
    public int SurviveDays {
        get
        {
            DateTime nowTime = Timestamp.GetCurrentTimestamp().ToDateTime().ToLocalTime();
            return (nowTime - BornTime.ToDateTime().ToLocalTime()).Days;
        }
    }
    
    public int CatAge => SurviveDays / 2;
}