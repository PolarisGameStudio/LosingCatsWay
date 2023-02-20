using Firebase.Firestore;

[FirestoreData]
public class Cloud_WorldData
{
    [FirestoreProperty] public int AdoptedCount { get; set; }
    [FirestoreProperty] public int AdultCount { get; set; }
    [FirestoreProperty] public int CatCount { get; set; }
    [FirestoreProperty] public int ChildCount { get; set; }
    [FirestoreProperty] public int LigationCount { get; set; }
    [FirestoreProperty] public int OldCount { get; set; }
    [FirestoreProperty] public int OutdoorCount { get; set; }
    [FirestoreProperty] public int ShelterCount { get; set; }
    [FirestoreProperty] public int BuyCatCount { get; set; }
    [FirestoreProperty] public int DeleteCatCount { get; set; }
    [FirestoreProperty] public int AddCatCount { get; set; }
}