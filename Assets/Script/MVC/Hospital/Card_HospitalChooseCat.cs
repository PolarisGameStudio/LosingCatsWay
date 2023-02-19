using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_HospitalChooseCat : MvcBehaviour
{
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private Image moodImage;
    [SerializeField] private TextMeshProUGUI catNameText;

    [Title("Bg")]
    [SerializeField] private Image bg;
    [SerializeField] private Image selectBg;
    [SerializeField] private Sprite normalBg;
    [SerializeField] private Sprite normalSelectBg;
    [SerializeField] private Sprite bugBg;
    [SerializeField] private Sprite bugSelectBg;

    [Title("Select")]
    [SerializeField] private GameObject border;
    [SerializeField] private GameObject selectObject;
    
    [Title("UI")]
    [SerializeField] private GameObject deadMask;
    [SerializeField] private GameObject blockMask;
    [SerializeField] private GameObject noObject;
    [SerializeField] private GameObject yesObject;
    [SerializeField] private GameObject questionObject;
    [SerializeField] private GameObject countObject; // 包含其他UI一組
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private GameObject metCountTitle;
    [SerializeField] private GameObject antiWormTitle;

    [HideInInspector] public int functionIndex;
    
    public void SetData(CloudCatData cloudCatData)
    {
        catSkin.ChangeSkin(cloudCatData);
        catNameText.text = cloudCatData.CatData.CatName;
        countText.text = cloudCatData.CatHealthData.MetDoctorCount.ToString("00");
        
        int mood = CatExtension.GetCatMood(cloudCatData);
        moodImage.sprite = App.factory.catFactory.GetMoodSprite(mood);

        ResetObjects();
        
        string sickId = cloudCatData.CatHealthData.SickId;
        bool isMetDoctor = cloudCatData.CatHealthData.IsMetDoctor; // 看過醫生
        functionIndex = App.model.hospital.FunctionIndex; // todo 解耦合
        
        bg.sprite = normalBg;
        selectBg.sprite = normalSelectBg;

        if (sickId is "SK001" or "SK002" && isMetDoctor)
        {
            deadMask.SetActive(true);
            noObject.SetActive(true);
            return;
        }
        
        bool hasWorm = cloudCatData.CatHealthData.IsBug;

        if (functionIndex == 0) // 看診
        {
            if (string.IsNullOrEmpty(sickId) && !hasWorm)
            {
                yesObject.SetActive(true);
                blockMask.SetActive(true);
                return;
            }
            
            if (!isMetDoctor)
            {
                questionObject.SetActive(true);
                return;
            }

            countObject.SetActive(true);
            metCountTitle.SetActive(true);
        }

        if (functionIndex == 1) // 疫苗
        {
            if (cloudCatData.CatHealthData.IsVaccine)
            {
                yesObject.SetActive(true);
                blockMask.SetActive(true);
                return;
            }

            noObject.SetActive(true);
        }

        if (functionIndex == 2) // 預防
        {
            if (hasWorm)
            {
                noObject.SetActive(true);
                blockMask.SetActive(true);
                return;
            }

            DateTime expired = cloudCatData.CatHealthData.NoBugExpireTimestamp.ToDateTime().ToLocalTime();
            if (expired > App.system.myTime.MyTimeNow)
            {
                countObject.SetActive(true);
                antiWormTitle.SetActive(true);
                
                bg.sprite = bugBg;
                selectBg.sprite = bugSelectBg;
                
                int day = (expired - App.system.myTime.MyTimeNow).Days + 1;
                countText.text = day.ToString("00");
                return;
            }

            noObject.SetActive(true);
        }

        if (functionIndex == 3) // 晶片
        {
            if (cloudCatData.CatHealthData.IsChip)
            {
                yesObject.SetActive(true);
                blockMask.SetActive(true);
                return;
            }

            noObject.SetActive(true);
        }

        if (functionIndex == 4) // 結紮
        {
            if (cloudCatData.CatHealthData.IsLigation)
            {
                yesObject.SetActive(true);
                blockMask.SetActive(true);
                return;
            }

            noObject.SetActive(true);
        }
    }

    private void ResetObjects()
    {
        yesObject.SetActive(false);
        noObject.SetActive(false);
        questionObject.SetActive(false);
        countObject.SetActive(false);
        metCountTitle.SetActive(false);
        antiWormTitle.SetActive(false);
        
        blockMask.SetActive(false);
        deadMask.SetActive(false);
    }

    public void SetSelect(bool value)
    {
        border.SetActive(value);
        selectObject.SetActive(value);
        
        if (value)
            border.transform.DOScale(Vector2.one, 0.2f).From(new Vector2(1.1f, 1.1f));
        else
            DOTween.Kill(border.transform, true);
    }

    public void Select()
    {
        int index = transform.GetSiblingIndex();
        App.controller.hospital.SelectCat(index);
    }
}
