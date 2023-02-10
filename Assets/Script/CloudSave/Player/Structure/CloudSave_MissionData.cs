using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class CloudSave_MissionData
{
    [FirestoreProperty] public Dictionary<string, int> QuestProgressData { get; set; }
    [FirestoreProperty] public Dictionary<string, int> QuestReceivedStatusData { get; set; }
    
    [FirestoreProperty] public Dictionary<string, int> KnowledgeCardData { get; set; }
    [FirestoreProperty] public Dictionary<string, int> KnowledgeCardStatus { get; set; }
    [FirestoreProperty] public List<string> MyQuests { get; set; }
}