using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LittleGame_Hand : LittleGame
{
    public TextMeshProUGUI statusText;
    public Transform pointCircle;
    public Image innerCircle;
    public Image pointerRing;
    [Space(10)]

    public Vector2 pointerCircleHitRange;
    [Space(10)]

    public Color32 hitColor = Color.white;
    public Color32 missColor = Color.white;

    private bool isActive;
    private bool isSuccess;

    public override void StartGame(Cat cat)
    {
        base.StartGame(cat);

        statusText.transform.localScale = Vector3.zero;

        innerCircle.color = Color.white;
        pointerRing.color = Color.white;

        pointCircle.DOScale(Vector2.zero, 2f).From(new Vector2(3, 3)).SetEase(Ease.Linear).SetDelay(0.5f)
            .OnStart(() => isActive = true)
            .OnComplete(Click);
    }

    public void Click()
    {
        if (!isActive)
            return;

        isActive = false;
        pointCircle.DOKill();

        float value = pointCircle.localScale.x;

        if (value >= pointerCircleHitRange.x && value <= pointerCircleHitRange.y)
        {
            //statusText.text = "成功";
            innerCircle.color = hitColor;
            pointerRing.color = hitColor;

            isSuccess = true;
        }
        else
        {
            //statusText.text = "失敗";
            innerCircle.color = missColor;
            pointerRing.color = missColor;

            isSuccess = false;
        }

        statusText.DOFade(1, 0.25f).From(0);

        statusText.transform.DOScale(Vector3.one, 0.25f).From(0).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Close();
                App.system.confirm.OnlyConfirm().Active(endId, () => 
                {
                    if (isSuccess)
                    {
                        Success();
                        cat.catHeartEffect.Play();
                        //Anim
                        anim.SetBool(CatAnimTable.LittleGameHandEndStatus.ToString(), true);
                    }
                    else
                    {
                        Failed();
                        //Anim
                        anim.SetBool(CatAnimTable.LittleGameHandEndStatus.ToString(), false);
                    }
                
                    anim.SetBool(CatAnimTable.IsCanExit.ToString(), true);
                    OpenLobby();
                });
            });
        }).SetEase(Ease.OutBack);
    }
}