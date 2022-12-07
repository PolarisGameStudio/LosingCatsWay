using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class CloudSave_MissionData
{
    [FirestoreProperty] public Dictionary<string, int> QuestProgressData { get; set; }
    [FirestoreProperty] public Dictionary<string, bool> QuestIsReceivedData { get; set; }
    [FirestoreProperty] public List<string> MyQuests { get; set; }
}