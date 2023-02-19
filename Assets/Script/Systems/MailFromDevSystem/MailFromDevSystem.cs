using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class MailFromDevSystem : MvcBehaviour
{
    [SerializeField] private UIView _uiView;

    [Title("UI")] [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;

    private CloudLosingCatData firstLosingCat;
    private bool isGetFirstDeadCat = false;

    public void Open()
    {
        GetFirstDeadCat();
        _uiView.Show();
    }

    public void Close()
    {
        _uiView.InstantHide();
    }

    private void SetMailContent()
    {
        if (firstLosingCat == null)
            return;

        string adoptLocationKey = firstLosingCat.CatDiaryData.AdoptLocation;
        string title = App.factory.stringFactory.GetMailFromDevContent("Title");
        string content = App.factory.stringFactory.GetMailFromDevContent("Content");
        string catByAdoptLocation = App.factory.stringFactory.GetMailFromDevContent(adoptLocationKey);

        string finalTitle = title.Replace("<PlayerName>", App.system.player.PlayerName);
        string finalContent = content.Replace("<CatName>", firstLosingCat.CatData.CatName)
            .Replace("<CatByAdoptLocation>", catByAdoptLocation)
            .Replace("<TotalCatCount>", GetTotalCatNumber().ToString("N0"));

        titleText.text = finalTitle;
        contentText.text = finalContent;
    }
    
    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }

    private async void GetFirstDeadCat()
    {
        if (isGetFirstDeadCat)
            return;
        
        var losingCats = await App.system.cloudSave.LoadCloudLosingCatDatas(App.system.player.PlayerId);
        
        if (losingCats.Count <= 0)
            return;
        
        firstLosingCat = null;
        for (int i = 0; i < losingCats.Count; i++)
        {
            if (losingCats[i].LosingCatStatus.Contains("First"))
            {
                firstLosingCat = losingCats[i];
                break;
            }
        }
        
        SetMailContent();

        isGetFirstDeadCat = true;
    }

    private int GetTotalCatNumber()
    {
        int result;
        
        if (!PlayerPrefs.HasKey("TotalCatNumber"))
        {
            result = Random.Range(100000, 1000001);
            PlayerPrefs.SetInt("TotalCatNumber", result);
        }
        else
        {
            result = PlayerPrefs.GetInt("TotalCatNumber");
        }

        return result;
    }
}
