using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using AnimationState = Spine.AnimationState;
using Random = UnityEngine.Random;

public class BigGame_Teaser : BigGameBehaviour
{
    #region Variables

    [Title("Return")]
    [SerializeField] private CurveBar curveBar;

    [PropertyRange(0, 0.5f)] [SerializeField]
    private float fillSize;

    [SerializeField] private Button returnButton;

    [Title("Swing")] 
    [SerializeField] private GameObject swingBar;
    [SerializeField] private Image swingFill;

    [HorizontalGroup("Spam", 0.5f)] [SerializeField]
    private float minSwingValue, maxSwingValue;

    [SerializeField] private GameObject[] swingButtons;

    [SerializeField] private TextMeshProUGUI turnText;

    [Title("UI")]
    [SerializeField] private RectTransform pauseRect;
    [SerializeField] private RectTransform howRect;
    [SerializeField] private Image pauseBg;
    [SerializeField] private RectTransform pauseMenuRect;

    [Title("DoTween")]
    [SerializeField] private Vector2 pauseOrigin;
    [SerializeField] private Vector2 howOrigin;
    [SerializeField] private RectTransform turnRect;
    [SerializeField] private Vector2 turnOrigin;

    private int hitCount; //成功貓玩次數
    private int turn;

    #endregion

    #region Spine

    [Title("Spine")] public SkeletonGraphic catSkeleton;
    public SkeletonGraphic stickSkeleton;

    #endregion

    #region Override

    public override void Open()
    {
        ResetTween();
        base.Open();
    }

    protected override void Init()
    {
        base.Init();

        chance = hearts.Length;
        hitCount = 0;
        turn = 0;
        RefreshTurnText();

        TweenIn();
        StartSwingBar();
    }

    #endregion

    #region Method

    public void Swing(int leftOrRight) // 0:Left 1:Right
    {
        App.system.soundEffect.Play("Button");
        VibrateExtension.Vibrate(VibrateType.Nope);

        if (leftOrRight == 0)
            stickSkeleton.AnimationState.SetAnimation(0, "StickHIT2", false);
        else
            stickSkeleton.AnimationState.SetAnimation(0, "StickHIT", false);

        stickSkeleton.AnimationState.AddAnimation(0, "StickIDE", true, 0);
        SetCatNoticeStick();

        App.system.soundEffect.Play("ED00031");

        float randomValue = Random.Range(minSwingValue, maxSwingValue);
        swingFill.fillAmount += randomValue;

        if (swingFill.fillAmount >= 1f)
            StartReturnBar();
    }

    private void StartSwingBar()
    {
        App.system.soundEffect.Play("ED00041");
        turn++;
        RefreshTurnText();
        
        swingBar.SetActive(true);
        swingBar.transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutBack).From(Vector2.zero);

        swingFill.fillAmount = 0;

        for (int i = 0; i < swingButtons.Length; i++)
        {
            swingButtons[i].SetActive(true);
            swingButtons[i].transform.DOScale(Vector2.one, 0.5f).From(Vector2.zero).SetEase(Ease.OutBack)
                .SetDelay(i * 0.25f);
        }

        //Return
        curveBar.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(false);

        catSkeleton.AnimationState.SetAnimation(0, "CatNotice", true);
        stickSkeleton.AnimationState.SetAnimation(0, "StickIDE", true);

