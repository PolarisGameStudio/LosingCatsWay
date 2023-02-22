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
    [SerializeField] private GameObject noChipName;
    [SerializeField] private GameObject noChipId;
    
    [HideInInspector] public bool isOpenInfo = false;

    [Title("Callback")] [SerializeField] private UnityEvent OnOpenCallback;
    [SerializeField] private UnityEvent OnCloseCallback;

    public async void SetData(CloudCatData cloudCatData)
    {
        string infoId = cloudCatData.CatData.ChipId;

        noChipName.SetActive(infoId.IsNullOrEmpty());
        noChipId.SetActive(infoId.IsNullOrEmpty());
        infoNameText.gameObject.SetActive(!infoId.IsNullOrEmpty());
        infoIdText.gameObject.SetActive(!infoId.IsNullOrEmpty());

        if (infoId.IsNullOrEmpty())
            return;
        
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
        infoRect.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutExpo);
    }

    public void CloseInfo()
    {
        isOpenInfo = false;
        infoRect.DOScale(Vector2.zero, 0.3f).SetEase(Ease.OutExpo);
    }
}
