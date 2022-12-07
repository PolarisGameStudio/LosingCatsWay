using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public class CloudSave_CatHealthData
{
    [FirestoreProperty] public string SickId { get; set; }
    [FirestoreProperty] public bool IsMetDoctor { get; set; }
    [FirestoreProperty] public bool IsLigation { get; set; }
    [FirestoreProperty] public bool IsVaccine { get; set; }
    [FirestoreProperty] public bool IsChip { get; set; }
    [FirestoreProperty] public bool IsBug { get; set; }
    [FirestoreProperty] public Timestamp LastMetDoctorTimeStamp { get; set; }
    [FirestoreProperty] public int MetDoctorCount { get; set; }
    [FirestoreProperty] public Timestamp NoBugExpireTimestamp { get; set; }
}
