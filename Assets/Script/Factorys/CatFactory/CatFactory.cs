using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class CatFactory : SerializedMonoBehaviour
{
    public CatDataSetting catDataSetting;
    [SerializeField] private Sprite[] ew_SexSprites;
    [SerializeField] private Sprite[] white_SexSprites;
    [SerializeField] private Sprite[] moodSprites; //0:Happy 1:Normal 2:Awful
    [InlineEditor(InlineEditorModes.LargePreview)] [SerializeField] private Sprite[] personalitySprites;
    [SerializeField] private Sprite[] personalityLevelSprites;
    [InlineEditor(InlineEditorModes.LargePreview)] [SerializeField] private Sprite[] personalityTipSprites;

    #region GetSprite

    /// <summary>
    /// 0:Male 1:Female
    /// </summary>
    /// <param name="sex"></param>
    /// <returns></returns>
    public Sprite GetCatSexSpriteEW(int sex) //Blue Red Icon
    {
        return ew_SexSprites[sex];
    }

    public Sprite GetCatSexSpriteWhite(int sex)
    {
        return white_SexSprites[sex];
    }

    public Sprite GetMoodSprite(int index)
    {
        return moodSprites[index];
    }

    public Sprite GetPersonalitySprite(int personality)
    {
        return personalitySprites[personality];
    }

    public Sprite GetPersonalityLevelSprite(int level)
    {
        return personalityLevelSprites[level];
    }

    public Sprite GetPersonalityTipsSprite(int personality)
    {
        return personalityTipSprites[personality];
    }

    #endregion

    #region Sick

    public float GetSickPercent(CloudCatData cloudCatData)
    {
        return catDataSetting.sickPercentBySurviveDay_Natural[cloudCatData.CatData.SurviveDays];
    }

    #endregion
}