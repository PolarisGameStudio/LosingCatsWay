using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public class CloudSave_PlayerData
{
    [FirestoreProperty] public string PlayerName { get; set; }
    [FirestoreProperty] public string PlayerId { get; set; }
    [FirestoreProperty] public int Level { get; set; }
    [FirestoreProperty] public int Exp { get; set; }
    [FirestoreProperty] public int Coin { get; set; }
    [FirestoreProperty] public int Diamond { get; set; }
    [FirestoreProperty] public int DiamondCatSlot { get; set; }
    [FirestoreProperty] public int GridSize { get; set; }
    [FirestoreProperty] public int PlayerGender { get; set; }
    [FirestoreProperty] public string UsingIcon { get; set; }
    [FirestoreProperty] public string UsingAvatar { get; set; }
    [FirestoreProperty] public int TutorialIndex { get; set; }
}