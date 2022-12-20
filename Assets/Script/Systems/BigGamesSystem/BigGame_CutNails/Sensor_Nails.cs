using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sensor_Nails : MvcBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private BigGame_CutNails cutNails;
    [SerializeField] private int nailIndex;
    [SerializeField] private ParticleSystem nailParticle;
    [SerializeField] private RectTransform nailRect;

    [Title("UI")] [SerializeField] private Image nailImage;
    [SerializeField] private Sprite fullNail;
    [SerializeField] private Sprite crackNail;
    [SerializeField] private Sprite halfNail;

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

    public void CheckNail(int value)
    {
        if (value <= 0)
        {
            nailImage.sprite = halfNail;
            return;
        }

        if (value < Value / 2)
        {
            nailImage.sprite = crackNail;
            return;
        }

        nailImage.sprite = fullNail;
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
        
        isCut = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isCut = false;
    }
}
