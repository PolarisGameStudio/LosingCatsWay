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

        titleText.text = titleText.text.Replace("<playerName>", App.system.player.PlayerName);
        int catTotalCount = 123456;
        contentText.text = contentText.text.Replace("<catName>", firstLosingCat.CatData.CatName).Replace("<catTotalCount>", catTotalCount.ToString("N0"));
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
}
