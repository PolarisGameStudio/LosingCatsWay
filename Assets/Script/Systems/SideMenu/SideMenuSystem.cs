using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SideMenuSystem : MvcBehaviour
{
    [Title("Container")]
    [SerializeField] private UIView uiView;

    [Title("Panel")] [SerializeField] private RectTransform bgRect;
    [SerializeField] private float duration;
    [SerializeField] private Button button;

    [Title("DoTween")] [SerializeField] private RectTransform[] buttonRects;

    private Vector2 origin;
    private Vector2 offset;

    private void Start()
    {
        origin = bgRect.anchoredPosition;
        offset = new Vector2(origin.x + bgRect.sizeDelta.x, origin.y);
        Close();
    }

    public void Open()
    {
        uiView.Show();
        bgRect.DOAnchorPos(origin, duration).SetEase(Ease.OutExpo);
        button.interactable = true;

        for (int i = 0; i < buttonRects.Length; i++)
        {
            buttonRects[i].DOScale(Vector2.one, 0.15f).From(Vector2.zero).SetDelay(0.05f * (i + 1));
        }
    }

    public void Close()
    {
        button.interactable = false;
        bgRect.DOAnchorPos(offset, duration).SetEase(Ease.InSine);
        DOVirtual.DelayedCall(duration * 0.75f, uiView.Hide);
    }

    #region ButtonEvents

    public void OpenArchivement()
    {
        Close();
        App.controller.pedia.OpenArchive();
    }

    public void OpenFirend()
    {
        Close();
        App.controller.friend.Open();
    }

    public void OpenQuest()
    {
        Close();
        App.controller.dailyQuest.Open();
    }

    public void OpenSettings()
    {
        Close();
        App.controller.settings.Open();
    }

    public void OpenGreenHouse()
    {
        Close();
        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            App.view.map.Close();
            App.controller.greenHouse.Open();
        });
    }

    public void OpenPark()
    {
        Close();
        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            App.view.map.Close();
            App.controller.park.Open();
        });
    }

    public void OpenSign()
    {
        Close();
        App.controller.monthSign.Open();
    }

    public void OpenPedia()
    {
        Close();
        App.controller.pedia.OpenPedia();
    }

    public void OpenLobby()
    {
        Close();
        App.system.shortcut.ToLobby();
    }

    #endregion
}
