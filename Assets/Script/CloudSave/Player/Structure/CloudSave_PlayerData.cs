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
    [FirestoreProperty] public int DiamondCatSlot { get; set; }
    [FirestoreProperty] public int GridSizeLevel { get; set; }
    [FirestoreProperty] public int PlayerGender { get; set; }
    [FirestoreProperty] public string UsingIcon { get; set; }
    [FirestoreProperty] public string UsingAvatar { get; set; }
    [FirestoreProperty] public int CatDeadCount { get; set; } //玩家死過幾隻貓
    [FirestoreProperty] public bool StartTutorialEnd { get; set; } //新手教學結束
    [FirestoreProperty] public bool ShelterTutorialEnd { get; set; } //收容所教學結束
}