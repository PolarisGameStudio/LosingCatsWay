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
    private CanvasGroup canvasGroup;
    [SerializeField] private Button button;

    [SerializeField] private float fadeDuration;

    private void Start()
    {
        canvasGroup.DOFade(0, 0);
    }

    public override void Enter()
    {
        base.Enter();
        canvasGroup.DOFade(1, fadeDuration);
        button.interactable = true;
    }

    public override void Exit()
    {
        button.interactable = false;
        canvasGroup.DOFade(0, fadeDuration).From(1).OnComplete(base.Exit);
    }
}
