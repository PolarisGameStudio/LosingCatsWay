using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;

public class SideMenuSystem : MvcBehaviour
{
    [Title("Container")]
    [SerializeField] private UIView uiView;

    [Title("Panel")] [SerializeField] private RectTransform bgRect;
    [SerializeField] private float duration;

    [Title("DoTween")] [SerializeField] private RectTransform[] buttonRects;

    private Vector2 origin;
    private Vector2 offset;

    public Callback OnOpen;
    /// 沒有任何操作就關掉
    public Callback OnOnlyClose;

    private void Start()
    {
        origin = bgRect.anchoredPosition;
        offset = new Vector2(origin.x + bgRect.sizeDelta.x, origin.y);
        Close();
    }

    public void Open()
    {
        if (App.system.tutorial.isTutorial)
            return;
        
        OnOpen?.Invoke();
        OnOpen = null;
        
        uiView.Show();
        bgRect.DOAnchorPos(origin, duration).SetEase(Ease.OutExpo);
        for (int i = 0; i < buttonRects.Length; i++)
            buttonRects[i].DOScale(Vector2.one, 0.15f).From(Vector2.zero).SetDelay(0.05f * (i + 1));
    }

    private void Close()
    {
        bgRect.DOAnchorPos(offset, duration).SetEase(Ease.InSine);
        DOVirtual.DelayedCall(duration * 0.75f, uiView.Hide);

        OnOnlyClose = null;
    }
    
    public void OnlyClose()
    {
        bgRect.DOAnchorPos(offset, duration).SetEase(Ease.InSine);
        DOVirtual.DelayedCall(duration * 0.75f, uiView.Hide);
        
        OnOnlyClose?.Invoke();
        OnOnlyClose = null;
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
