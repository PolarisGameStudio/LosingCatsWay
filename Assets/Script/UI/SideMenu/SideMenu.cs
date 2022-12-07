using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;

[RequireComponent(typeof(RectTransform))]
public class SideMenu : MvcBehaviour
{
    [Title("UI")] 
    public UIView view;
    public Image blackBg;
    public RectTransform sideMenu;
    
    public void OpenSideMenu()
    {
        view.InstantShow();
        blackBg.DOFade(0.5f, 0.25f).From(0);
        sideMenu.DOAnchorPosX(0, 0.25f).SetEase(Ease.OutExpo);
    }

    public void CloseSideMenu()
    {
        blackBg.DOFade(0, 0.25f).From(0.5f);
        sideMenu.DOAnchorPosX(sideMenu.sizeDelta.x, 0.25f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            view.InstantHide();
        });
    }

    public void InstantCloseSideMenu()
    {
        view.InstantHide();
    }

    public void OneClickToHome()
    {
        App.system.shortcut.ToLobby();
    }
}
