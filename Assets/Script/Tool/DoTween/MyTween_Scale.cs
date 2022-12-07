using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class MyTween_Scale : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 startScale = Vector3.one;
    public Vector3 targetScale;
    public float duration;
    public int loop;
    public LoopType loopType;
    public Ease ease;
    public float delay;

    public void Play()
    {
        targetTransform.DOScale(targetScale, duration).From(startScale).SetEase(ease).SetLoops(loop, loopType).SetDelay(delay);
    }
}
