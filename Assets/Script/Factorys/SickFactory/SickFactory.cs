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

    #region GetSicks

    public string GetMoistureSick(CloudCatData cloudCatData) // 水病
    {
        int ageLevel = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays);
        bool isBoy = cloudCatData.CatData.Sex == 0;
        float percent;
        
        tmpSicks.Clear();
        tmpSicks.AddRange(moistureSicks);
            
        if (ageLevel == 2)
            tmpSicks.AddRange(oldSicks);

        percent = 1f / tmpSicks.Count * 2;
        if (isBoy && Random.value < percent)
            return "SK019";
            
        return tmpSicks.GetRandom();
    }

    public string GetFavourbilitySick(CloudCatData cloudCatData) // 心病
    {
        int ageLevel = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays);
        bool isBoy = cloudCatData.CatData.Sex == 0;
        float percent;
        
        tmpSicks.Clear();
        tmpSicks.AddRange(favourbilitySicks);
            
        if (ageLevel == 2)
            tmpSicks.AddRange(oldSicks);
            
        percent = 1f / tmpSicks.Count * 2;
        if (isBoy && Random.value < percent)
            return "SK019";
            
        return tmpSicks.GetRandom();
    }

    public string GetSatietySick(CloudCatData cloudCatData) //吃病
    {
        int ageLevel = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays);
        bool isBoy = cloudCatData.CatData.Sex == 0;
        float percent;
        
        tmpSicks.Clear();
        tmpSicks.AddRange(satietySicks);
        
        if (ageLevel == 2)
            tmpSicks.AddRange(oldSicks);
            
        percent = 1f / tmpSicks.Count * 2;
        if (isBoy && Random.value < percent)
            return "SK019";
        
        return tmpSicks.GetRandom();
    }

    public string GetVaccineSick()
    {
        if (Random.value < 0.03f)
            return vaccineSicks.GetRandom();

        return string.Empty;
    }

    public string GetLigationSick(CloudCatData cloudCatData)
    {
        if (Random.value < GetLigationSickPercent(cloudCatData.CatData.SurviveDays))
            return vaccineSicks.GetRandom();

        return string.Empty;
    }

    #endregion

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

    public Sprite GetHospitalFunctionSprite(string id)
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
