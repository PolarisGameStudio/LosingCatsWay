using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Random = UnityEngine.Random;
using Firebase.Firestore;
using Firebase.Auth;

public class DebugTool_Cat
{
    public async Task<CloudCatData> GetCreateCat(string owner, bool isAdult)
    {
        CloudCatData cloudCatData = new CloudCatData();

        CloudSave_CatData catData = new CloudSave_CatData();
        catData.CatId = FirebaseFirestore.DefaultInstance.Collection("Cats").Document().Id;
        catData.CatName = "-";
        catData.Sex = Random.value > .5f ? (byte)1 : (byte)0;
        catData.Variety = GetRandomCatVariety();
        catData.Owner = owner;
        catData.BodyScale = Random.Range(0.9f, 1.1f);
        catData.PersonalityTypes = new List<int>(GetRandomPersonality());
        catData.PersonalityLevels = new List<int>(GetPersonalityLevel(catData.PersonalityTypes));
        catData.Trait = GetRandomTrait();
        catData.DeathTime = new Timestamp();
        catData.IsFavorite = false;
        
        if (isAdult)
            catData.BornTime = Timestamp.FromDateTime(Timestamp.GetCurrentTimestamp().ToDateTime() - TimeSpan.FromDays(5));
        else
            catData.BornTime = Timestamp.GetCurrentTimestamp();

        
        catData.ChipId = String.Empty;

        CloudSave_CatSkinData catSkinData = new CloudSave_CatSkinData();
        if (IsPurebred(catData.Variety))
            GeneratePurebredCatSkinId(catSkinData);
        else
            GenerateMixedCatSkinId(catSkinData);
        catSkinData.UseSkinId = string.Empty;

        CloudSave_CatSurviveData catSurviveData = new CloudSave_CatSurviveData();
        catSurviveData.Satiety = 60;
        catSurviveData.Moisture = 60;
        catSurviveData.Favourbility = 60;
        catSurviveData.RealSatiety = 100;
        catSurviveData.RealMoisture = 100;
        catSurviveData.RealFavourbility = 100;
        catSurviveData.IsUseToFind = false;
        catSurviveData.LikeFoodIndex = Random.Range(0, 3);
        catSurviveData.HateFoodIndex = Random.Range(0, 3);
        catSurviveData.LikeLitterIndex = Random.Range(0, 3);
        catSurviveData.HateLitterindex = Random.Range(0, 3);
        catSurviveData.IsLikeDrink = Random.value > 0.5f;

        CloudSave_CatHealthData catHealthData = new CloudSave_CatHealthData();
        catHealthData.SickId = string.Empty;
        catHealthData.IsLigation = false;
        catHealthData.IsVaccine = false;
        catHealthData.IsChip = false;
        catHealthData.IsBug = false;
        catHealthData.LastMetDoctorTimeStamp = new Timestamp();
        catHealthData.MetDoctorCount = 0;
        catHealthData.NoBugExpireTimestamp = new Timestamp();
        catHealthData.IsMetDoctor = false;

        CloudSave_CatDiaryData catDiaryData = new CloudSave_CatDiaryData();
        catDiaryData.DiaryDatas = new List<CloudSave_DiaryData>();
        catDiaryData.AdoptTimestamp =Timestamp.GetCurrentTimestamp();
        catDiaryData.AdoptLocation = string.Empty;
        catDiaryData.DiarySatietyScore = 0;
        catDiaryData.DiaryLitterScore = 0;
        catDiaryData.DiaryMoistureScore = 0;
        catDiaryData.DiaryFavourbilityScore = 0;
        catDiaryData.UsedFlower = false;
        catDiaryData.FlowerExpiredTimestamp = new Timestamp();

        CloudSave_CatServerData catServerData = new CloudSave_CatServerData();
        catServerData.IsDead = false;

        cloudCatData.CatData = catData;
        cloudCatData.CatSkinData = catSkinData;
        cloudCatData.CatSurviveData = catSurviveData;
        cloudCatData.CatHealthData = catHealthData;
        cloudCatData.CatDiaryData = catDiaryData;
        cloudCatData.CatServerData = catServerData;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Cats").Document(catData.CatId);
        await docRef.SetAsync(cloudCatData);
        return cloudCatData;
    }
    
