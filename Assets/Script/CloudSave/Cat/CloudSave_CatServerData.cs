using Firebase.Firestore;

[FirestoreData]
public class CloudSave_CatServerData
{
    [FirestoreProperty] public bool IsDead { get; set; }
}
