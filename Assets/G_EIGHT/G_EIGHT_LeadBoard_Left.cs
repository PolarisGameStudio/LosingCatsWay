using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine;
using UnityEngine;

public class G_EIGHT_LeadBoard_Left : G_EIGHT_LeadBoard
{
    public bool isFirst = false;

    public float leftPositionX;
    public float rightPositionX;
    public float centerPositionX;

    private LeaderBoard _leaderBoardTmp;

    public override void Out(float delay)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            skeletonGraphic.AnimationState.Start += WaitStartOutRun;

            if (isFirst)
            {
                skeletonGraphic.AnimationState.SetAnimation(0, "AI_Cohesive/SitToIdle_01", false);
                skeletonGraphic.AnimationState.AddAnimation(0, "AI_Cohesive/IdleToRun_01", false, 0);
            }
            else
                skeletonGraphic.AnimationState.SetAnimation(0, "AI_Cohesive/IdleToRun_01", false);

            skeletonGraphic.AnimationState.AddAnimation(0, "AI_Main/Run_01", true, 0);
        });
    }

    private void WaitStartOutRun(TrackEntry entry)
    {
        if (entry.Animation.Name.Equals("AI_Main/Run_01"))
        {
            skeletonGraphic.transform.DOLocalMoveX(leftPositionX, 0.5f).From(centerPositionX).OnComplete(() =>
            {
                skeletonGraphic.AnimationState.SetAnimation(0, "AI_Cohesive/RunToIdle_01", false);
            });
            
            hatRatioText.DOFade(0, 0.25f);
            catCountText.DOFade(0, 0.25f);

            skeletonGraphic.AnimationState.Start -= WaitStartOutRun;
        }
    }

    public override void In(float delay, LeaderBoard leaderBoard)
    {
        _leaderBoardTmp = leaderBoard;
        ChangeSkin(leaderBoard.Variety);
        
        DOVirtual.DelayedCall(delay, () =>
        {
            skeletonGraphic.AnimationState.Start += WaitStartInRun;
            skeletonGraphic.AnimationState.SetAnimation(0, "AI_Main/Run_01", true);
        });
    }

    private void WaitStartInRun(TrackEntry entry)
    {
        if (entry.Animation.Name.Equals("AI_Main/Run_01"))
        {
            hatRatioText.text = 0.ToString();
            catCountText.text = 0.ToString();

            hatRatioText.DOFade(1, 0.25f).OnComplete(() =>
            {
                ChangeHatRatio(0, _leaderBoardTmp.HatRatio);
                ChangeCatCount(0, _leaderBoardTmp.CatCount);
            });
            catCountText.DOFade(1, 0.25f);

            skeletonGraphic.transform.DOLocalMoveX(centerPositionX, 0.5f).From(rightPositionX).OnComplete(() =>
            {
                skeletonGraphic.AnimationState.SetAnimation(0, "AI_Cohesive/RunToIdle_01", false);

                if (isFirst)
                {
                    skeletonGraphic.AnimationState.AddAnimation(0, "AI_Cohesive/IdleToSit_01", false, 0);
                    skeletonGraphic.AnimationState.AddAnimation(0, "AI_Main/Sit_01", true, 0);
                }
                else
                    skeletonGraphic.AnimationState.AddAnimation(0, "AI_Main/IDLE_Ordinary01", true, 0);
            });

            skeletonGraphic.AnimationState.Start -= WaitStartInRun;
        }
    }

    public override void OutIn(float delay, LeaderBoard leaderBoard)
    {
        _leaderBoardTmp = leaderBoard;

        skeletonGraphic.AnimationState.Start += WaitStartOutIn;
        Out(delay);
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