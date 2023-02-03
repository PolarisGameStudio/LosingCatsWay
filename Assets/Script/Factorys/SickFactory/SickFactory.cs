using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SickFactory : SerializedMonoBehaviour
{
    [Title("SickLevel")] [InfoBox("0:Light 1:Medium 2:Heavy")]
    [SerializeField] private Dictionary<string, int> SickLevels = new Dictionary<string, int>();
    
    [Title("MetCount")] [InfoBox("-1:Worm or MustDead")]
    [SerializeField] private Dictionary<string, int> SickMetCounts = new Dictionary<string, int>();

    [Title("Sprites")] [SerializeField] private Dictionary<string, Sprite> sickSprites;
    [SerializeField] private Dictionary<string, Sprite> clinicSprites;

    private List<string> tmpSicks = new List<string>(); //No more new for performance

    private List<string> vaccineSicks = new List<string>
    {
        "SK001", "SK002", "SK003", "SK004", "SK005"
    };

    private List<string> ligationSicks = new List<string>
    {
        "SK006", "SK007"
    };

    private List<string> oldSicks = new List<string>
    {
        "SK016", "SK017", "SK018", "SK019"
    };

    private List<string> moistureSicks = new List<string>
    {
        "SK008", "SK009", "SK010"
    };

    private List<string> favourbilitySicks = new List<string>
    {
        "SK011"
    };

    private List<string> satietySicks = new List<string>
    {
        "SK012", "SK013", "SK014", "SK015"
    };

    public string GetCatSick(CloudCatData cloudCatData)
    {
        int ageLevel = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays);
        bool isBoy = cloudCatData.CatData.Sex == 0;
        float percent;

        if (ageLevel == 0)
        {
            if (cloudCatData.CatSurviveData.RealSatiety <= 0)
                return "SK011";
            if (cloudCatData.CatSurviveData.RealMoisture <= 0)
                return "SK011";
            if (cloudCatData.CatSurviveData.RealFavourbility <= 0)
                return "SK011";
            return string.Empty;
        }

        if (cloudCatData.CatSurviveData.RealMoisture <= 0)
        {
            tmpSicks.Clear();
            tmpSicks.AddRange(moistureSicks);
            
            if (ageLevel == 2)
                tmpSicks.AddRange(oldSicks);

            percent = 1f / tmpSicks.Count * 2;
            if (isBoy && Random.value < percent)
                return "SK019";
            
            return tmpSicks.GetRandom();
        }

        if (cloudCatData.CatSurviveData.RealFavourbility <= 0)
        {
            tmpSicks.Clear();
            tmpSicks.AddRange(favourbilitySicks);
            
            if (ageLevel == 2)
                tmpSicks.AddRange(oldSicks);
            
            percent = 1f / tmpSicks.Count * 2;
            if (isBoy && Random.value < percent)
                return "SK019";
            
            return tmpSicks.GetRandom();
        }

        if (cloudCatData.CatSurviveData.RealSatiety <= 0)
        {
            tmpSicks.Clear();
            tmpSicks.AddRange(satietySicks);
        
            if (ageLevel == 2)
                tmpSicks.AddRange(oldSicks);
            
            percent = 1f / tmpSicks.Count * 2;
            if (isBoy && Random.value < percent)
                return "SK019";
        
            return tmpSicks.GetRandom();
        }

        return string.Empty; // 無
    }

    public string GetVaccineSicks(CloudCatData cloudCatData)
    {
        int ageLevel = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays);
        bool isVaccine = cloudCatData.CatHealthData.IsVaccine;

        if (isVaccine)
            return string.Empty;
        
        if (Random.value < 0.03f && ageLevel != 2)
            return vaccineSicks.GetRandom();

        return string.Empty;
    }

    public string GetLigationSicks(CloudCatData cloudCatData)
    {
        bool isLigation = cloudCatData.CatHealthData.IsLigation;

        if (isLigation)
            return string.Empty;
        
        if (Random.value < GetLigationSickPercent(cloudCatData.CatData.SurviveDays))
            return vaccineSicks.GetRandom();

        return string.Empty;
    }
    
    public int GetSickLevel(string id)
    {
        return SickLevels[id];
    }

    public int GetMetCount(string id)
    {
        return SickMetCounts[id];
    }

    public Sprite GetSickSprite(string sickId)
    {
        return sickSprites[sickId];
    }

    public Sprite GetClinicSprite(string id)
    {
        return clinicSprites[id];
    }

    private float GetLigationSickPercent(int surviveDays)
    {
        if (surviveDays <= 5)
            return 0;
        if (surviveDays <= 23)
            return 0.03f;
        return 0.05f;
    }
}
