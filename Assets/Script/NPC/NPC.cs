using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class NPC : MvcBehaviour
{
    public string id;
    public SkeletonGraphic skeleton;

    [Space(10)] [Title("UI")] 
    public Transform talkBg;
    public TextMeshProUGUI content;
    public TextMeshProUGUI npcName;
    
    
    private int index = 0;

    private string animationIdleId = "Idle";
    private string animationSpeakeId = "Speaking_1&2";
    private string animationSpcialId = "Speaking_SP";

    public void Click()
    {
        CancelInvoke("CloseTalkBg");
        
        Invoke("CloseTalkBg", 3.75f);
        talkBg.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBack);
        
        content.text = App.factory.stringFactory.GetNpcContent(id, (index + 1));
        npcName.text = App.factory.stringFactory.GetNpcName(id);

        if (index < 2)
            skeleton.AnimationState.SetAnimation(0, animationSpeakeId, false);
        else
            skeleton.AnimationState.SetAnimation(0, animationSpcialId, false);
        
        skeleton.AnimationState.AddAnimation(0, animationIdleId, true, 0);
        index++;

        if (index == 3)
            index = 0;
    }

    private void CloseTalkBg()
    {
        talkBg.DOScale(Vector3.zero, 0.25f).From(Vector3.one).SetEase(Ease.OutBack);
    }
}