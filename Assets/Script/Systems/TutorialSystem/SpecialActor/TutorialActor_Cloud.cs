using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TutorialActor_Cloud : TutorialActor
{
    [Title("Cloud")] [SerializeField] private RectTransform[] cloudRects;
    [SerializeField] private Vector2[] cloudStartPoints;
    [SerializeField] private Vector2[] cloudEndPoints;
    [SerializeField] private TextMeshProUGUI clickText;
    [SerializeField] private UIView islandTitleView;
    
    [SerializeField] private UnityEvent OnCloudStart;
    [SerializeField] private UnityEvent OnCloudEnd;

    public void OpenCloud()
    {
        OnCloudStart?.Invoke();
        clickText.DOFade(0, 0.35f).OnComplete(() =>
        {
            for (int i = 0; i < cloudRects.Length; i++)
                cloudRects[i].DOAnchorPos(cloudEndPoints[i], 1f).From(cloudStartPoints[i]);
            
            DOVirtual.DelayedCall(0.35f, islandTitleView.Show);
            DOVirtual.DelayedCall(2.35f, () =>
            {
                Exit();
                OnCloudEnd?.Invoke();
            });
        });
    }

    [Button]
    private void DebugGetCloudStartPosition()
    {
        for (int i = 0; i < cloudRects.Length; i++)
            cloudStartPoints[i] = cloudRects[i].anchoredPosition;
    }
    
    [Button]
    private void DebugGetCloudEndPosition()
    {
        for (int i = 0; i < cloudRects.Length; i++)
            cloudEndPoints[i] = cloudRects[i].anchoredPosition;
    }

    [Button]
    private void DebugSetStart()
    {
        for (int i = 0; i < cloudRects.Length; i++)
            cloudRects[i].anchoredPosition = cloudStartPoints[i];
    }
    
    [Button]
    private void DebugSetEnd()
    {
        for (int i = 0; i < cloudRects.Length; i++)
            cloudRects[i].anchoredPosition = cloudEndPoints[i];
    }
}
