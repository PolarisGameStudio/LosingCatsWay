using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class G_EIGHT_LeadBoard_Right : G_EIGHT_LeadBoard
{
    public Image mask;

    public override void Out(float delay)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            mask.enabled = true;
            mask.transform.DOScaleY(1, 0.25f).From(0).OnComplete(() =>
            {
                skeletonGraphic.enabled = false;
                hatRatioText.enabled = false;
                catCountText.enabled = false;
                catVarietyNameText.enabled = false;
            });
        });
    }

    public override void In(float delay, LeaderBoard leaderBoard)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            skeletonGraphic.enabled = true;
            hatRatioText.enabled = true;
            catCountText.enabled = true;
            catVarietyNameText.enabled = true;

            catVarietyNameText.text = catVarietyStringData.Contents[leaderBoard.Variety];
            ChangeSkin(leaderBoard.Variety);
            
            mask.transform.DOScaleY(0, 0.25f).From(1).OnComplete(() =>
            {
                ChangeHatRatio(0, leaderBoard.HatRatio);
                ChangeCatCount(0, leaderBoard.CatCount);
                mask.enabled = false;
            });
        });
    }

    public override void OutIn(float delay, LeaderBoard leaderBoard)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            mask.enabled = true;
            mask.transform.DOScaleY(1, 0.25f).From(0).OnComplete(() =>
            {
                ChangeSkin(leaderBoard.Variety);
                mask.transform.DOScaleY(0, 0.25f).From(1).OnComplete(() =>
                {
                    ChangeHatRatio(0, leaderBoard.HatRatio);
                    ChangeCatCount(0, leaderBoard.CatCount);
                    mask.enabled = false;
                });
            });
        });
    }

    public override void ChangeValue(LeaderBoard leaderBoard, LeaderBoard prevLeaderBoard)
    {
        ChangeHatRatio(prevLeaderBoard.HatRatio, leaderBoard.HatRatio);
        ChangeCatCount(prevLeaderBoard.CatCount, leaderBoard.CatCount);
    }
}