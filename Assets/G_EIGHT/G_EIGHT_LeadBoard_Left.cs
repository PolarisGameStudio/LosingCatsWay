using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine;
using UnityEngine;

public class G_EIGHT_LeadBoard_Left : G_EIGHT_LeadBoard
{
    private LeaderBoard _leaderBoardTmp;

    public override void Out(float delay)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            skeletonGraphic.AnimationState.Complete += WaitStartOutRun;
            skeletonGraphic.AnimationState.SetAnimation(0, "G8/3_outjump", false);
        });
    }

    private void WaitStartOutRun(TrackEntry entry)
    {
        if (entry.Animation.Name.Equals("G8/3_outjump"))
        {
            hatRatioText.DOFade(0, 0.25f);
            catCountText.DOFade(0, 0.25f);
            catVarietyNameText.DOFade(0, 0.25f);

            skeletonGraphic.AnimationState.Complete -= WaitStartOutRun;
        }
    }

    public override void In(float delay, LeaderBoard leaderBoard)
    {
        _leaderBoardTmp = leaderBoard;
        catVarietyNameText.text = catVarietyStringData.Contents[leaderBoard.Variety];
        ChangeSkin(leaderBoard.Variety);

        DOVirtual.DelayedCall(delay, () =>
        {
            skeletonGraphic.AnimationState.Start += WaitStartInRun;
            skeletonGraphic.AnimationState.SetAnimation(0, "G8/3_injump", true);
            skeletonGraphic.AnimationState.AddAnimation(0, "AI_Main/IDLE_Ordinary01", true, 0);
        });
    }

    private void WaitStartInRun(TrackEntry entry)
    {
        if (entry.Animation.Name.Equals("AI_Main/IDLE_Ordinary01"))
        {
            hatRatioText.text = 0.ToString();
            catCountText.text = 0.ToString();

            hatRatioText.DOFade(1, 0.25f).OnComplete(() =>
            {
                ChangeHatRatio(0, _leaderBoardTmp.HatRatio);
                ChangeCatCount(0, _leaderBoardTmp.CatCount);
            });

            catCountText.DOFade(1, 0.25f);
            catVarietyNameText.DOFade(1, 0.25f);
            
            skeletonGraphic.AnimationState.SetAnimation(0, "AI_Cohesive/RunToIdle_01", false);
            skeletonGraphic.AnimationState.Start -= WaitStartInRun;
        }
    }

    public override void OutIn(float delay, LeaderBoard leaderBoard)
    {
        _leaderBoardTmp = leaderBoard;

        skeletonGraphic.AnimationState.Start += WaitStartOutIn;
        Out(delay);
    }

    protected override void ChangeSkin(string variety)
    {
        base.ChangeSkin(variety);
        skeletonGraphic.Skeleton.SetAttachment("SantaHat", "SantaHat");
    }

    private void WaitStartOutIn(TrackEntry entry)
    {
        if (entry.Animation.Name.Equals("AI_Cohesive/RunToIdle_01"))
        {
            In(0, _leaderBoardTmp);
            skeletonGraphic.AnimationState.Start -= WaitStartOutIn;
        }
    }

    public override void ChangeValue(LeaderBoard leaderBoard, LeaderBoard prevLeaderBoard)
    {
        ChangeHatRatio(prevLeaderBoard.HatRatio, leaderBoard.HatRatio);
        ChangeCatCount(prevLeaderBoard.CatCount, leaderBoard.CatCount);
    }
}