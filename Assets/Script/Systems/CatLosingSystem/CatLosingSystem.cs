using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class CatLosingSystem : MvcBehaviour
{
    [SerializeField] private UIView _uiView;
    [SerializeField] private CatSkin catskin;
    [SerializeField] private TextMeshProUGUI catNameText;

    [Title("DoTween")] 
    [SerializeField] private CanvasGroup letterCanvasGroup;
    [SerializeField] private RectTransform letterRect;
    [SerializeField] private CanvasGroup contentCanvasGroup;
    [SerializeField] private RectTransform paperRect;
    [SerializeField] private CanvasGroup paperCanvasGroup;

    private Cat removeCat;
    
    [Button]
    public void Active(Cat cat)
    {
        removeCat = cat;
        Open(removeCat.cloudCatData);
    }

    public void Open(CloudCatData cloudCatData)
    {
        catskin.ChangeSkin(cloudCatData);
        catNameText.text = cloudCatData.CatData.CatName;
        
        //Letter
        letterCanvasGroup.alpha = 0;
        Vector2 letterOrigin = letterRect.anchoredPosition;
        Vector2 letterOffset = letterRect.anchoredPosition;
        letterOffset.x += 200f;
        
        //Paper
        paperCanvasGroup.alpha = 0;
        Vector2 paperOrigin = paperRect.anchoredPosition;
        Vector2 paperOffset = paperRect.anchoredPosition;
        paperOffset.y -= 200f;
        
        //Content
        contentCanvasGroup.alpha = 0;
        
        _uiView.Show();

        letterCanvasGroup.DOFade(1, 0.45f).From(0).SetEase(Ease.InQuart);
        letterRect.DOAnchorPos(letterOrigin, 0.45f).From(letterOffset).SetEase(Ease.OutExpo);
        letterRect.DORotate(Vector3.zero, 0.45f).From(new Vector3(0, 0, -45f)).SetEase(Ease.OutExpo);
        
        paperCanvasGroup.DOFade(1, 0.5f).From(0).SetDelay(0.5f);
        paperRect.DOAnchorPos(paperOrigin, 0.5f).From(paperOffset).SetEase(Ease.OutExpo).SetDelay(0.5f);

        contentCanvasGroup.DOFade(1, 0.35f).From(0).SetDelay(1f);
    }

    public void AdsRescue()
    {
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            App.system.ads.Active(AdsType.LobbyCatLosing, () =>
            {
                removeCat.cloudCatData.CatSurviveData.Favourbility += 50;
                _uiView.InstantHide();
            });
        });
    }

    [Button]
    public void Close()
    {
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            int randomLocationIndex = Random.Range(0, 2);
            removeCat.cloudCatData.CatData.Owner = "Location" + randomLocationIndex;
            
            App.system.cloudSave.SaveCloudCatData(removeCat.cloudCatData);
            App.system.cat.Remove(removeCat);

            _uiView.InstantHide();
        });
    }
}
