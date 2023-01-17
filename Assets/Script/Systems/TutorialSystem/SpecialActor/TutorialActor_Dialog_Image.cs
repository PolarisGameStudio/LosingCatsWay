using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TutorialActor_Dialog_Image : TutorialActor_Dialog
{
    [Title("Image")] [SerializeField] private Image image;
    [SerializeField] private bool fadeIn;
    [SerializeField] private bool fadeOut;

    public override void Enter()
    {
        base.Enter();
        if (fadeIn)
            image.DOFade(1, 0.3f).From(0).SetEase(Ease.OutSine);
    }

    public override void Exit()
    {
        if (fadeOut)
            image.DOFade(0, 0.25f).From(1).SetEase(Ease.OutSine)
                .OnComplete(base.Exit);
        else
            base.Exit();
    }
}