    public async void CreateCat(string owner, bool isAdult)
    {
        CloudCatData cloudCatData = new CloudCatData();

        CloudSave_CatData catData = new CloudSave_CatData();
        catData.CatId = FirebaseFirestore.DefaultInstance.Collection("Cats").Document().Id;
        catData.CatName = "-";
        catData.Sex = Random.value > .5f ? (byte)1 : (byte)0;
        catData.Variety = GetRandomCatVariety();
        catData.Owner = owner;
        catData.BodyScale = Random.Range(0.9f, 1.1f);
        catData.PersonalityTypes = new List<int>(GetRandomPersonality());
        catData.PersonalityLevels = new List<int>(GetPersonalityLevel(catData.PersonalityTypes));
        catData.Trait = GetRandomTrait();
        catData.DeathTime = new Timestamp();
        catData.IsFavorite = false;
        
        if (isAdult)
            catData.BornTime = Timestamp.FromDateTime(Timestamp.GetCurrentTimestamp().ToDateTime() - TimeSpan.FromDays(5));
        else
            catData.BornTime = Timestamp.GetCurrentTimestamp();

        
        catData.ChipId = String.Empty;

        CloudSave_CatSkinData catSkinData = new CloudSave_CatSkinData();
        if (IsPurebred(catData.Variety))
            GeneratePurebredCatSkinId(catSkinData);
        else
            GenerateMixedCatSkinId(catSkinData);
        catSkinData.UseSkinId = string.Empty;

        CloudSave_CatSurviveData catSurviveData = new CloudSave_CatSurviveData();
        catSurviveData.Satiety = 60;
        catSurviveData.Moisture = 60;
        catSurviveData.Favourbility = 60;
        catSurviveData.RealSatiety = 100;
        catSurviveData.RealMoisture = 100;
        catSurviveData.RealFavourbility = 100;
        catSurviveData.IsUseToFind = false;
        catSurviveData.LikeFoodIndex = Random.Range(0, 3);
        catSurviveData.HateFoodIndex = Random.Range(0, 3);
        catSurviveData.LikeLitterIndex = Random.Range(0, 3);
        catSurviveData.HateLitterindex = Random.Range(0, 3);
        catSurviveData.IsLikeDrink = Random.value > 0.5f;

        CloudSave_CatHealthData catHealthData = new CloudSave_CatHealthData();
        catHealthData.SickId = string.Empty;
        catHealthData.IsLigation = false;
        catHealthData.IsVaccine = false;
        catHealthData.IsChip = false;
        catHealthData.IsBug = false;
        catHealthData.LastMetDoctorTimeStamp = new Timestamp();
        catHealthData.MetDoctorCount = 0;
        catHealthData.NoBugExpireTimestamp = new Timestamp();
        catHealthData.IsMetDoctor = false;

        CloudSave_CatDiaryData catDiaryData = new CloudSave_CatDiaryData();
        catDiaryData.DiaryDatas = new List<CloudSave_DiaryData>();
        catDiaryData.AdoptTimestamp =Timestamp.GetCurrentTimestamp();
        catDiaryData.AdoptLocation = string.Empty;
        catDiaryData.DiarySatietyScore = 0;
        catDiaryData.DiaryLitterScore = 0;
        catDiaryData.DiaryMoistureScore = 0;
        catDiaryData.DiaryFavourbilityScore = 0;
        catDiaryData.UsedFlower = false;
        catDiaryData.FlowerExpiredTimestamp = new Timestamp();

        CloudSave_CatServerData catServerData = new CloudSave_CatServerData();
        catServerData.IsDead = false;

        cloudCatData.CatData = catData;
        cloudCatData.CatSkinData = catSkinData;
        cloudCatData.CatSurviveData = catSurviveData;
        cloudCatData.CatHealthData = catHealthData;
        cloudCatData.CatDiaryData = catDiaryData;
        cloudCatData.CatServerData = catServerData;

        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = db.Collection("Cats").Document(catData.CatId);
        await docRef.SetAsync(cloudCatData);
    }

    #region GetProperties

