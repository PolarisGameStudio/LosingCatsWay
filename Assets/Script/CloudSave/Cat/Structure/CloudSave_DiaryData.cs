using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public class CloudSave_DiaryData
{
    [FirestoreProperty] public Timestamp DiaryDate { get; set; }
    [FirestoreProperty] public string DiaryId { get; set; }
}
