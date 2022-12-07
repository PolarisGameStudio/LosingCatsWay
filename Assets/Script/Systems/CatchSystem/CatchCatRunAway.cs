using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
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
        uiView.InstantHide();
    }

    public void Cancel()
    {
        CancelEvent?.Invoke();
        CancelEvent = null;
        uiView.InstantHide();
    }
}
