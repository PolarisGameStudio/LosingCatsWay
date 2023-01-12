using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TutorialActor_ImageWindow : TutorialActor
{
    [Title("ImageWindow")] [SerializeField]
    private Image image;

    [SerializeField] private float fadeDuration;

    private void Start()
    {
        image.DOFade(0, 0);
    }

    public override void Enter()
    {
        base.Enter();
        image.DOFade(1, fadeDuration);
    }

    public override void Exit()
    {
        image.DOFade(0, fadeDuration).From(1).OnComplete(base.Exit);
    }
}
