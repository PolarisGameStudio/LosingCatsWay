using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class StringFactory : SerializedMonoBehaviour
{
    [Title("Room")] 
    [SerializeField] private Dictionary<string, StringData> RoomNameData;

    [Title("Item")] 
    [SerializeField] private Dictionary<string, StringData> ItemName;
    [SerializeField] private Dictionary<string, StringData> ItemDescription;
    [SerializeField] private Dictionary<string, StringData> UseItemTitle = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> ItemType;

    [Title("Character")] 
    [SerializeField] private Dictionary<string, StringData> CharacterName;

    [Title("CatNotify")]
    [SerializeField] private Dictionary<string, StringData> CatNotifyData = new Dictionary<string, StringData>();

    [Title("Confirm")] 
    [SerializeField] private Dictionary<string, StringData> ConfirmTitleData = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> ConfirmContentData = new Dictionary<string, StringData>();

    [Title("Npc")] 
    [SerializeField] private Dictionary<string, StringData> NpcData = new Dictionary<string, StringData>();

    [Title("Quest")] 
    [SerializeField] private Dictionary<string, StringData> QuestData = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> QuestTitleData = new Dictionary<string, StringData>();

    [Title("Cat")] 
    [SerializeField] private Dictionary<string, string> KittyNames = new Dictionary<string, string>();
    [SerializeField] private Dictionary<string, StringData> CatVarietyData = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> PersonalityData = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> TraitData = new Dictionary<string, StringData>();

    [Title("Unlock")] 
    [SerializeField] private Dictionary<string, StringData> UnlockData = new Dictionary<string, StringData>();

    [Title("Diary")]
    [SerializeField] private Dictionary<string, StringData> DiaryTitle_A = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> DiaryTitle_B = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> DiaryTitle_C = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> DiaryTitle_D = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> DiaryTitle_E = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> DiaryContent_A = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> DiaryContent_B = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> DiaryContent_C = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> DiaryContent_D = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> DiaryContent_E = new Dictionary<string, StringData>();
    
    [Title("Pedia")]
    [SerializeField] private Dictionary<string, StringData> PediaTitle = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> PediaContent = new Dictionary<string, StringData>();

    [Title("Game")]
    [SerializeField] private Dictionary<string, StringData> GameString = new Dictionary<string, StringData>();

    [Title("Gender")]
    [SerializeField] private Dictionary<string, string> boyString = new Dictionary<string, string>();
    [SerializeField] private Dictionary<string, string> girlString = new Dictionary<string, string>();

    [Title("Health")]
    [SerializeField] private Dictionary<string, StringData> sickName;
    [SerializeField] private Dictionary<string, StringData> sickInfo;
    [SerializeField] private Dictionary<string, StringData> HospitalFunctionName;
    [SerializeField] private Dictionary<string, StringData> HospitalFunctionInfo;

    [Title("AgeLevel")]
    [SerializeField] private Dictionary<string, StringData> ageLevelStringDatas;

    [Title("FindCat")]
    [SerializeField] private Dictionary<string, StringData> MapNameData = new Dictionary<string, StringData>();
    [SerializeField] private Dictionary<string, StringData> MapContentData = new Dictionary<string, StringData>();

    [Title("Clinic")]
    [SerializeField] private Dictionary<string, StringData> paymentName;
    [SerializeField] private Dictionary<string, StringData> paymentInfo;

    [Title("CatchCat")]
    [SerializeField] private Dictionary<string, StringData> CatchCatHints;

    [Title("Mall")]
    [SerializeField] private Dictionary<string, StringData> MallItemName;

    private string countryId = "tw";

    #region Properties

    private MyApplication app = null;
    
    private MyApplication App
    {
        get
        {
            if (app == null)
                app = FindObjectOfType<MyApplication>();
            
            return app;
        }
    }

    public string GetCountryByLocaleIndex()
    {
        int index = 0;

        if (App != null)
            index = App.model.settings.LanguageIndex;

        string result = string.Empty;

        switch (index)
        {
            case 0:
                result = "tw";
                break;
            case 1:
                result = "cn";
                break;
            case 2:
                result = "us";
                break;
        }

        return result;
    }

    private string CountryId
    {
        get
        {
            countryId = GetCountryByLocaleIndex();
            return countryId;
        }
    }

    #endregion

    public string GetRoomName(string roomId)
    {
        string result = RoomNameData[CountryId].Contents.ContainsKey(roomId)
            ? RoomNameData[CountryId].Contents[roomId]
            : "要叫阿邦上";
        return result;
    }

    public string GetItemName(string itemId)
    {
        string result = ItemName[CountryId].Contents.ContainsKey(itemId)
            ? ItemName[CountryId].Contents[itemId]
            : "要叫阿邦上";
        return result;
    }

    public string GetItemDescription(string itemId)
    {
        string result = ItemDescription[CountryId].Contents.ContainsKey(itemId)
            ? ItemDescription[CountryId].Contents[itemId]
            : "要叫阿邦上";
        return result;
    }

    public string GetUseItemTitle(string itemId)
    {
        string result = UseItemTitle[CountryId].Contents.ContainsKey(itemId)
            ? UseItemTitle[CountryId].Contents[itemId]
            : "要叫阿邦上";
        return result;
    }

    public string GetItemTypeString(string id)
    {
        string result = ItemType[CountryId].Contents.ContainsKey(id)
            ? ItemType[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetCharacterName(string index)
    {
        return CharacterName[CountryId].Contents[index];
    }

    public string GetCatNotify(string id)
    {
        string result = CatNotifyData[CountryId].Contents.ContainsKey(id)
            ? CatNotifyData[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetConfirmTitle(string id)
    {
        string result = ConfirmTitleData[CountryId].Contents.ContainsKey(id)
            ? ConfirmTitleData[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetConfirmContent(string id)
    {
        string result = ConfirmContentData[CountryId].Contents.ContainsKey(id)
            ? ConfirmContentData[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetNpcName(string id)
    {
        var t = id + "_Name";
        string result = NpcData[CountryId].Contents.ContainsKey(t)
            ? NpcData[CountryId].Contents[t]
            : "要叫阿邦上";
        return result;
    }

    public string GetNpcContent(string id, int index)
    {
        var t = id + "_Speaker_" + index;
        string result = NpcData[CountryId].Contents.ContainsKey(t)
            ? NpcData[CountryId].Contents[t]
            : "要叫阿邦上";
        return result;
    }

    public string GetQuestContent(string id)
    {
        string result = QuestData[CountryId].Contents.ContainsKey(id)
            ? QuestData[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetQuestTitle(string id)
    {
        string result = QuestTitleData[CountryId].Contents.ContainsKey(id)
            ? QuestTitleData[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    #region Cat

    public string GetKittyName()
    {
        return KittyNames[CountryId];
    }

    public string GetCatVariety(string id)
    {
        string result = (CatVarietyData[CountryId].Contents.ContainsKey(id))
            ? CatVarietyData[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetPersonality(string id)
    {
        string result = (PersonalityData[CountryId].Contents.ContainsKey(id))
            ? PersonalityData[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetTraitString(string id)
    {
        string result = (TraitData[CountryId].Contents.ContainsKey(id))
            ? TraitData[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    #endregion

    public string GetUnlock(string id)
    {
        string result = UnlockData[CountryId].Contents.ContainsKey(id)
            ? UnlockData[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }
    
    #region Dairy

    public string GetDiaryTitleByTone(string id, string tone)
    {
        string result = string.Empty;

        if (tone == "A")
        {
            result = (DiaryTitle_A[CountryId].Contents.ContainsKey(id))
                ? DiaryTitle_A[CountryId].Contents[id]
                : "要叫阿邦上";
            return result;
        }

        if (tone == "B")
        {
            result = (DiaryTitle_B[CountryId].Contents.ContainsKey(id))
                ? DiaryTitle_B[CountryId].Contents[id]
                : "要叫阿邦上";
            return result;
        }

        if (tone == "C")
        {
            result = (DiaryTitle_C[CountryId].Contents.ContainsKey(id))
                ? DiaryTitle_C[CountryId].Contents[id]
                : "要叫阿邦上";
            return result;
        }

        if (tone == "D")
        {
            result = (DiaryTitle_D[CountryId].Contents.ContainsKey(id))
                ? DiaryTitle_D[CountryId].Contents[id]
                : "要叫阿邦上";
            return result;
        }

        result = (DiaryTitle_E[CountryId].Contents.ContainsKey(id))
            ? DiaryTitle_E[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetDiaryContentByTone(string id, string tone)
    {
        string result = string.Empty;

        if (tone == "A")
        {
            result = (DiaryContent_A[CountryId].Contents.ContainsKey(id))
                ? DiaryContent_A[CountryId].Contents[id]
                : "要叫阿邦上";
            return result;
        }

        if (tone == "B")
        {
            result = (DiaryContent_B[CountryId].Contents.ContainsKey(id))
                ? DiaryContent_B[CountryId].Contents[id]
                : "要叫阿邦上";
            return result;
        }

        if (tone == "C")
        {
            result = (DiaryContent_C[CountryId].Contents.ContainsKey(id))
                ? DiaryContent_C[CountryId].Contents[id]
                : "要叫阿邦上";
            return result;
        }

        if (tone == "D")
        {
            result = (DiaryContent_D[CountryId].Contents.ContainsKey(id))
                ? DiaryContent_D[CountryId].Contents[id]
                : "要叫阿邦上";
            return result;
        }

        result = (DiaryContent_E[CountryId].Contents.ContainsKey(id))
            ? DiaryContent_E[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }
    
    #endregion

    public string GetGameString(string id)
    {
        string result = GameString[CountryId].Contents.ContainsKey(id)
            ? GameString[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }
    
    public string GetPediaTitle(string id)
    {
        string result = PediaTitle[CountryId].Contents.ContainsKey(id)
            ? PediaTitle[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }
    
    public string GetPediaContent(string id)
    {
        string result = PediaContent[CountryId].Contents.ContainsKey(id)
            ? PediaContent[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetBoyString()
    {
        return boyString[CountryId];
    }

    public string GetGirlString()
    {
        return girlString[CountryId];
    }

    public string GetAgeLevelString(int surviveDays)
    {
        string id = CatExtension.GetCatAgeLevel(surviveDays).ToString();
        string result = ageLevelStringDatas[CountryId].Contents.ContainsKey(id)
            ? ageLevelStringDatas[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetMapNameString(string id)
    {
        string result = MapNameData[CountryId].Contents.ContainsKey(id)
            ? MapNameData[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetMapContentString(string id)
    {
        string result = MapContentData[CountryId].Contents.ContainsKey(id)
            ? MapContentData[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetSickName(string id)
    {
        string result = sickName[CountryId].Contents.ContainsKey(id)
            ? sickName[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }
    
    public string GetSickInfo(string id)
    {
        string result = sickInfo[CountryId].Contents.ContainsKey(id)
            ? sickInfo[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetPaymentName(string id)
    {
        string result = paymentName[CountryId].Contents.ContainsKey(id)
            ? paymentName[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }
    
    public string GetPaymentInfo(string id)
    {
        string result = paymentInfo[CountryId].Contents.ContainsKey(id)
            ? paymentInfo[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetCatchCatHint(string id)
    {
        string result = CatchCatHints[CountryId].Contents.ContainsKey(id)
            ? CatchCatHints[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetMallItemName(string id)
    {
        string result = MallItemName[CountryId].Contents.ContainsKey(id)
            ? MallItemName[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }

    public string GetHospitalFunctionName(string id)
    {
        string result = HospitalFunctionName[CountryId].Contents.ContainsKey(id)
            ? HospitalFunctionName[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }
    
    public string GetHospitalFunctionInfo(string id)
    {
        string result = HospitalFunctionInfo[CountryId].Contents.ContainsKey(id)
            ? HospitalFunctionInfo[CountryId].Contents[id]
            : "要叫阿邦上";
        return result;
    }
}