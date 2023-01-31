using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public class GreenHouseData
{
    [FirestoreProperty] public string FlowerID { get; set; }
    [FirestoreProperty] public int Page { get; set; }
    [FirestoreProperty] public int Position { get; set; }
}
