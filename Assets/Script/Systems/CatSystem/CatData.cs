using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FirestoreData]
public class CatData
{
    //TODO 晶片後才顯示
    [FirestoreProperty] public string CatId { get; set; }
    [FirestoreProperty] public string CatName { get; set; }

    [FirestoreProperty] public int SurviveDays { get; set; } //存活天數
    public int Age { get => SurviveDays / 2; }

    [FirestoreProperty] public bool IsLigation { get; set; } //結紮了嗎

    /// <summary>
    /// 0 : 公, 1 : 母
    /// </summary>
    [FirestoreProperty] public byte Sex { get; set; } // 0：公 1：母
    [FirestoreProperty] public string Variety { get; set; } //品種

    [FirestoreProperty] public string OwnerString { get; set; }

    #region LikeHate

    [FirestoreProperty] public bool IsLikeDrink { get; set; } //愛喝水嗎
    [FirestoreProperty] public int LikeFoodIndex { get; set; } //喜歡的乾糧
    [FirestoreProperty] public int HateFoodIndex { get; set; } //討厭的乾糧

    public int LikeSnackIndex;

    [FirestoreProperty] public int LikeLitterIndex { get; set; } //喜歡的貓砂
    [FirestoreProperty] public int HateLitterIndex { get; set; } //討厭的貓砂

    #endregion

    #region Status

    //Status //表
    [FirestoreProperty] public float Satiety { get; set; }
    [FirestoreProperty] public float Favorability { get; set; } //TODO Fun
    [FirestoreProperty] public float Moisture { get; set; }

    //裏
    [FirestoreProperty] public int RealSatiety { get; set; }
    [FirestoreProperty] public int RealFavorability { get; set; }
    [FirestoreProperty] public int RealMoisture { get; set; }

    #endregion

    #region Health

    [FirestoreProperty] public string SickId { get; set; } //病狀名
    /// <summary>
    /// 0輕 1中 2重
    /// </summary>
    [FirestoreProperty] public int SickLevel { get; set; }
    [FirestoreProperty] public long LastMetDoctorTimeStamp { get; set; } //最後一次看醫生什麼時候
    [FirestoreProperty] public int MetDoctorCount { get; set; } //回診次數
    [FirestoreProperty] public bool IsVaccine { get; set; } //打疫苗了嗎 //拿到就要打 //不然會生病
    [FirestoreProperty] public bool IsChip { get; set; }
    [FirestoreProperty] public bool IsBug { get; set; } //長蟲嗎
    [FirestoreProperty] public int NoBugDays { get; set; } //預防長蟲日
    [FirestoreProperty] public bool IsReadyDead { get; set; } //是否等死
    [FirestoreProperty] public int ReadyDeadDays { get; set; } //距離死亡多少日
    [FirestoreProperty] public bool IsDead { get; set; } //是否已死亡

    #endregion

    //情緒(表情)
    /// <summary>
    /// 0：好 //1：普通 //2：差
    /// </summary>
    [FirestoreProperty] public int Mood { get; set; } //0：好 //1：普通 //2：差
    
    [FirestoreProperty] public List<int> Traits { get; set; } //專長(播動畫用)
    [FirestoreProperty] public float BodyScale { get; set; }

    [FirestoreProperty] public List<int> PersonalityTypes { get; set; }
    [FirestoreProperty] public List<int> PersonalityLevels { get; set; }

    #region CatSkin

    [FirestoreProperty] public bool IsPurebredCat { get; set; }
    [FirestoreProperty] public bool IsCatTailCurves { get; set; }

    [FirestoreProperty] public int CatVarietyIndex { get; set; }

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

    #endregion
}
