using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_Cloister : MvcBehaviour
{
    #region Variables
    
    [SerializeField] private CatSkin catSkin;

    [Title("CatInfo")] [SerializeField] private TextMeshProUGUI catNameText;
    [SerializeField] private Image genderImage;

    [Title("Timer")] [SerializeField] private TextMeshProUGUI daysText;
    [SerializeField] private TextMeshProUGUI timeText;

    [TabGroup("Images")]
    [SerializeField] private Image leftImage;
    [TabGroup("Images")]
    [SerializeField] private Image timerImage;
    [TabGroup("Images")]
    [SerializeField] private Image buttonImage;
    [TabGroup("Images")]
    [SerializeField] private Image borderImage;

    [TabGroup("Sprites")]
    [Title("Border")]
    [TabGroup("Sprites")]
    [SerializeField] private Sprite redBorder;
    [TabGroup("Sprites")]
    [Title("Timer")]
    [TabGroup("Sprites")]
    [SerializeField] private Sprite redTimer;
    [TabGroup("Sprites")]
    [Title("Button")]
    [TabGroup("Sprites")]
    [SerializeField] private Sprite redButton;
    [TabGroup("Sprites")]
    [Title("Left")]
    [TabGroup("Sprites")]
    [SerializeField] private Sprite redLeft;
    
    [Title("UsedFlower")] [SerializeField] private GameObject usedIcon;
    [SerializeField] private GameObject usedText;

    private DateTime expiredDate;
    
    #endregion

    public void SetData(CloudLosingCatData losingCatData)
    {
        expiredDate = losingCatData.CatDiaryData.FlowerExpiredTimestamp.ToDateTime().ToLocalTime();
        
        //CatSkin
        catSkin.ChangeSkin(losingCatData);
        
        //Info
        catNameText.text = losingCatData.CatData.CatName;
        genderImage.sprite = App.factory.catFactory.GetCatSexSpriteEW(losingCatData.CatData.Sex);
        
        //Timer
        if (losingCatData.CatDiaryData.UsedFlower)
            CancelInvoke(nameof(CountDown));
        else
            InvokeRepeating(nameof(CountDown), 1f, 1f);
        
        //Used
        bool usedFlower = losingCatData.CatDiaryData.UsedFlower;
        usedIcon.SetActive(usedFlower);
        usedText.SetActive(usedFlower);
    }

    private void CountDown()
    {
        DateTime now = App.system.myTime.MyTimeNow;
        int days = (expiredDate - now).Days;
        int hours = (expiredDate - now).Hours;
        int minutes = (expiredDate - now).Minutes;
        int seconds = (expiredDate - now).Seconds;
        
        daysText.text = days.ToString();
        timeText.text = $"{hours:00}:{minutes:00}:{seconds:00}";

        if ((expiredDate - now).TotalDays < 1) //Under a day
        {
            leftImage.sprite = redLeft;
            timerImage.sprite = redTimer;
            buttonImage.sprite = redButton;
            borderImage.sprite = redBorder;
            return;
        }

        if ((expiredDate - now).TotalSeconds <= 0) //Expired
        {
            SetActive(false);
            App.controller.cloister.Select(-1); //如果在時間到的時候保持選中，則隱藏花
            //TODO DeleteLosingCat
            CancelInvoke(nameof(CountDown));
        }
    }

    public void ReadDiary()
    {
        Select();
        App.controller.cloister.Close();
        App.controller.cloister.OpenDiary();
    }

    public void Select()
    {
        int index = transform.GetSiblingIndex();
        App.controller.cloister.Select(index);
    }

    public void UseFlower()
    {
        Select();
        App.controller.cloister.UseFlower();
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        catSkin.SetActive(active);
    }
}
