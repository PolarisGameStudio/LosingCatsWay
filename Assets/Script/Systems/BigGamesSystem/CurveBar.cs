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
    [SerializeField] private float pointerSpeed;

    Sequence clockwiseSeq;
    Sequence unclockwiseSeq;

    float startPercent, endPercent;

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

    /// <summary>
    /// Set pointer hit area and rotate to center.
    /// </summary>
    /// <param name="fillSize"></param>
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
        this.startPercent = outSize / 2; //左
        this.endPercent = this.startPercent + fillSize; //右
    }

    public void SetFillSize(float fillSize, bool toLeft)
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
    public void Rotate(bool clockwise = true, bool autobreak = false, bool endToStart = false, bool resetPointer = true)
    {
        if (resetPointer) ResetPointer(clockwise: !clockwise);

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

        clockwiseSeq.timeScale = 0;
        unclockwiseSeq.timeScale = 0;

        float speedMultiply = (clockwise) ? GetPointerPercentByAngle() : (1f - GetPointerPercentByAngle());

        clockwiseSeq
            .Append(rotateRect.DORotate(new Vector3(0, 0, -targetAngle), pointerSpeed * speedMultiply).SetEase(Ease.Linear)).SetSpeedBased();

        unclockwiseSeq
            .Append(rotateRect.DORotate(new Vector3(0, 0, targetAngle), pointerSpeed * speedMultiply).SetEase(Ease.Linear)).SetSpeedBased();

        if (!autobreak)
        {
            clockwiseSeq.OnComplete(() => unclockwiseSeq.Play());
            unclockwiseSeq.OnComplete(() => clockwiseSeq.Play());

            clockwiseSeq.SetLoops(-1, LoopType.Yoyo);
            unclockwiseSeq.SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            if (endToStart)
            {
                clockwiseSeq.OnComplete(() =>
                {
                    ResetPointer(clockwise: false);
                    Rotate(clockwise: true, autobreak: true, endToStart: true);
                });
                unclockwiseSeq.OnComplete(() =>
                {
                    ResetPointer(clockwise: true);
                    Rotate(clockwise: false, autobreak: true, endToStart: true);
                });
            }
        }

        if (clockwise)
        {
            clockwiseSeq.timeScale = 1;
            clockwiseSeq.Play();
        }
        else
        {
            unclockwiseSeq.timeScale = 1;
            unclockwiseSeq.Play();
        }
    }

    /// <summary>
    /// Reset pointer rotation to center.
    /// </summary>
    public void ResetPointer(bool killSequence = true)
    {
        if (killSequence)
        {
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
        }

        Quaternion resetAngle = Quaternion.Euler(0, 0, 0);
        rotateRect.localRotation = resetAngle;
    }

    /// <summary>
    /// Reset pointer rotation to target angle.
    /// </summary>
    /// <param name="clockwise"></param>
    public void ResetPointer(bool clockwise, bool killSequence = true)
    {
        if (killSequence)
        {
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
        }

        if (clockwise)
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
            if (clockwiseSeq.IsPlaying()) clockwiseSeq.Pause();
        if (unclockwiseSeq != null) 
            if (unclockwiseSeq.IsPlaying()) unclockwiseSeq.Pause();
    }

    [Button(30)]
    public void PointerHit(bool pause = false, bool autoResume = true)
    {
        if (pause) clockwiseSeq.timeScale = 0;
        if (pause) unclockwiseSeq.timeScale = 0;

        if (!autoResume) return;

        DOVirtual.DelayedCall(.1f, () =>
        {
            clockwiseSeq.timeScale = 1;
            unclockwiseSeq.timeScale = 1;
        });
    }

    [Button(30)]
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

    /// <summary>
    /// Get pointer position by percent.Based on 0 to 1.
    /// </summary>
    /// <returns></returns>
    public float GetPointerPercentByAngle()
    {
        float result = 0;

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

    /// <summary>
    /// Pointer enter area.
    /// </summary>
    /// <returns></returns>
    public bool CheckPointerInArea()
    {
        if (GetPointerPercentByAngle() >= startPercent && GetPointerPercentByAngle() <= endPercent) return true;
        else return false;
    }

    /// <summary>
    /// Pointer reach left.
    /// </summary>
    /// <returns></returns>
    public bool CheckPointerReachStartPoint()
    {
        if (GetPointerPercentByAngle() < 0.01) return true;
        return false;
    }

    /// <summary>
    /// Pointer reach right.
    /// </summary>
    /// <returns></returns>
    public bool CheckPointerReachEndPoint()
    {
        if (GetPointerPercentByAngle() > 0.99) return true;
        return false;
    }

    #endregion
}
