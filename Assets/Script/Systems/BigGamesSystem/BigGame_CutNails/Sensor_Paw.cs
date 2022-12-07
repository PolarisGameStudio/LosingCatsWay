using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Sensor_Paw : MvcBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private BigGame_CutNails cutNails;

    private bool isCut;

    public void OpenSensor()
    {
        gameObject.SetActive(true);
    }

    public void CloseSensor()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isCut) return;
        cutNails.CutPaw();
        
        VibrateExtension.Vibrate(VibrateType.Cancel);
        VibrateExtension.Vibrate(VibrateType.Peek);
        
        isCut = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isCut = false;
    }
}
