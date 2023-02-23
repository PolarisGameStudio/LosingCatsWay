using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TutorialActor_LobbyTreeSpine : TutorialActor
{
    [Title("Tree")]
    [SerializeField] private SkeletonGraphic treeGraphic;
    [SerializeField] private TextMeshProUGUI clickText;
    [SerializeField] private UIView islandTitleView;
    [SerializeField] private GameObject effect;

    [SerializeField] private UnityEvent OnSpineStart;
    [SerializeField] private UnityEvent OnSpineEnd;

    public override void Enter()
    {
        effect.SetActive(false);
        base.Enter();
        treeGraphic.AnimationState.SetAnimation(0, "Tree_Idle", true);
    }

    public void Click()
    {
        OnSpineStart?.Invoke();
        effect.SetActive(true);
        clickText.DOFade(0, 0.35f);
        var t = treeGraphic.AnimationState.SetAnimation(0, "Tree_Idle2", false);
        t.Complete += WaitTreeIdle;

        DOVirtual.DelayedCall(0.5f, islandTitleView.Show);
        DOVirtual.DelayedCall(4.5f, islandTitleView.Hide);
    }

    private void WaitTreeIdle(TrackEntry trackentry)
    {
        trackentry.Complete -= WaitTreeIdle;
        Exit();
        OnSpineEnd?.Invoke();
    }
}
