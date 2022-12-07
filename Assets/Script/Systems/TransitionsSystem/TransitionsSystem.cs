using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using SpriteShadersUltimate;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TransitionsSystem : MvcBehaviour
{
    [Title("UI")] 
    public UIView view;
    public Image bg;

    public float openTime;
    public float exitTime;

    private float openOriginValue = 5.3f;
    private float closeOriginValue = 16.5f;
    private string propertyName = "_HalftoneFade";

    public void Active(float waitTime, UnityAction coverAction = null, UnityAction endAction = null)
    {
        view.InstantShow();

        float originValue = openOriginValue;
        
        DOTween.To(() => originValue, x => originValue = x, closeOriginValue, openTime).OnUpdate(() =>
        {
            bg.material.SetFloat(propertyName, originValue);
        }).OnComplete(() => { coverAction?.Invoke(); });

        waitTime += openTime;
        DOVirtual.DelayedCall(waitTime, () =>
        {
            originValue = closeOriginValue;

            DOTween.To(() => originValue, x => originValue = x, openOriginValue, exitTime).OnUpdate(() =>
            {
                bg.material.SetFloat(propertyName, originValue);
            }).OnComplete(() =>
            {
                view.InstantHide();
                endAction?.Invoke();
            });
        });
    }

    public void OnlyOpen(UnityAction endAction = null)
    {
        view.InstantShow();

        float originValue = openOriginValue;

        DOTween.To(() => originValue, x => originValue = x, closeOriginValue, openTime).OnUpdate(() =>
        {
            bg.material.SetFloat(propertyName, originValue);
        }).OnComplete(() => { endAction?.Invoke(); });
    }

    public void OnlyClose()
    {
        float originValue = closeOriginValue;

        DOTween.To(() => originValue, x => originValue = x, openOriginValue, exitTime).OnUpdate(() =>
        {
            bg.material.SetFloat(propertyName, originValue);
        }).OnComplete(() => { view.InstantHide(); });
    }

    public void InstantShow()
    {
        view.InstantShow();
        bg.material.SetFloat(propertyName, closeOriginValue);
    }

}