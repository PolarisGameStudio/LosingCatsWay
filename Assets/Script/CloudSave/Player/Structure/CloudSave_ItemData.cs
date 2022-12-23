using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class CloudSave_ItemData
{
    [FirestoreProperty] public Dictionary<string, int> RoomData { get; set; }
    [FirestoreProperty] public Dictionary<string, int> FoodData { get; set; }
    [FirestoreProperty] public Dictionary<string, int> ToolData { get; set; }
    [FirestoreProperty] public Dictionary<string, int> LitterData { get; set; }
    [FirestoreProperty] public Dictionary<string, int> SkinData { get; set; }
    [FirestoreProperty] public Dictionary<string, bool> ItemsCanBuyAtStore { get; set; }
    
    //Player
    [FirestoreProperty] public Dictionary<string, int> PlayerIconData { get; set; }
    [FirestoreProperty] public Dictionary<string, int> PlayerAvatarData { get; set; }
}