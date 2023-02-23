using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using I2.Loc;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailButton : MvcBehaviour
{
    [Title("UI")] 
    public GameObject unSelected;
    public GameObject selected;
    
    public GameObject unReadBg;
    public GameObject isReadBg;

    public TextMeshProUGUI titleText1;
    public TextMeshProUGUI titleText2;
    
    public TextMeshProUGUI dateText1;
    public TextMeshProUGUI dateText2;
    
    public TextMeshProUGUI expiredText1;
    public TextMeshProUGUI expiredText2;

    private int clickIndex = -1;
    
    public void SetStatus(int index) // 0 選取 1 還沒領 2 領了
    {
        unSelected.SetActive(index != 0);
        selected.SetActive(index == 0);
        
        if (index == 0)
            return;
        
        unReadBg.SetActive(index == 1);
        isReadBg.SetActive(index == 2);
    }

    public void SetData(MailData mailData, int index)
    {
        var currentLanguageCode = LocalizationManager.CurrentLanguageCode;

        string title = mailData.Content[currentLanguageCode].Title;
        
        titleText1.text = title;
        dateText1.text = mailData.StartTime.ToDateTime().ToShortDateString();
        
        expiredText1.text = (mailData.EndTime.ToDateTime() - Timestamp.GetCurrentTimestamp().ToDateTime()).Days.ToString();
        
        titleText2.text = titleText1.text;
        dateText2.text = dateText1.text;
        expiredText2.text = expiredText1.text;

        clickIndex = index;
    }

    public void Click()
    {
        print("測試");
        App.system.mail.Select(clickIndex);
    }
}
