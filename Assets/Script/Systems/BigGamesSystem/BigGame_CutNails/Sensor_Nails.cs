using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sensor_Nails : MvcBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private BigGame_CutNails cutNails;
    [SerializeField] private int nailIndex;
    [SerializeField] private ParticleSystem nailParticle;
    [SerializeField] private RectTransform nailRect;

    private bool isCut;
    [ReadOnly] public int Value;
    [ReadOnly] public bool IsClean;

    public void OpenSensor()
    {
        gameObject.SetActive(true);
    }

    public void CloseSensor()
    {
        gameObject.SetActive(false);
    }

    public void ResetNail()
    {
        RectTransform rt = transform as RectTransform;
        nailRect.anchoredPosition = rt.anchoredPosition;

        IsClean = false;
    }

    private void MoveNail()
    {
        Vector2 origin = nailRect.anchoredPosition;
        Vector2 offset = origin;
        offset.y -= 70f / Value;
        nailRect.DOAnchorPos(offset, 0.1f).From(origin);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isCut) return;
        if (IsClean) return;
        cutNails.CutNail(nailIndex);
        
        if (!nailParticle.isPlaying)
            nailParticle.Play();
        
        VibrateExtension.Vibrate(VibrateType.Cancel);
        VibrateExtension.Vibrate(VibrateType.Nope);
        
        MoveNail();
        isCut = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isCut = false;
    }
}
