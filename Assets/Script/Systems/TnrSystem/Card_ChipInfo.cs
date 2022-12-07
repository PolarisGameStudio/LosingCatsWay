using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.Common.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Card_ChipInfo : MvcBehaviour
{
    [SerializeField] private RectTransform infoRect;
    [SerializeField] private TextMeshProUGUI infoNameText;
    [SerializeField] private TextMeshProUGUI infoIdText;

    [HideInInspector] public bool isOpenInfo = false;

    [Title("Callback")] [SerializeField] private UnityEvent OnOpenCallback;
    [SerializeField] private UnityEvent OnCloseCallback;

    public async void SetData(CloudCatData cloudCatData)
    {
        string infoId = cloudCatData.CatData.ChipId;

        if (infoId.IsNullOrEmpty())
        {
            infoIdText.text = "無資料：未植入晶片"; //todo
            infoNameText.text = "無資料：未植入晶片"; //TODO
            return;
        }
        
        string infoName = await App.system.cloudSave.LoadOtherPlayerName(cloudCatData.CatData.ChipId);
        infoIdText.text = $"ID:{infoId}";
        infoNameText.text = infoName;
    }

    public void ToggleInfo()
    {
        if (isOpenInfo)
        {
            CloseInfo();
            OnCloseCallback?.Invoke();
        }
        else
        {
            OpenInfo();
            OnOpenCallback?.Invoke();
        }
    }

    public void OpenInfo()
    {
        isOpenInfo = true;
        infoRect.DOScale(Vector2.one, 0.45f).SetEase(Ease.OutBack);
    }

    public void CloseInfo()
    {
        isOpenInfo = false;
        infoRect.DOScale(Vector2.zero, 0.45f).SetEase(Ease.InBack);
    }
}
