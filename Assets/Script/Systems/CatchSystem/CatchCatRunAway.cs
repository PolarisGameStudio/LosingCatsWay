using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class CatchCatRunAway : MvcBehaviour
{
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private UIView uiView;
    [SerializeField] private GameObject infoMask;
    [SerializeField] private GameObject idMask;
    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private GameObject okMask;

    [Title("VIP")] 
    [SerializeField] private GameObject adsButton;
    [SerializeField] private GameObject vipButton;
    
    private Callback OkEvent, CancelEvent;

    public void Active(CloudCatData cloudCatData, Callback okEvent = null, Callback cancelEvent = null)
    {
        catSkin.ChangeSkin(cloudCatData);
        infoMask.SetActive(!cloudCatData.CatHealthData.IsChip);
        idMask.SetActive(!cloudCatData.CatHealthData.IsChip);
        idText.text = (cloudCatData.CatHealthData.IsChip) ? $"ID:{cloudCatData.CatData.CatId}" : "ID:--";
        okMask.SetActive(App.system.catchCat.IsWatchAd);

        OkEvent = okEvent;
        CancelEvent = cancelEvent;

        bool flag = App.system.player.Vip;
        adsButton.SetActive(!flag);
        vipButton.SetActive(flag);
        
        uiView.Show();
    }

    public void CopyId()
    {
        idText.text.CopyToClipboard();
    }

    public void Confirm()
    {
        //TODO AD
        App.system.catchCat.IsWatchAd = true;
        
        OkEvent?.Invoke();
        OkEvent = null;
        Close();
    }

    public void Cancel()
    {
        CancelEvent?.Invoke();
        CancelEvent = null;
        Close();
    }

    public void Close()
    {
        uiView.InstantHide();
    }
}
