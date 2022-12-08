using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Spine.Unity;
using TMPro;
using UnityEngine;

public abstract class G_EIGHT_LeadBoard : MonoBehaviour
{
    public StringData catVarietyName;
    public TextMeshProUGUI hatRatioText;
    public TextMeshProUGUI catCountText;

    public SkeletonGraphic skeletonGraphic;
    
    public virtual void Out(float delay)
    {
    }
    
    public virtual void In(float delay, LeaderBoard leaderBoard)
    {
    }

    public virtual void OutIn(float delay, LeaderBoard leaderBoard)
    {
    }

    public virtual void ChangeValue(LeaderBoard leaderBoard, LeaderBoard prevLeaderBoard)
    {
    }

    protected virtual void ChangeSkin(string variety)
    {
        var newVariety = variety.Replace('_', '-');

        if (newVariety.Equals("White-Special"))
            newVariety = "White_Special";
        
        // 王若呈那邊ID在靠北
        if (newVariety.Contains("Siamese") && !newVariety.Contains("GT") && !newVariety.Contains("CT"))
        {
            var t = newVariety.Split('-');
            newVariety = t[1] + '-' + t[0];
        }
        
        skeletonGraphic.Skeleton.SetSkin("Normal_Cat/" + newVariety);
    }
    
    protected void ChangeHatRatio(float from, float to)
    {
        float originHatRatio = from;
        
        DOTween.To(() => originHatRatio, x => originHatRatio = x, to, 0.25f).OnUpdate(() =>
        {
            hatRatioText.text = originHatRatio.ToString();
        });
    }
    
    protected void ChangeCatCount(int from, int to)
    {
        int originCatCount = from;

        DOTween.To(() => originCatCount, x => originCatCount = x, to, 0.25f).OnUpdate(() =>
        {
            catCountText.text = originCatCount.ToString();
        });
    }
}
