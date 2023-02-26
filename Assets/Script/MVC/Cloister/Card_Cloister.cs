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

    [TabGroup("Images"), SerializeField] private Image leftImage;
    [TabGroup("Images"), SerializeField] private Image timerImage;
    [TabGroup("Images"), SerializeField] private Image buttonImage;
    [TabGroup("Images"), SerializeField] private Image borderImage;
    [TabGroup("Images"), SerializeField] private Image holeImage;

    [TabGroup("Sprites"), SerializeField] private Sprite redBorder;
    [TabGroup("Sprites"), SerializeField] private Sprite redTimer;
    [TabGroup("Sprites"), SerializeField] private Sprite redButton;
    [TabGroup("Sprites"), SerializeField] private Sprite redLeft;
    [TabGroup("Sprites"), SerializeField] private Sprite redHole;
    
    [Title("UsedFlower")] [SerializeField] private GameObject usedIcon;
    [SerializeField] private GameObject usedText;

    [Title("Select")] [SerializeField] private GameObject selectObject;

    [Title("Texts")]
    [SerializeField] private TextMeshProUGUI[] timerTexts;
    [SerializeField] private Material redTextMat;
    
    private DateTime expiredDate;
    private CloudLosingCatData _losingCatData;
    
    #endregion

    public void SetData(CloudLosingCatData losingCatData)
    {
        expiredDate = losingCatData.ExpiredTimestamp.ToDateTime().ToLocalTime();
        
        //CatSkin
        catSkin.ChangeSkin(losingCatData);
        
        //Info
        catNameText.text = losingCatData.CatData.CatName;
        genderImage.sprite = App.factory.catFactory.GetCatSexSpriteEW(losingCatData.CatData.Sex);
        
        //Timer
        if (losingCatData.LosingCatStatus.Contains("Flower") || losingCatData.LosingCatStatus.Contains("AngelCat") || losingCatData.LosingCatStatus.Contains("First"))
            CancelInvoke(nameof(CountDown));
        else
            InvokeRepeating(nameof(CountDown), 1f, 1f);
        
        //Used
        bool usedFlower = losingCatData.LosingCatStatus.Contains("Flower");
        usedIcon.SetActive(usedFlower);
        usedText.SetActive(usedFlower);

        _losingCatData = losingCatData;
    }

    private void CountDown()
    {
        if (_losingCatData.LosingCatStatus.Contains("Flower") || _losingCatData.LosingCatStatus.Contains("AngelCat") || _losingCatData.LosingCatStatus.Contains("First"))
        {
            CancelInvoke(nameof(CountDown));
            return;
        }
        
        DateTime now = App.system.myTime.MyTimeNow;
        int days = (expiredDate - now).Days;
        int hours = (expiredDate - now).Hours;
        int minutes = (expiredDate - now).Minutes;
        int seconds = (expiredDate - now).Seconds;
        
        daysText.text = days.ToString();
        timeText.text = $"{hours:00}:{minutes:00}:{seconds:00}";
        
        if (expiredDate <= now) //Expired
        {
            SetActive(false);
            App.controller.cloister.Select(-1); //如果在時間到的時候保持選中，則隱藏花
            CancelInvoke(nameof(CountDown));
            
            int index = transform.GetSiblingIndex();
            App.controller.cloister.Remove(index);
            return;
        }

        if ((expiredDate - now).TotalDays < 1) //Under a day
        {
            leftImage.sprite = redLeft;
            timerImage.sprite = redTimer;
            buttonImage.sprite = redButton;
            borderImage.sprite = redBorder;
            holeImage.sprite = redHole;

            for (int i = 0; i < timerTexts.Length; i++)
                timerTexts[i].material = redTextMat;
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

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        catSkin.SetActive(active);
    }

    public void SetSelect(bool value)
    {
        selectObject.SetActive(value);
    }
}
