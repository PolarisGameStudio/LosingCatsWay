using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class BigGame_Meat : BigGameBehaviour
{
    #region Variables

    [SerializeField] private CurveBar curveBar;
    [PropertyRange(0, 1)][SerializeField] private float fillSize;
    [SerializeField] private Image percentFill;
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private TextMeshProUGUI turnText;

    [Title("UI")]
    [SerializeField] private RectTransform pauseRect;
    [SerializeField] private RectTransform howRect;
    [SerializeField] private RectTransform barRect;
    [SerializeField] private Button[] buttons;
    [SerializeField] private Image pauseBg;
    [SerializeField] private RectTransform pauseMenuRect;
    [SerializeField] private RectTransform feedCountRect;

    [Title("Spine")] [SerializeField] private SkeletonGraphic _skeletonGraphic;

    [Title("DoTween")] [SerializeField] private Vector2 barOrigin;
    [SerializeField] private Vector2 pauseOrigin;
    [SerializeField] private Vector2 howOrigin;
    [SerializeField] private Vector2 feedCountOrigin;

    private bool direction = false; //L:true R:false
    private float happy;
    private int feedCount;
    private bool isPointerPause;
    private int turn;

    #endregion

    #region Override

    public override void Open()
    {
        ResetTween();
        base.Open();
    }

    public override void Init()
    {
        base.Init();

        TweenIn();
        PlayWaitMeat();

        curveBar.SetFillSize(fillSize);
        curveBar.ResetPointer();
        direction = true;
        curveBar.Rotate(clockwise: true, autobreak: true, endToStart: true);
        isPointerPause = false;

        percentText.text = "0%";
        percentFill.fillAmount = 0;

        chance = hearts.Length;

        feedCount = 0;
        turn = 0;
        RefreshTurnText();

        CancelInvoke(nameof(CheckStatus));
        ResetPercent();
    }

    #endregion

    #region Method

    public void ToLeft()
    {
        if (feedCount >= 3) return;
        if (chance <= 0) return;
        if (turn >= 3) return;
        if (direction) return;
        if (isPointerPause) return;

        direction = true;
        App.system.soundEffect.Play("Button");
        VibrateExtension.Vibrate(VibrateType.Nope);

        curveBar.Rotate(clockwise: true, autobreak: true, endToStart: true, resetPointer: false);
    }

    public void ToRight()
    {
        if (feedCount >= 3) return;
        if (chance <= 0) return;
        if (turn >= 3) return;
        if (!direction) return;
        if (isPointerPause) return;

        direction = false;
        App.system.soundEffect.Play("Button");
        VibrateExtension.Vibrate(VibrateType.Nope);

        curveBar.Rotate(clockwise: false, autobreak: true, endToStart: true, resetPointer: false);
    }

    void CheckStatus()
    {
        if (curveBar.CheckPointerInArea())
        {
            float origin = happy / 50;
            happy += 5f;
            happy = Mathf.Clamp(happy, 0f, 50f);

            percentText.text = $"{happy * 2}%";
            percentFill.DOFillAmount((happy / 50f), 0.1f).From(origin);

            if (happy >= 50f)
            {
                turn++;
                RefreshTurnText();
                Hit();
            }

            return;
        }

        if (happy > 0)
        {
            float origin = happy / 50;
            happy -= 5f;
            happy = Mathf.Clamp(happy, 0f, 50f);

            percentText.text = $"{happy * 2}%";
            percentFill.DOFillAmount((happy / 50f), 0.5f).From(origin);
        }
        else
        {
            turn++;
            RefreshTurnText();
            Miss();
        }
    }

    private void ResetPercent()
    {
        happy = 20;
        percentText.text = $"{happy * 2}%";
        percentFill.DOFillAmount((happy / 50f), 0.1f);
        InvokeRepeating(nameof(CheckStatus), 0.5f, 0.5f);
    }

    public override void OpenPause()
    {
        if (App.system.tutorial.isTutorial)
            return;
        base.OpenPause();
        curveBar.PointerPause();
        CancelInvoke(nameof(CheckStatus));
        pauseBg.DOFade(1, 0.45f).From(0).OnStart(() =>
        {
            pauseBg.raycastTarget = true;
        });
        pauseMenuRect.DOScale(Vector2.one, 0.35f).From(Vector2.zero).SetEase(Ease.OutBack);
    }

    public override void ClosePause()
    {
        base.ClosePause();
        curveBar.PointerResume();
        InvokeRepeating(nameof(CheckStatus), 0.5f, 0.5f);
        pauseBg.DOFade(0, 0.45f).From(1).OnComplete(() =>
        {
            pauseBg.raycastTarget = false;
        });
        pauseMenuRect.DOScale(Vector2.zero, 0.35f).From(Vector2.one).SetEase(Ease.InBack);
    }

    public void Exit()
    {
        ClosePause();
        CancelInvoke(nameof(CheckStatus));
        Close();
    }

    private void Miss()
    {
        VibrateExtension.Vibrate(VibrateType.Pop);
        
        CancelInvoke(nameof(CheckStatus));
        isPointerPause = true;
        curveBar.PointerPause();
        curveBar.ResetPointer();
        
        hearts[hearts.Length - chance].transform
            .DOScale(Vector3.zero, 0.25f)
            .SetEase(Ease.InBack)
            .From(Vector3.one)
            .OnComplete(() => 
            {
                chance--;
                PlayFailMeat();
            });
    }

    private void Hit()
    {
        VibrateExtension.Vibrate(VibrateType.Nope);
        
        CancelInvoke(nameof(CheckStatus));
        feedCount++;
        isPointerPause = true;
        curveBar.PointerPause();
        curveBar.ResetPointer();
        
        PlayWinMeat();
    }

    private void RefreshTurnText()
    {
        turnText.text = $"{turn}/3";
    }

    #endregion

    #region Spine

    private void PlayWaitMeat()
    {
        _skeletonGraphic.AnimationState.AddAnimation(0, "Wait_MeatPuree", true, 0);
    }
    
    private void PlayWinMeat()
    {
        TrackEntry t = _skeletonGraphic.AnimationState.SetAnimation(0, "Win_MeatPuree", false);
        t.Complete += TrackToWaitMeat;
    }

    private void PlayFailMeat()
    {
        TrackEntry t = _skeletonGraphic.AnimationState.SetAnimation(0, "Fail_MeatPuree", false);
        t.Complete += TrackToWaitMeat;
    }

    private void TrackToWaitMeat(TrackEntry trackentry)
    {
        trackentry.Complete -= TrackToWaitMeat;
        PlayWaitMeat();

        if (feedCount >= 3 || chance <= 0 || turn == 3)
        {
            OpenSettle();
            return;
        }
        
        isPointerPause = false;
        ResetPercent();
        direction = true;
        curveBar.Rotate(clockwise: true, autobreak: true, endToStart: true);
    }

    #endregion
    
    #region Debug

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h < 0) ToLeft();
        if (h > 0) ToRight();
    }

    #endregion

    #region DoTween

    private void ResetTween()
    {
        curveBar.PointerPause();
        curveBar.ResetPointer();
        CancelInvoke(nameof(CheckStatus));
        
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].transform.localScale = Vector2.zero;

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].transform.localScale = Vector2.zero;
        
        Vector2 barOffset = new Vector2(barOrigin.x, barOrigin.y - barRect.sizeDelta.y * 2);
        barRect.anchoredPosition = barOffset;
        
        Vector2 pauseOffset = new Vector2(pauseOrigin.x, pauseOrigin.y + pauseRect.sizeDelta.y * 2);
        pauseRect.anchoredPosition = pauseOffset;
        Vector2 howOffset = new Vector2(howOrigin.x, howOrigin.y + howRect.sizeDelta.y * 2);
        howRect.anchoredPosition = howOffset;

        Vector2 feedCountOffset = new Vector2(feedCountOrigin.x, feedCountOrigin.y + feedCountRect.sizeDelta.y * 2);
        feedCountRect.anchoredPosition = feedCountOffset;
    }

    private void TweenIn()
    {
        // HeartTween
        for (int i = 0; i < hearts.Length; i++)
            hearts[hearts.Length - (i + 1)].transform.DOScale(Vector2.one, 0.25f)
                .SetDelay(i * 0.125f)
                .SetEase(Ease.OutBack);

        //Buttons
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutBack)
                .SetDelay(i * 0.25f);

        pauseRect.DOAnchorPos(pauseOrigin, 0.15f).SetEase(Ease.OutBack);
        howRect.DOAnchorPos(howOrigin, 0.15f).SetEase(Ease.OutBack).SetDelay(0.0625f);
        feedCountRect.DOAnchorPos(feedCountOrigin, 0.25f).SetEase(Ease.OutBack).SetDelay(0.125f);
        barRect.DOAnchorPos(barOrigin, 0.35f).SetEase(Ease.OutBack);
    }

    #endregion
}
