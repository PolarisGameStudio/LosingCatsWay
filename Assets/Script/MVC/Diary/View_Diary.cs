using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_Diary : ViewBehaviour
{
    [SerializeField] private CatFlower catFlower;
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private DiaryDots[] dots;
    [SerializeField] private TextMeshProUGUI catNameText;

    [SerializeField] private Image genderImage;
    [SerializeField] private GameObject[] tagObjects;
    [SerializeField] private TextMeshProUGUI[] tagTexts;

    [TabGroup("Pages")] [SerializeField] private UIView frontPage;
    [TabGroup("Pages")] [SerializeField] private UIView contentPage;
    [TabGroup("Pages")] [SerializeField] private UIView lastPage;

    [TabGroup("Content")] [SerializeField] private TextMeshProUGUI dayText;
    [TabGroup("Content")] [SerializeField] private TextMeshProUGUI monthText;
    [TabGroup("Content")] [SerializeField] private TextMeshProUGUI yearText;
    [TabGroup("Content")] [SerializeField] private TextMeshProUGUI titleText;
    [TabGroup("Content")] [SerializeField] private Image moodImage;
    [TabGroup("Content")] [SerializeField] private TextMeshProUGUI contentText;
    [TabGroup("Content")] [SerializeField] private TextMeshProUGUI byeText;
    
    public override void Init()
    {
        base.Init();
        //App.model.cloister.OnSelectedLosingCatChange += OnSelectedLosingCatChange;
        App.model.diary.OnLosingCatDataChange += OnSelectedLosingCatChange;
        App.model.diary.OnPageIndexChange += OnPageIndexChange;
        App.model.diary.OnSelectedDiaryDataChange += OnSelectedDiaryDataChange;
    }

    public override void Open()
    {
        base.Open();
        catSkin.SetActive(true);
        catFlower.gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        catSkin.SetActive(false);
        catFlower.gameObject.SetActive(false);
    }

    private void OnPageIndexChange(object value)
    {
        int index = (int)value;
        
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].Active(false);
        }

        dots[index + 1].Active(true);

        if (index < 0)
        {
            lastPage.InstantHide();
            contentPage.InstantHide();
            frontPage.Show();
        }
    }

    private void OnSelectedLosingCatChange(object value)
    {
        var data = (CloudLosingCatData)value;
        catFlower.ChangeSkin(data);
        catFlower.DoAnimation(data.CatDiaryData.UsedFlower);
        catSkin.ChangeSkin(data);

        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].gameObject.SetActive(false);
        }

        int count = data.CatDiaryData.DiaryDatas.Count + 1; //First page
        for (int i = 0; i < count; i++)
        {
            dots[i].gameObject.SetActive(true);
        }

        catNameText.text = data.CatData.CatName;
        genderImage.sprite = App.factory.catFactory.GetCatSexSpriteEW(data.CatData.Sex);
        
        //Tag
        List<string> keys = new List<string>();
        for (int i = 0; i < data.CatData.PersonalityTypes.Count; i++)
        {
            int type = data.CatData.PersonalityTypes[i];
            int level = data.CatData.PersonalityLevels[i];
            string key = $"{type}{level}";
            keys.Add(key);
        }

        for (int i = 0; i < tagObjects.Length; i++)
        {
            if (i >= keys.Count)
                tagObjects[i].SetActive(false);
            else
            {
                tagObjects[i].SetActive(true);
                tagTexts[i].text = "#" + App.factory.stringFactory.GetPersonality(keys[i]);
            }
        }
    }

    private void OnSelectedDiaryDataChange(object value)
    {
        CloudSave_DiaryData data = (CloudSave_DiaryData)value;
        DateTime diaryDate = data.DiaryDate.ToDateTime();
        string diaryId = data.DiaryId;

        if (diaryId[0] != '7') //Middle pages
        {
            frontPage.InstantHide();
            contentPage.InstantHide();
            lastPage.InstantHide();
            
            //DateUI
            dayText.text = diaryDate.Day.ToString("00");
            monthText.text = diaryDate.Month.ToString();
            yearText.text = diaryDate.Year.ToString();
        
            //Tone
            string[] tmpStrings = diaryId.Split('_');
            string id = tmpStrings[0];
            string tone = tmpStrings[1];
            
            //Title
            titleText.text = App.factory.stringFactory.GetDiaryTitleByTone(id, tone);
            //Content
            contentText.text = App.factory.stringFactory.GetDiaryContentByTone(id, tone);

            contentPage.Show();
        }
        else //Last page
        {
            contentPage.InstantHide();
            
            string[] tmpStrings = diaryId.Split('_');
            string id = tmpStrings[0];
            string tone = tmpStrings[1];
            
            //Bye
            byeText.text = App.factory.stringFactory.GetDiaryContentByTone(id, tone);
            lastPage.Show();
        }
    }
}
