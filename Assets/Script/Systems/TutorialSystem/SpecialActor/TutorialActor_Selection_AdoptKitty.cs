using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class TutorialActor_Selection_AdoptKitty : TutorialActor_Selection
{
    [Title("AdoptKitty")]
    [SerializeField] private RectTransform rejectRect;
    [SerializeField] private RectTransform acceptRect;
    [SerializeField] private RectTransform imageRect;

    private bool isShake;
    private Vector2 strength;

    public override void Enter()
    {
        strength = new Vector2(20f, 0f);
        base.Enter();
        
        imageRect.DOScale(Vector2.one, 0.3f).From(Vector2.zero).SetEase(Ease.OutBack);
        rejectRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.25f);
        acceptRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.5f);
    }

    public override void Accept()
    {
        base.Accept();
        base.Exit();
    }

    public override void Reject()
    {
        base.Reject();
        if (!isShake)
        {
            App.system.soundEffect.Play("ED00028");
            VibrateExtension.Vibrate(VibrateType.Nope);
            rejectRect.DOShakeAnchorPos(0.175f, strength, 10, 0)
                .OnStart(() => isShake = true)
                .OnComplete(() => isShake = false);
        }
    }
}
