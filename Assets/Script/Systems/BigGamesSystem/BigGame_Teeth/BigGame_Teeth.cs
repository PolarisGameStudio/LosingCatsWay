using DG.Tweening;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BigGame_Teeth : BigGameBehaviour
{
    #region Variables

    [SerializeField] private Canvas canvas;
    
    [Title("Bar")]
    public CurveBar curveBar;
    [PropertyRange(0, 1)][SerializeField] private float fillSize;
    [SerializeField] private RectTransform barRect;

    [Title("Teeths")]
    [SerializeField] private CatTeeth[] teeths;
    [HorizontalGroup("Teeths", 0.5f)]
    [SerializeField] private int minTeeths, maxTeeths;
    List<CatTeeth> dirtTeeths = new List<CatTeeth>();

    [Title("Finger")]
    [SerializeField] private RectTransform fingerRect;
    [SerializeField] private Vector2 fingerOrigin;
    [SerializeField] private float fingerMoveDuration;
    [SerializeField] private SkeletonGraphic fingerGraphic;

    [Title("Particle")] [SerializeField] private ParticleSystem bubbleParticleSystem;
    [SerializeField] private UIParticle bubbleParticle;
    [SerializeField] private UIParticle bubbleBgParticle;

    [Title("UI")]
    [SerializeField] private RectTransform pauseRect;
    [SerializeField] private RectTransform howRect;
    [SerializeField] private Image pauseBg;
    [SerializeField] private RectTransform pauseMenuRect;
    [SerializeField] private CanvasGroup hurtMask;

    [Title("DoTween")] [SerializeField] private Vector2 curveBarOrigin;
    [SerializeField] private Vector2 curveBarOffset;
    [SerializeField] private Vector2 pauseOrigin;
    [SerializeField] private Vector2 howOrigin;

    private bool isSuccess = false;
    [ReadOnly] public CatTeeth currentTeeth;
    private bool isFingerMove;

    #endregion

    #region Override

    public override void Open()
    {
        ResetTween();
        base.Open();
        bubbleBgParticle.gameObject.SetActive(true);
        bubbleParticle.gameObject.SetActive(true);
        bubbleBgParticle.Play();
        bubbleParticle.Stop();
    }

    public override void Close()
    {
        base.Close();
        bubbleBgParticle.gameObject.SetActive(false);
        bubbleParticle.gameObject.SetActive(false);
        bubbleBgParticle.Stop();
        bubbleParticle.Stop();
    }

    public override void Init()
    {
        base.Init();
        
        for (int i = 0; i < teeths.Length; i++)
        {
            teeths[i].Init(); //重設牙齒
        }

        List<CatTeeth> tmp = new List<CatTeeth>(teeths);
        tmp.Shuffle();

        int teethCount = Random.Range(minTeeths, maxTeeths + 1); //取隨機牙齒數
        dirtTeeths.Clear(); //清空髒牙齒
        for (int i = 0; i < teethCount; i++)
        {
            var teeth = tmp[0];

            teeth.Active(5, i * 0.125f);
            dirtTeeths.Add(teeth);

            tmp.RemoveAt(0);
        }

        isSuccess = false;

        currentTeeth = null;

        chance = hearts.Length;
        
        TweenIn();

        curveBar.SetFillSize(fillSize);
        curveBar.Rotate();
        
        //Bar
        barRect.anchoredPosition = curveBarOffset;

        InvokeRepeating("CheckTeethsClean", 0, 0.1f);
    }

    public void Success()
    {
        if (isSuccess)
            return;

        CancelInvoke("CheckTeethsClean");
        isSuccess = true;

        curveBar.PointerPause();
        curveBar.ResetPointer();
        bubbleParticle.Stop();
        bubbleBgParticle.gameObject.SetActive(false);
        bubbleParticle.gameObject.SetActive(false);
        
        DOVirtual.DelayedCall(1, () =>
        {
            OpenSettle();
        });
    }

    public void Failed()
    {
        CancelInvoke("CheckTeethsClean");

        curveBar.PointerPause();
        curveBar.ResetPointer();
        bubbleParticle.Stop();
        bubbleBgParticle.gameObject.SetActive(false);
        bubbleParticle.gameObject.SetActive(false);
        
        DOVirtual.DelayedCall(1, () =>
        {
            OpenSettle();
        });
    }

    #endregion

    #region Method

    public void MoveFinger(RectTransform rectTransform)
    {
        isFingerMove = true;
        
        barRect.DOKill();
        
        if (barRect.anchoredPosition != curveBarOffset)
            barRect.DOAnchorPos(curveBarOffset, 0.25f).SetEase(Ease.InBack);
        barRect.DOAnchorPos(curveBarOrigin, 0.25f).SetEase(Ease.OutBack)
            .SetDelay(fingerMoveDuration);

        fingerRect.DOAnchorPos(rectTransform.anchoredPosition, fingerMoveDuration).OnComplete(() => isFingerMove = false);
    }

    public bool IsFocusTeeth(CatTeeth catTeeth)
    {
        if (currentTeeth == null)
            return false;
        if (currentTeeth != catTeeth)
            return false;
        return true;
    }

    public bool IsCanClick()
    {
        if (chance <= 0)
            return false;
        if (isSuccess)
            return false;
        if (isFingerMove)
            return false;
        return true;
    }

    public void FingerClean()
    {
        if (chance <= 0)
            return;

        if (!bubbleParticleSystem.IsAlive() && !bubbleParticleSystem.isPlaying)
        {
            bubbleParticle.RefreshParticles();
            bubbleParticle.Play();
        }

        fingerGraphic.AnimationState.SetAnimation(0, "FingerCot/FingerCot_Clean", false);
        fingerGraphic.AnimationState.AddAnimation(0, "FingerCot/FingerCot_Idle", true, 0);
    }

    public void FingerHurt()
    {
        if (chance <= 0)
            return;
        
        chance--;
        
        VibrateExtension.Vibrate(VibrateType.Peek);
        
        canvas.transform.DOShakePosition(0.15f, 10f)
            .OnComplete(() =>
            {
                if (chance <= 0)
                    Failed();
            });
        hurtMask.DOFade(1, 0.15f).From(0).SetLoops(2, LoopType.Yoyo);
        
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].SetActive(true);

        for (int i = 0; i < hearts.Length - chance; i++)
            hearts[i].SetActive(false);

        fingerGraphic.AnimationState.SetAnimation(0, "FingerCot/FingerCot_Fail", false);
        fingerGraphic.AnimationState.AddAnimation(0, "FingerCot/FingerCot_Idle", true, 0);
    }

    public void CheckTeethsClean()
    {
        if (dirtTeeths.Count <= 0)
            return;

        for (int i = 0; i < dirtTeeths.Count; i++)
        {
            if (!dirtTeeths[i].IsClean())
                return;
        }

        Success();
    }

    public override void OpenPause()
    {
        base.OpenPause();
        pauseBg.DOFade(1, 0.45f).From(0).OnStart(() =>
        {
            pauseBg.raycastTarget = true;
        });
        pauseMenuRect.DOScale(Vector2.one, 0.35f).From(Vector2.zero).SetEase(Ease.OutBack);
    }

    public override void ClosePause()
    {
        base.ClosePause();
        pauseBg.DOFade(0, 0.45f).From(1).OnComplete(() =>
        {
            pauseBg.raycastTarget = false;
        });
        pauseMenuRect.DOScale(Vector2.zero, 0.35f).From(Vector2.one).SetEase(Ease.InBack);
    }

    public void Exit()
    {
        ClosePause();
        Close();
    }

    #endregion

    #region Tween

    private void ResetTween()
    {
        curveBar.PointerPause();
        curveBar.ResetPointer();
        
        hurtMask.alpha = 0;
        
        Vector2 fingerOffset = new Vector2(fingerOrigin.x + fingerRect.sizeDelta.x, fingerOrigin.y);
        fingerRect.anchoredPosition = fingerOffset;
        
        barRect.anchoredPosition = curveBarOffset;
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].transform.localScale = Vector2.zero;
        }
        
        Vector2 pauseOffset = new Vector2(pauseOrigin.x, pauseOrigin.y + pauseRect.sizeDelta.y * 2);
        pauseRect.anchoredPosition = pauseOffset;
        Vector2 howOffset = new Vector2(howOrigin.x, howOrigin.y + howRect.sizeDelta.y * 2);
        howRect.anchoredPosition = howOffset;
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

        fingerRect.DOAnchorPos(fingerOrigin, 0.35f).SetEase(Ease.OutBack);
    }

    public void BackToOrigin()
    {
        barRect.DOAnchorPos(curveBarOffset, 0.25f).SetEase(Ease.InBack);
        fingerRect.DOAnchorPos(fingerOrigin, 0.35f).SetEase(Ease.OutBack);
    }

    #endregion
}