using Firebase.Firestore;

[FirestoreData]
public class PurchaseRecord
{
    [FirestoreProperty] public int BuyCount { get; set; }
    [FirestoreProperty] public Timestamp LastBuyTime { get; set; }
}