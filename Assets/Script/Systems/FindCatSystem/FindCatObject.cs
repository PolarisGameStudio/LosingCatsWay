using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class FindCatObject : MvcBehaviour
{
    [SerializeField] private FindCatMap _findCatMap;
    
    public Transform cat;

    public Vector3 showPosition;
    public Vector3 hidePosition;

    [ReadOnly] public bool isShowing = false;

    [SerializeField] private Image outline;
    
    public void Active()
    {
        CancelInvoke("Hide");

        float waitTime = Random.Range(0.5f, 1.5f);
        isShowing = true;

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
        cat.localScale = Vector2.one;
        cat.DOLocalMove(showPosition, 0.25f).From(hidePosition).SetEase(Ease.OutBack);
        outline.DOFade(255f, 0.25f).From(0);
    }
    
    public void Hide()
    {
        // if (isShowing)
        //     Invoke("NextCat", 0.25f);
        
        // isShowing = false;
        cat.DOLocalMove(hidePosition, 0.25f).From(showPosition).SetEase(Ease.InBack).OnComplete(() =>
        {
            if (isShowing)
                Invoke("NextCat", 0.25f);
            
            isShowing = false;
            cat.localScale = Vector2.zero;
        });
        outline.DOFade(0, 0.25f);
    }

    private void NextCat()
    {
        _findCatMap.NextCat();
    }

    public void QuickHide()
    {
        isShowing = false;
        CancelInvoke("NextCat");
        CancelInvoke("Hide");
        cat.DOLocalMove(hidePosition, 0);
        outline.DOFade(0, 0);
    }

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
    private void SetShowPosition()
    {
        RectTransform rt = cat.transform as RectTransform;
        rt.anchoredPosition = showPosition;
    }

    [HorizontalGroup("Set")]
    [Button]
    private void SetHidePosition()
    {
        RectTransform rt = cat.transform as RectTransform;
        rt.anchoredPosition = hidePosition;
    }

    [Button]
    private void ScaleZero()
    {
        cat.localScale = Vector2.zero;
    }

    [Button]
    private void ScaleOne()
    {
        cat.localScale = Vector2.one;
    }
}
