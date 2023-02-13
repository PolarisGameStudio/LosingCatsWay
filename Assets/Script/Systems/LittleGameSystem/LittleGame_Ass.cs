using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Spine;
using Spine.Unity;
using UnityEngine.UI;

public class LittleGame_Ass : LittleGame
{
    [Title("Game")]
    [SerializeField] private int clickTimes;
    [SerializeField] private Image fillCircle;

    private int tmpClick;
    private SkeletonAnimation spine;

    public override void StartGame(Cat cat)
    {
        base.StartGame(cat);

        tmpClick = clickTimes;

        fillCircle.fillAmount = 0;

        spine = cat.hand.GetComponent<SkeletonAnimation>();
        spine.AnimationState.ClearTracks();
    }

    public void Click()
    {
        App.system.soundEffect.Play("ED00027");
        tmpClick = Mathf.Clamp(tmpClick - 1, 0, clickTimes);
        float addAmount = 1f / clickTimes;
        float prevAmount = fillCircle.fillAmount;
        float nowAmount = prevAmount + addAmount;
        fillCircle.DOFillAmount(nowAmount, 0.25f).From(prevAmount).SetEase(Ease.OutExpo);
        
        VibrateExtension.Vibrate(VibrateType.Nope);
        App.system.soundEffect.Play("Button");
        
        RefreshClickSpine();

        if (tmpClick == 0)
        {
            Close();
            cat.catHeartEffect.Play();
            cat.hand.gameObject.SetActive(false);
            //ExitAnim
            anim.SetBool(CatAnimTable.IsCanExit.ToString(), true);
            // Success();
            // OpenLobby();
            SuccessToLobby();
        }
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
