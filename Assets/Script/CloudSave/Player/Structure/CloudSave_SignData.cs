using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public class CloudSave_SignData
{
    [FirestoreProperty] public List<int> MonthSigns { get; set; }
    [FirestoreProperty] public int MonthResignCount { get; set; }
    [FirestoreProperty] public Timestamp LastMonthSignDate { get; set; }
    [FirestoreProperty] public int WeekSigns { get; set; }
}