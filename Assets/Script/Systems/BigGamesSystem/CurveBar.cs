using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurveBar : MonoBehaviour
{
    [Title("Bar")]
    [SerializeField] private Image barFill;

    [Title("Pointer")]
    [SerializeField] private Image pointerImage;
    [SerializeField] private Sprite enterSprite;
    [SerializeField] private Sprite exitSprite;
    [SerializeField] private GameObject missObject;
    [SerializeField] private RectTransform thunder;
    [SerializeField] private float thunderDuration;
    [SerializeField] private Ease thunderEase;
    [SerializeField] private RectTransform rotateRect;
    [SerializeField] private float targetAngle;
    
    [Title("Speed")]
    [SerializeField] private float pointerSpeed;
    
    private Sequence clockwiseSeq;
    private Sequence unclockwiseSeq;

    private float startPercent, endPercent;

    private bool isClockwisePause;

    #region Basic

    private void LateUpdate()
    {
        if (CheckPointerInArea())
        {
            pointerImage.sprite = enterSprite;
        }
        else
        {
            pointerImage.sprite = exitSprite;
        }
    }

    #endregion

    #region Method

    /// Set pointer hit area and rotate to center.
    public void SetFillSize(float fillSize)
    {
        //FillAmount
        float size = Mathf.Abs(targetAngle * 2) / 360 * fillSize;
        //barFill.fillAmount = size;
        barFill.DOFillAmount(size, 0.45f).From(0).SetEase(Ease.OutExpo).SetDelay(0.25f);

        //Rotation
        float angle = 360f * size / 2;
        barFill.rectTransform.localRotation = Quaternion.Euler(0, 0, -angle);

        //Calculate area for pointer
        float outSize = 1f - fillSize; //總外圍
        startPercent = outSize / 2; //左
        endPercent = startPercent + fillSize; //右
    }

    public void SetFillSize(float fillSize, bool toLeft) // 安全區偏左
    {
        //FillAmount
        float size = Mathf.Abs(targetAngle * 2) / 360 * fillSize;
        //barFill.fillAmount = size;
        barFill.DOFillAmount(size, 0.45f).From(0).SetEase(Ease.OutExpo).SetDelay(0.25f);

        //Rotation
        float angle = 360f * size / 2;
        
        if (toLeft)
        {
            barFill.rectTransform.localRotation = Quaternion.Euler(0, 0, -angle * 3);
        }
        else
        {
            barFill.rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
        }

        //Calculate area for pointer
        if (toLeft)
        {
            float currentAngle = -angle * 3;
            startPercent = (1f - (currentAngle / -targetAngle)) / 2;
            endPercent = startPercent + fillSize;
        }
        else
        {
            startPercent = (angle + targetAngle) / Mathf.Abs(targetAngle * 2);
            endPercent = startPercent + fillSize;
        }
    }

    //Pointer
    public void Rotate(bool toLeft = true, bool autoBreak = false, bool autoRestart = false, bool resetPointer = true, bool randomizeSpeed = false)
    {
        if (resetPointer)
            ResetPointer(!toLeft);

        if (clockwiseSeq != null && clockwiseSeq.IsPlaying())
        {
            clockwiseSeq.Kill();
            clockwiseSeq = null;
        }
        if (unclockwiseSeq != null && unclockwiseSeq.IsPlaying())
        {
            unclockwiseSeq.Kill();
            unclockwiseSeq = null;
        }

        clockwiseSeq = DOTween.Sequence();
        unclockwiseSeq = DOTween.Sequence();

        float speedMultiply = toLeft ? GetPointerPercentByAngle() : 1f - GetPointerPercentByAngle();

        clockwiseSeq
            .Append(rotateRect.DORotate(new Vector3(0, 0, -targetAngle), pointerSpeed * speedMultiply)
                .SetEase(Ease.Linear))
            .SetSpeedBased()
            .OnStepComplete(() =>
            {
                clockwiseSeq.timeScale = 1;
                if (randomizeSpeed)
                    if (Random.value <= 0.3f)
                        clockwiseSeq.timeScale = 0.75f;
                    else if (Random.value <= 0.6f)
                        clockwiseSeq.timeScale = 1.5f;
            });

        unclockwiseSeq
            .Append(rotateRect.DORotate(new Vector3(0, 0, targetAngle), pointerSpeed * speedMultiply)
                .SetEase(Ease.Linear))
            .SetSpeedBased()
            .OnStepComplete(() =>
            {
                unclockwiseSeq.timeScale = 1;
                if (randomizeSpeed)
                    if (Random.value <= 0.3f)
                        unclockwiseSeq.timeScale = 0.75f;
                    else if (Random.value <= 0.6f)
                        unclockwiseSeq.timeScale = 1.5f;
            });

        clockwiseSeq.Pause();
        unclockwiseSeq.Pause();
        
        if (!autoBreak)
        {
            clockwiseSeq.OnComplete(() => unclockwiseSeq.Play());
            unclockwiseSeq.OnComplete(() => clockwiseSeq.Play());

            clockwiseSeq.SetLoops(-1, LoopType.Yoyo);
            unclockwiseSeq.SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            if (autoRestart)
            {
                clockwiseSeq.OnComplete(() =>
                {
                    ResetPointer(false);
                    Rotate(toLeft: true, autoBreak: true, autoRestart: true);
                });
                unclockwiseSeq.OnComplete(() =>
                {
                    ResetPointer(true);
                    Rotate(toLeft: false, autoBreak: true, autoRestart: true);
                });
            }
        }

        if (toLeft)
            clockwiseSeq.Play();
        else
            unclockwiseSeq.Play();
    }

    /// Reset pointer rotation to center.
    public void ResetPointer()
    {
        Quaternion resetAngle = Quaternion.Euler(0, 0, 0);
        rotateRect.localRotation = resetAngle;
    }

    /// Reset pointer rotation to target angle.
    public void ResetPointer(bool isLeft)
    {
        if (isLeft)
        {
            Quaternion resetAngle = Quaternion.Euler(0, 0, -targetAngle);
            rotateRect.localRotation = resetAngle;
        }
        else
        {
            Quaternion resetAngle = Quaternion.Euler(0, 0, targetAngle);
            rotateRect.localRotation = resetAngle;
        }
    }

    public void PointerPause()
    {
        if (clockwiseSeq != null) 
            if (clockwiseSeq.IsPlaying())
            {
                clockwiseSeq.Pause();
                isClockwisePause = true;
            }
        if (unclockwiseSeq != null) 
            if (unclockwiseSeq.IsPlaying())
            {
                unclockwiseSeq.Pause();
                isClockwisePause = false;
            }
    }

    public void PointerResume()
    {
        if (isClockwisePause) 
            clockwiseSeq.Play();
        else
            unclockwiseSeq.Play();
    }

    public void PointerMiss()
    {
        missObject.SetActive(true);

        thunder.DOScale(Vector3.one, thunderDuration).From(Vector3.zero)
            .SetEase(thunderEase)
            .SetLoops(1, LoopType.Yoyo)
            .OnComplete(() =>
            {
                thunder.DOScale(Vector3.zero, thunderDuration).SetEase(thunderEase);
                missObject.SetActive(false);
            });
    }

    #endregion

    #region Get

    /// Get pointer position by percent.Based on 0 to 1.
    public float GetPointerPercentByAngle()
    {
        float result;

        //Calc result from -angle to angle
        //0.包括負數的部分所以乘以二
        float max = targetAngle * 2;
        //1.取得EDITOR看見的角度
        float rotAngle = (rotateRect.localEulerAngles.z > 180f) ? rotateRect.localEulerAngles.z - 360f : rotateRect.localEulerAngles.z;
        //2.加上預設角度
        float curr = rotAngle + targetAngle;
        //3.算出 角度（X） / 總角度（1） 也就是 0~1
        result = curr / max;

        return result;
    }

    /// Pointer enter area.=
    public bool CheckPointerInArea()
    {
        if (GetPointerPercentByAngle() >= startPercent && GetPointerPercentByAngle() <= endPercent) 
            return true;
        return false;
    }

    /// Pointer reach left.
    public bool CheckPointerReachLeft()
    {
        if (GetPointerPercentByAngle() < 0.01) 
            return true;
        return false;
    }

    /// Pointer reach right.
    public bool CheckPointerReachRight()
    {
        if (GetPointerPercentByAngle() > 0.99) 
            return true;
        return false;
    }

    #endregion
}
