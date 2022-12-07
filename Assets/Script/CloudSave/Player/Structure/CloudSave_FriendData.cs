using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class CloudSave_FriendData
{
    [FirestoreProperty] public List<string> FriendIds { get; set; }
    [FirestoreProperty] public List<string> FriendInvites { get; set; }
}