        CancelInvoke(nameof(SwingTimeCount));
        InvokeRepeating(nameof(SwingTimeCount), 0, 0.01f);
    }

    private void SwingTimeCount()
    {
        swingFill.fillAmount -= 0.001f;
    }

    private void StartReturnBar()
    {
        CancelInvoke(nameof(SwingTimeCount));
        swingBar.SetActive(false);

        for (int i = 0; i < swingButtons.Length; i++)
            swingButtons[i].SetActive(false);

        stickSkeleton.AnimationState.SetAnimation(0, "StickWait", true);
        catSkeleton.AnimationState.SetAnimation(0, "CatWait", true);

        curveBar.gameObject.SetActive(true);
        curveBar.transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutBack).From(Vector2.zero);

        curveBar.ResetPointer(false);
        curveBar.SetFillSize(fillSize, true);

        returnButton.gameObject.SetActive(true);
        returnButton.interactable = true;
        returnButton.transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutBack).From(Vector2.zero).SetDelay(0.25f);

        curveBar.Rotate(autoBreak: true);
        InvokeRepeating(nameof(CheckPointerReach), 0f, 0.1f);
    }

    public void Return()
    {
        returnButton.interactable = false;
        returnButton.transform.DOScale(Vector2.zero, 0.15f).SetEase(Ease.InOutSine).SetDelay(0.1f);
        curveBar.PointerPause();

        App.system.soundEffect.Play("ED00031");
        
        if (curveBar.CheckPointerInArea())
            Hit();
        else
            Miss();
    }

    private void Hit()
    {
        CancelInvoke(nameof(CheckPointerReach));

        VibrateExtension.Vibrate(VibrateType.Pop);
        
        catSkeleton.AnimationState.SetAnimation(0, "WinCat", false);
        stickSkeleton.AnimationState.SetAnimation(0, "WinStick", false);
        
        hitCount++;
        App.system.soundEffect.Play("ED00032");

        if (hitCount == 3 || turn == 3)
        {
            DOVirtual.DelayedCall(2f, OpenSettle);
            return;
        }

        DOVirtual.DelayedCall(1f, () =>
        {
            returnButton.gameObject.SetActive(false);
            curveBar.gameObject.SetActive(false);
            StartSwingBar();
        });
    }

    private void Miss()
    {
        returnButton.interactable = false;

        CancelInvoke(nameof(CheckPointerReach));
        
        VibrateExtension.Vibrate(VibrateType.Nope);

        catSkeleton.AnimationState.SetAnimation(0, "LoseCat", false);
        stickSkeleton.AnimationState.SetAnimation(0, "LoseStick", false);

        curveBar.PointerMiss();
        App.system.soundEffect.Play("ED00033");

        hearts[hearts.Length - chance].transform.DOScale(Vector2.zero, 0.25f).SetEase(Ease.InBack).From(Vector2.one);
        chance--;

        if (chance == 0 || turn == 3)
        {
            DOVirtual.DelayedCall(2f, OpenSettle);
            return;
        }

        DOVirtual.DelayedCall(5f, () =>
        {
            returnButton.gameObject.SetActive(false);
            curveBar.gameObject.SetActive(false);
            StartSwingBar();
        });
    }

    public override void Pause()
    {
        base.Pause();
        curveBar.PointerPause();
    }

    public override void Resume()
    {
        base.Resume();
        curveBar.PointerResume();
    }

    public override void OpenPause()
    {
        if (App.system.tutorial.isTutorial)
            return;
        base.OpenPause();
        
        pauseBg.DOFade(1, 0.45f).From(0).OnStart(() =>
        {
            pauseBg.raycastTarget = true;
        });
        pauseMenuRect.DOScale(Vector2.one, 0.35f).From(Vector2.zero).SetEase(Ease.OutBack);
        
        // Pause(); //base
    }

    public override void ClosePause()
    {
        base.ClosePause();
        
        pauseBg.DOFade(0, 0.45f).From(1).OnComplete(() =>
        {
            pauseBg.raycastTarget = false;
        });
        pauseMenuRect.DOScale(Vector2.zero, 0.35f).From(Vector2.one).SetEase(Ease.InBack);
        
        // Resume();
    }

    public void Exit()
    {
        ClosePause();
        Close();
    }

    #endregion

    private void RefreshTurnText()
    {
        turnText.text = turn + "/3";
    }

    #region Invoke

    private void CheckPointerReach()
    {
        if (!curveBar.CheckPointerReachLeft())
            return;
        returnButton.interactable = false;
        returnButton.transform.DOScale(Vector2.zero, 0.15f).SetEase(Ease.InOutSine).SetDelay(0.1f);
        Miss();
    }

    #endregion

    #region Tween

    private void ResetTween()
    {
        curveBar.PointerPause();
        
        catSkeleton.AnimationState.SetAnimation(0, "CatNotice", true);
        stickSkeleton.AnimationState.SetAnimation(0, "StickIDE", true);
        
        returnButton.transform.localScale = Vector2.zero;
        returnButton.gameObject.SetActive(false);
        
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].transform.localScale = Vector2.zero;
        
        Vector2 pauseOffset = new Vector2(pauseOrigin.x, pauseOrigin.y + pauseRect.sizeDelta.y * 2);
        pauseRect.anchoredPosition = pauseOffset;
        Vector2 howOffset = new Vector2(howOrigin.x, howOrigin.y + howRect.sizeDelta.y * 2);
        howRect.anchoredPosition = howOffset;
        
        returnButton.transform.DOScale(Vector2.one, 0.5f).SetEase(Ease.OutBack).From(Vector2.zero).SetDelay(0.25f);

        Vector2 turnOffset = new Vector2(turnOrigin.x, turnOrigin.y + turnRect.sizeDelta.y * 2);
        turnRect.anchoredPosition = turnOffset;

        for (int i = 0; i < swingButtons.Length; i++)
            swingButtons[i].transform.localScale = Vector2.zero;
        
        CancelInvoke("SwingTimeCount");
    }

    private void TweenIn()
    {
        // HeartTween
        for (int i = 0; i < hearts.Length; i++)
            hearts[hearts.Length - (i + 1)].transform.DOScale(Vector2.one, 0.25f)
                .SetDelay(i * 0.125f)
                .SetEase(Ease.OutBack);

        pauseRect.DOAnchorPos(pauseOrigin, 0.15f).SetEase(Ease.OutBack);
        howRect.DOAnchorPos(howOrigin, 0.15f).SetEase(Ease.OutBack).SetDelay(0.0625f);
        turnRect.DOAnchorPos(turnOrigin, 0.25f).SetEase(Ease.OutBack).SetDelay(0.125f);
    }

    #endregion

    #region Spine

    private void SetCatNoticeStick()
    {
        catSkeleton.AnimationState.ClearTracks();
        catSkeleton.AnimationState.SetAnimation(0, "CatStartHIT", false);
        catSkeleton.AnimationState.AddAnimation(0, "CatNotice", true, 0);
    }

    #endregion
}