    private string GetRandomCatVariety()
    {
        Array array = Enum.GetValues(typeof(MixedCatType));

        if (Random.value < 0.1)
        {
            array = Enum.GetValues(typeof(PurebredCatType));
        }

        var result = array.GetValue(Random.Range(0, array.Length)).ToString();

        return result;
    }

    private string GetRandomTrait()
    {
        int[] total = { 60, 30, 10 };
        int index = MathfExtension.RandomRate(total, 100);

        string result = String.Empty;

        switch (index)
        {
            case 0:
                result += 'C' + Random.Range(1, 10).ToString("000");
                break;
            case 1:
                result += 'R' + Random.Range(1, 4).ToString("000");
                break;
            case 2:
                result += 'S' + Random.Range(1, 3).ToString("000");
                break;
        }

        return result;
    }

    private List<int> GetRandomPersonality()
    {
        int[] total = { 35, 35, 30 };
        int count = MathfExtension.RandomRate(total, 100) + 1;

        List<int> personality = new List<int> { 0, 1, 2, 3 };
        personality.Shuffle();

        List<int> result = new List<int>();
        for (int i = 0; i < count; i++)
        {
            result.Add(personality[i]);
        }

        return result;
    }

    private List<int> GetPersonalityLevel(List<int> personality)
    {
        int count = personality.Count;
        List<int> result = new List<int>();
        for (int i = 0; i < count; i++)
        {
            result.Add(Random.Range(0, 4));
        }

        return result;
    }

    private CloudSave_CatSkinData GenerateMixedCatSkinId(CloudSave_CatSkinData catSkinData)
    {
        catSkinData.BodyIndex = Random.Range(0, 5);
        catSkinData.BeardIndex = 0;
        catSkinData.MouthAndNosesIndex = Random.Range(0, 3);
        catSkinData.MouthMeatIndex = Random.Range(0, 3);
        catSkinData.LeftFootBehindsIndex = Random.Range(0, 3);
        catSkinData.LeftFootFrontsIndex = Random.Range(0, 3);
        catSkinData.RightFootBehindsIndex = Random.Range(0, 3);
        catSkinData.RightFootFrontsIndex = Random.Range(0, 3);

        catSkinData.IsCatTailCurves = false;

        catSkinData.TailIndex = Random.Range(0, 2);
        catSkinData.TailCurvesIndex = Random.Range(0, 2);

        //Eye

        catSkinData.EyeTypeIndex = Random.Range(0, 4);
        catSkinData.LeftEyeColorIndex = Random.Range(0, 7);

        if (Random.value >= 0.99f)
            catSkinData.RightEyeColorIndex = Random.Range(0, 7);
        else
            catSkinData.RightEyeColorIndex = catSkinData.LeftEyeColorIndex;

        //Ear

        catSkinData.EarLeftIndex = Random.Range(0, 3);
        catSkinData.EarRightIndex = catSkinData.EarLeftIndex;

        return catSkinData;
    }

    private CloudSave_CatSkinData GeneratePurebredCatSkinId(CloudSave_CatSkinData catSkinData)
    {
        catSkinData.BodyIndex = 0;
        catSkinData.BeardIndex = 0;
        catSkinData.MouthAndNosesIndex = 0;
        catSkinData.MouthMeatIndex = 0;
        catSkinData.EarLeftIndex = 0;
        catSkinData.EarRightIndex = 0;
        catSkinData.LeftFootBehindsIndex = 0;
        catSkinData.LeftFootFrontsIndex = 0;
        catSkinData.RightFootBehindsIndex = 0;
        catSkinData.RightFootFrontsIndex = 0;
        catSkinData.IsCatTailCurves = false;

        catSkinData.TailIndex = Random.Range(0, 2);
        catSkinData.TailCurvesIndex = Random.Range(0, 2);

        //Eye

        catSkinData.EyeTypeIndex = Random.Range(0, 4);
        catSkinData.LeftEyeColorIndex = Random.Range(0, 7);

        if (Random.value >= 0.99f)
            catSkinData.RightEyeColorIndex = Random.Range(0, 7);
        else
            catSkinData.RightEyeColorIndex = catSkinData.LeftEyeColorIndex;

        return catSkinData;
    }

    private bool IsPurebred(string variety)
    {
        return Enum.IsDefined(typeof(PurebredCatType), variety);
    }

    #endregion
}