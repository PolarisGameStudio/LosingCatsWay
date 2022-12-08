using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class G_EIGHT_DebugTool : MonoBehaviour
{
    [Button]
    public void Create250()
    {
        List<CreateTemplate> createTemplates = GetCreateTemplates();

        for (int i = 0; i < createTemplates.Count; i++)
        {
            CreateTemplate createTemplate = createTemplates[i];
            string variety = createTemplate.Variety;
            
            for (int j = 0; j < createTemplate.CatCount; j++)
            {
                string hasHat = String.Empty;

                if (j < createTemplate.HatCount)
                    hasHat = "SantaHat";
                
                CreateCat(variety, hasHat, "Location0");
            }
        }
    }

    [Button]
    public void Create8()
    {
        List<CreateTemplate> createTemplates = new List<CreateTemplate>();
        createTemplates.Add(new CreateTemplate("LB_Tabby", 0, 1));
        createTemplates.Add(new CreateTemplate("G_Mackerel", 0, 1));
        createTemplates.Add(new CreateTemplate("DB_Tabby_O", 0, 1));
        createTemplates.Add(new CreateTemplate("P_Calico", 0, 1));
        createTemplates.Add(new CreateTemplate("K_Tortoiseshel", 0, 1));
        createTemplates.Add(new CreateTemplate("P_Black", 0, 1));
        createTemplates.Add(new CreateTemplate("G_American", 0, 1));
        createTemplates.Add(new CreateTemplate("Siamese_4", 0, 1));

        for (int i = 0; i < createTemplates.Count; i++)
        {
            CreateTemplate createTemplate = createTemplates[i];
            string variety = createTemplate.Variety;
            
            for (int j = 0; j < createTemplate.CatCount; j++)
            {
                string hasHat = String.Empty;

                if (j < createTemplate.HatCount)
                    hasHat = "SantaHat";
                
                CreateCat(variety, hasHat, "玩家ID");
            }
        }
    }

    private void CreateCat(string variety, string useSkinId, string owner)
    {
        CloudCatData cloudCatData = new CloudCatData();

        CloudSave_CatData catData = new CloudSave_CatData();
        catData.CatId = FirebaseFirestore.DefaultInstance.Collection("Cats").Document().Id;
        catData.CatName = "G8超棒";
        catData.Sex = (Random.value > .5f) ? (byte)1 : (byte)0;
        catData.Variety = variety;
        catData.Owner = owner;
        catData.BodyScale = Random.Range(0.9f, 1.1f);
        catData.PersonalityTypes = new List<int>(GetRandomPersonality());
        catData.PersonalityLevels = new List<int>(GetPersonalityLevel(catData.PersonalityTypes));
        catData.Trait = GetRandomTrait();
        catData.DeathTime = new Timestamp();
        catData.IsFavorite = false;
        catData.BornTime = Timestamp.FromDateTime(Timestamp.GetCurrentTimestamp().ToDateTime() - TimeSpan.FromDays(5));
        catData.ChipId = String.Empty;

        CloudSave_CatSkinData catSkinData = new CloudSave_CatSkinData();
        if (IsPurebred(catData.Variety))
            GeneratePurebredCatSkinId(catSkinData);
        else
            GenerateMixedCatSkinId(catSkinData);

        catSkinData.UseSkinId = useSkinId;

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
        catDiaryData.AdoptTimestamp = Timestamp.GetCurrentTimestamp();
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
        docRef.SetAsync(cloudCatData);
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
    
    private bool IsPurebred(string variety)
    {
        return Enum.IsDefined(typeof(PurebredCatType), variety);
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
    
    private List<CreateTemplate> GetCreateTemplates()
    {
        List<CreateTemplate> createTemplates = new List<CreateTemplate>();
        
        createTemplates.Add(new CreateTemplate("White_Special", 3, 3));
        createTemplates.Add(new CreateTemplate("GT_Siamese", 2, 2));
        createTemplates.Add(new CreateTemplate("G_American_O", 2, 2));
        createTemplates.Add(new CreateTemplate("Siamese_1", 2, 2));
        createTemplates.Add(new CreateTemplate("Siamese_2", 1, 1));
        createTemplates.Add(new CreateTemplate("Siamese_3", 1, 1));
        createTemplates.Add(new CreateTemplate("CT_Siamese", 1, 1));
        createTemplates.Add(new CreateTemplate("PW_Mackerel_O", 1, 1));
        createTemplates.Add(new CreateTemplate("P_C_Tabby", 1, 1));
        createTemplates.Add(new CreateTemplate("P_P_Calico", 1, 1));
        createTemplates.Add(new CreateTemplate("PW_Mackerel", 0, 8));
        createTemplates.Add(new CreateTemplate("PW_Tabby_O", 0, 8));
        createTemplates.Add(new CreateTemplate("PW_Tabby", 0, 8));
        createTemplates.Add(new CreateTemplate("P_PW_Tabby", 0, 8));
        createTemplates.Add(new CreateTemplate("P_PT_Calico", 0, 8));
        createTemplates.Add(new CreateTemplate("C_Mackerel_O", 0, 8));
        createTemplates.Add(new CreateTemplate("C_Tabby_O", 0, 8));
        createTemplates.Add(new CreateTemplate("C_Mackerel", 0, 8));
        createTemplates.Add(new CreateTemplate("C_Tabby", 0, 8));
        createTemplates.Add(new CreateTemplate("YB_Mackerel_O", 0, 8));
        createTemplates.Add(new CreateTemplate("YB_Tabby_O", 0, 8));
        createTemplates.Add(new CreateTemplate("YB_Mackerel", 0, 8));
        createTemplates.Add(new CreateTemplate("YB_Tabby", 0, 8));
        createTemplates.Add(new CreateTemplate("Cow", 0, 8));
        createTemplates.Add(new CreateTemplate("PT_Calico", 0, 8));
        createTemplates.Add(new CreateTemplate("T_Calico", 0, 8));
        createTemplates.Add(new CreateTemplate("P_Tortoiseshell", 0, 8));
        createTemplates.Add(new CreateTemplate("P_Tortoiseshell_O", 0, 8));
        createTemplates.Add(new CreateTemplate("G_Benz", 0, 8));
        createTemplates.Add(new CreateTemplate("Gray", 0, 8));
        createTemplates.Add(new CreateTemplate("Benz", 0, 8));
        createTemplates.Add(new CreateTemplate("K_Calico", 0, 8));
        createTemplates.Add(new CreateTemplate("G_Mackerel_O", 0, 8));
        createTemplates.Add(new CreateTemplate("G_Tabby", 0, 8));
        createTemplates.Add(new CreateTemplate("T_Tortoiseshell_O", 0, 8));
        createTemplates.Add(new CreateTemplate("T_Tortoiseshell", 0, 7));
        createTemplates.Add(new CreateTemplate("DG_Tabby_O", 0, 7));
        createTemplates.Add(new CreateTemplate("DG_Mackerel", 0, 7));
        createTemplates.Add(new CreateTemplate("DG_Tabby", 0, 7));
        createTemplates.Add(new CreateTemplate("Black", 0, 7));

        return createTemplates;
    }
}

public class CreateTemplate
{
    public CreateTemplate(string variety, int hatCount, int catCount)
    {
        Variety = variety;
        HatCount = hatCount;
        CatCount = catCount;
    }

    public string Variety;
    public int HatCount;
    public int CatCount;
}