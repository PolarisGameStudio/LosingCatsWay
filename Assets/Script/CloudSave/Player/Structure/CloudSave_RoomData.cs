using Firebase.Firestore;

[FirestoreData]
public class CloudSave_RoomData
{
    [FirestoreProperty] public string Id { get; set; }
    [FirestoreProperty] public int X { get; set; }
    [FirestoreProperty] public int Y { get; set; }
}