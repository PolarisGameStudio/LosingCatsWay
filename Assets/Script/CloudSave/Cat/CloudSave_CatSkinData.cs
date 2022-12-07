using Firebase.Firestore;

[FirestoreData]
public class CloudSave_CatSkinData
{
    [FirestoreProperty] public bool IsCatTailCurves { get; set; }

    [FirestoreProperty] public int BodyIndex { get; set; }
    [FirestoreProperty] public int BeardIndex { get; set; }
    [FirestoreProperty] public int MouthAndNosesIndex { get; set; }
    [FirestoreProperty] public int MouthMeatIndex { get; set; }
    [FirestoreProperty] public int EarLeftIndex { get; set; }
    [FirestoreProperty] public int EarRightIndex { get; set; }
    [FirestoreProperty] public int LeftFootBehindsIndex { get; set; }
    [FirestoreProperty] public int LeftFootFrontsIndex { get; set; }
    [FirestoreProperty] public int RightFootBehindsIndex { get; set; }
    [FirestoreProperty] public int RightFootFrontsIndex { get; set; }
    [FirestoreProperty] public int TailIndex { get; set; }
    [FirestoreProperty] public int TailCurvesIndex { get; set; }

    [FirestoreProperty] public int EyeTypeIndex { get; set; }
    [FirestoreProperty] public int LeftEyeColorIndex { get; set; }
    [FirestoreProperty] public int RightEyeColorIndex { get; set; }
    
    [FirestoreProperty] public string UseSkinId { get; set; }
}
