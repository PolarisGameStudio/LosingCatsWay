using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LittleGame_Hand : LittleGame
{
    [Title("Game")]
    [SerializeField] private TextMeshProUGUI successText;
    [SerializeField] private TextMeshProUGUI failedText;
    [SerializeField] private Image dynamicCircle;
    [SerializeField] private RectTransform dynamicCircleRect;
    
    [Title("Setup")] [SerializeField] private Vector2 dynamicStartScale;
    [SerializeField, HorizontalGroup("HitRange")] private float hitRangeMin, hitRangeMax;
    [SerializeField] private Color32 hitColor = Color.white;
    [SerializeField] private Color32 missColor = Color.white;

    private bool isActive;
    private bool isSuccess;

    public override void StartGame(Cat cat)
    {
        base.StartGame(cat);

        successText.transform.localScale = Vector2.zero;
        failedText.transform.localScale = Vector2.zero;

        dynamicCircle.color = Color.white;

        dynamicCircleRect.DOScale(Vector2.zero, 2f).From(dynamicStartScale).SetEase(Ease.Linear).SetDelay(0.5f)
            .OnStart(() => isActive = true)
            .OnComplete(Click);
    }

    public void Click()
    {
        if (!isActive)
            return;

        VibrateExtension.Vibrate(VibrateType.Nope);
        App.system.soundEffect.Play("Button");
        
        isActive = false;
        dynamicCircleRect.DOKill();

        float value = dynamicCircleRect.localScale.x;

        if (value >= hitRangeMin && value <= hitRangeMax)
        {
            dynamicCircle.color = hitColor;
            isSuccess = true;
        }
        else
        {
            dynamicCircle.color = missColor;
            isSuccess = false;
        }

        TextMeshProUGUI tmpText = isSuccess ? successText : failedText;
        
        tmpText.DOFade(1, 0.25f).From(0);
        tmpText.transform.DOScale(Vector3.one, 0.25f).From(0).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Close();
                if (isSuccess)
                {
                    //Success();
                    SuccessToLobby();
                    cat.catHeartEffect.Play();
                    //Anim
                    anim.SetBool(CatAnimTable.LittleGameHandEndStatus.ToString(), true);
                }
                else
                {
                    //Failed();
                    FailedToLobby();
                    //Anim
                    anim.SetBool(CatAnimTable.LittleGameHandEndStatus.ToString(), false);
                }
                
                anim.SetBool(CatAnimTable.IsCanExit.ToString(), true);
                //OpenLobby();
            });
        }).SetEase(Ease.OutBack);
    }
}