using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Spine;
using Spine.Unity;
using UnityEngine.UI;

public class LittleGame_Ass : LittleGame
{
    public TextMeshProUGUI clickText;
    public int clickTimes;
    [SerializeField] private Image fillCircle;

    private int value;
    private SkeletonAnimation spine;

    public override void StartGame(Cat cat)
    {
        base.StartGame(cat);

        value = clickTimes;
        RefreshClickText();

        fillCircle.fillAmount = 0;

        spine = cat.hand.GetComponent<SkeletonAnimation>();
        spine.AnimationState.ClearTracks();
    }

    public void Click()
    {
        value = Mathf.Clamp(value - 1, 0, clickTimes);
        float addAmount = 1f / clickTimes;
        float prevAmount = fillCircle.fillAmount;
        float nowAmount = prevAmount + addAmount;
        fillCircle.DOFillAmount(nowAmount, 0.25f).From(prevAmount).SetEase(Ease.OutExpo);
        
        RefreshClickText();
        RefreshClickSpine();

        if (value == 0)
        {
            Close();

            App.system.confirm.OnlyConfirm().Active(endId, () => 
            {
                Success();
                cat.catHeartEffect.Play();
                cat.hand.gameObject.SetActive(false);
                //ExitAnim
                anim.SetBool(CatAnimTable.IsCanExit.ToString(), true);
                OpenLobby();
            });
        }
    }

    private void RefreshClickText()
    {
        clickText.text = value.ToString();
    }

    private void RefreshClickSpine()
    {
        cat.hand.gameObject.SetActive(true);

        spine.AnimationState.ClearTracks();
        TrackEntry track = spine.AnimationState.SetAnimation(0, "Small_BubbleGame/Ass_Tools", false);
        track.Complete += ClickComplete;
    }

    void ClickComplete(TrackEntry track)
    {
        cat.hand.gameObject.SetActive(false);
        track.Complete -= ClickComplete;
    }
}
