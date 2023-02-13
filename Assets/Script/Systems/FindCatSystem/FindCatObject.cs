using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class FindCatObject : MvcBehaviour
{
    [SerializeField] private FindCatMap _findCatMap;
    
    [Title("Cat")]
    public Transform cat;
    public Vector3 showPosition;
    public Vector3 hidePosition;
    [ReadOnly] public bool isShowing;
    
    [Title("Doll")]
    public RectTransform doll;
    [SerializeField] private Vector2 showDollPos;
    [SerializeField] private Vector2 hideDollPos;
    [SerializeField] private Vector2 showDollScale;
    [ReadOnly] public bool isDoll;

    [SerializeField] private Image outline;
    
    public void Active()
    {
        CancelInvoke("Hide");

        float waitTime = Random.Range(0.5f, 1f);
        isShowing = true;
        
        if (_findCatMap.dollCount < 3)
        {
            isDoll = Random.value < 0.4f;
            _findCatMap.dollCount += isDoll ? 1 : 0;
        }
        else
            isDoll = false;

        Show();
        Invoke("Hide", waitTime);
    }

    public void Stop()
    {
        isShowing = false;
        NextCat();

        CancelInvoke("Hide");
        Hide();
    }

    public void Show()
    {
        App.system.soundEffect.Play("ED00042");
        outline.DOFade(1f, 0.25f).From(0);

        if (isDoll)
        {
            doll.localScale = showDollScale;
            doll.DOLocalMove(showDollPos, 0.25f).From(hideDollPos).SetEase(Ease.OutBack);
            return;
        }
        
        cat.localScale = Vector2.one;
        cat.DOLocalMove(showPosition, 0.25f).From(hidePosition).SetEase(Ease.OutBack);
    }
    
    public void Hide()
    {
        App.system.soundEffect.Play("ED00042");
        outline.DOFade(0, 0);
        
        if (isShowing)
            Invoke("NextCat", 0.5f);
        isShowing = false;

        if (isDoll)
        {
            isDoll = false;
            doll.DOLocalMove(hideDollPos, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
            {
                doll.localScale = Vector2.zero;
            });
            return;
        }
            
        cat.DOLocalMove(hidePosition, 0.25f).SetEase(Ease.InBack).OnComplete(() =>
        {
            cat.localScale = Vector2.zero;
        });
    }

    private void NextCat()
    {
        _findCatMap.NextCat();
    }

    public void QuickHide()
    {
        isShowing = false;
        isDoll = false;
        
        CancelInvoke("NextCat");
        CancelInvoke("Hide");
        
        cat.DOLocalMove(hidePosition, 0);
        cat.localScale = Vector2.zero;
        doll.DOLocalMove(hideDollPos, 0);
        doll.localScale = Vector2.zero;
        outline.DOFade(0, 0);
    }

    #region Debug
    
    [HorizontalGroup("Get")]
    [Button]
    private void GetShowPosition()
    {
        RectTransform rt = cat.transform as RectTransform;
        showPosition = rt.anchoredPosition;
    }

    [HorizontalGroup("Get")]
    [Button]
    private void GetHidePosition()
    {
        RectTransform rt = cat.transform as RectTransform;
        hidePosition = rt.anchoredPosition;
    }

    [HorizontalGroup("Set")]
    [Button]
    private void EditorShow()
    {
        RectTransform rt = cat.transform as RectTransform;
        rt.anchoredPosition = showPosition;
        rt.localScale = Vector2.one;
    }

    [HorizontalGroup("Set")]
    [Button]
    private void EditorHide()
    {
        RectTransform rt = cat.transform as RectTransform;
        rt.anchoredPosition = hidePosition;
        rt.localScale = Vector2.zero;
    }

    [HorizontalGroup("DollPos")]
    [Button]
    private void GetDollShow()
    {
        showDollPos = doll.anchoredPosition;
        showDollScale = doll.localScale;
    }

    [HorizontalGroup("DollPos")]
    [Button]
    private void GetDollHide()
    {
        hideDollPos = doll.anchoredPosition;
    }

    [HorizontalGroup("DollEditor")]
    [Button]
    private void EditorDollShow()
    {
        doll.anchoredPosition = showDollPos;
        doll.localScale = showDollScale;
    }

    [HorizontalGroup("DollEditor")]
    [Button]
    private void EditorDollHide()
    {
        doll.anchoredPosition = hideDollPos;
        doll.localScale = Vector2.zero;
    }
    
    #endregion
}
