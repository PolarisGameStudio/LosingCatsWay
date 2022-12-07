using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatTeeth : MvcBehaviour
{
    public BigGame_Teeth bigGame_Teeth;

    public GameObject selectedObject;
    public GameObject dirtObject;
    public GameObject clickObject;
    public CanvasGroup canvasGroup;

    int startCount;
    RectTransform rectTransform;

    #region Properties

    public int Count
    {
        get;
        set;
    }

    public bool IsClean()
    {
        return Count <= 0;
    }

    #endregion

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Init()
    {
        dirtObject.SetActive(false);
        selectedObject.SetActive(false);
        clickObject.SetActive(false);
    }

    /// Set teeth to dirt.
    public void Active(int count, float tweenDelay = 0.25f)
    {
        Count = count;
        startCount = count;

        dirtObject.SetActive(true);
        clickObject.SetActive(true);

        selectedObject.SetActive(true);
        canvasGroup.DOFade(1, 0.25f).From(0).SetDelay(tweenDelay);
        selectedObject.transform.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetDelay(tweenDelay).SetEase(Ease.OutExpo);
    }

    /// End clean.
    public void Close()
    {
        clickObject.SetActive(false);
        selectedObject.transform.DOScale(Vector2.zero, 0.25f).From(Vector2.one).SetEase(Ease.OutExpo)
            .OnComplete(() => selectedObject.SetActive(false));
    }

    public void Click()
    {
        if (!bigGame_Teeth.IsCanClick())
            return;
        
        VibrateExtension.Vibrate(VibrateType.Nope);

        if (!bigGame_Teeth.IsFocusTeeth(this))
        {
            bigGame_Teeth.currentTeeth = this;
            bigGame_Teeth.MoveFinger(rectTransform);
            return;
        }

        if (!bigGame_Teeth.curveBar.CheckPointerInArea())
        {
            bigGame_Teeth.curveBar.PointerMiss();
            bigGame_Teeth.FingerHurt();
            return;
        }

        bigGame_Teeth.FingerClean();
        Count--;
        canvasGroup.DOFade((1f / startCount) * Count, 0.25f);

        if (IsClean())
        {
            Close();
            DOVirtual.DelayedCall(0.5f, bigGame_Teeth.BackToOrigin);
        }
    }
}
