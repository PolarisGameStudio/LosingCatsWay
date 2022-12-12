using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_ClinicChooseCat : MvcBehaviour
{
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private Image moodImage;
    [SerializeField] private TextMeshProUGUI catNameText;
    [SerializeField] private Image cardBg;
    [SerializeField] private Image cardSelectedBg;
    [SerializeField] private GameObject deadMask;
    [SerializeField] private GameObject blackMask;

    [Title("Sprites")] [SerializeField] private Sprite normalBgSprite;
    [SerializeField] private Sprite normalSelectedBgSprite;
    [SerializeField] private Sprite bugBgSprite;
    [SerializeField] private Sprite bugSelectedBgSprite;
    
    [Title("Selected")]
    [SerializeField] private GameObject border;
    [SerializeField] private GameObject selectedObject;

    [Title("Status")] [SerializeField] private GameObject countTitle;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private GameObject checkObject;
    [SerializeField] private GameObject noneObject;
    [SerializeField] private GameObject questionObject;

    [ReadOnly] public int FunctionIndex = 0;
    
    public void SetData(Cat cat)
    {
        catSkin.ChangeSkin(cat.cloudCatData);

        int mood = CatExtension.GetCatMood(cat.cloudCatData);
        moodImage.sprite = App.factory.catFactory.GetMoodSprite(mood);

        catNameText.text = cat.cloudCatData.CatData.CatName;

        CloudSave_CatHealthData catHealthData = cat.cloudCatData.CatHealthData;
        deadMask.SetActive(catHealthData.MetDoctorCount == -1);
        DateTime noBugExpiredDate = catHealthData.NoBugExpireTimestamp.ToDateTime().ToLocalTime();
        
        ResetUI();
        if (FunctionIndex == 0)
        {
            bool noSick = string.IsNullOrEmpty(catHealthData.SickId);

            if (noSick)
            {
                IsCheck(true);
                blackMask.SetActive(true);
                return;
            }

            if (!catHealthData.IsMetDoctor)
            {
                IsQuestion(true);
                return;
            }

            IsNumber(true);
            countText.text = catHealthData.MetDoctorCount.ToString();
            return;
        }

        if (FunctionIndex == 1)
        {
            if (catHealthData.IsVaccine)
            {
                IsCheck(true);
                blackMask.SetActive(true);
            }
            else
                IsNone(true);
            
            return;
        }

        if (FunctionIndex == 2)
        {
            cardBg.sprite = bugBgSprite;
            cardSelectedBg.sprite = bugSelectedBgSprite;
            
            if (catHealthData.IsBug)
            {
                IsNone(true);
                blackMask.SetActive(true);
                return;
            }

            if (noBugExpiredDate > App.system.myTime.MyTimeNow)
            {
                IsNumber(true);
                int day = (noBugExpiredDate - App.system.myTime.MyTimeNow).Days;
                if (day == 0) // <24H 
                    day = 1;
                countText.text = day.ToString("00");
                return;
            }

            IsNone(true);
            return;
        }

        if (FunctionIndex == 3)
        {
            if (catHealthData.IsChip)
            {
                IsCheck(true);
                blackMask.SetActive(true);
            }
            else
                IsNone(true);
            return;
        }

        if (FunctionIndex == 4)
        {
            if (catHealthData.IsLigation)
            {
                IsCheck(true);
                blackMask.SetActive(true);
            }
            else
                IsNone(true);
        }
    }

    public void SetSelect(bool value)
    {
        border.SetActive(value);
        selectedObject.SetActive(value);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        catSkin.SetActive(active);
    }

    public void ResetUI()
    {
        cardBg.sprite = normalBgSprite;
        cardSelectedBg.sprite = normalSelectedBgSprite;
        blackMask.SetActive(false);
        
        IsNumber(false);
        IsCheck(false);
        IsQuestion(false);
        IsNone(false);
    }
    
    public void IsNumber(bool value)
    {
        countTitle.SetActive(value);
        countText.gameObject.SetActive(value);
    }
    
    public void IsCheck(bool value)
    {
        checkObject.SetActive(value);
    }

    public void IsQuestion(bool value)
    {
        questionObject.SetActive(value);
    }

    public void IsNone(bool value)
    {
        noneObject.SetActive(value);
    }
}
