using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public class CloudSave_CatSurviveData
{
    [FirestoreProperty] public float Satiety { get; set; }
    [FirestoreProperty] public float Moisture { get; set; }
    [FirestoreProperty] public float Favourbility { get; set; }
    [FirestoreProperty] public float RealSatiety { get; set; }
    [FirestoreProperty] public float RealMoisture { get; set; }
    [FirestoreProperty] public float RealFavourbility { get; set; }
    [FirestoreProperty] public bool IsUseToFind { get; set; } //找貓使用中，其他人不可用

    [FirestoreProperty] public Timestamp CleanLitterTimestamp { get; set; }
    [FirestoreProperty] public int UsingLitter { get; set; }
    [FirestoreProperty] public int CleanLitterCount { get; set; }
    
    [FirestoreProperty] public int LikeFoodIndex { get; set; }
    [FirestoreProperty] public int HateFoodIndex { get; set; }

    [FirestoreProperty] public int LikeLitterIndex { get; set; }
    [FirestoreProperty] public int HateLitterindex { get; set; }
    
    [FirestoreProperty] public bool IsLikeDrink { get; set; }

    public int HateSnackIndex;
    public int LikeSoupIndex;

    public void ChangeRealSatiety(float value)
    {
        RealSatiety = Mathf.Clamp(RealSatiety + value, 0, 100);
    }
    
    public void ChangeRealMoisture(float value)
    {
        RealMoisture = Mathf.Clamp(RealMoisture + value, 0, 100);
    }
    
    public void ChangeRealFavourbility(float value)
    {
        RealFavourbility = Mathf.Clamp(RealFavourbility + value, 0, 100);
    }
}