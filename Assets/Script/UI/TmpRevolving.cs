using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class TmpRevolving : MonoBehaviour
{
    [SerializeField] private RectTransform parentRect;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float revolvingTime;
    [SerializeField] private float waitTime;
    [SerializeField] private float offset;
    [SerializeField] private int maxCharCount;
    
    [Title("Config")]
    [SerializeField] private bool PlayOnStart;
    [SerializeField] private bool stopAtCenter;
    [SerializeField, ShowIf("stopAtCenter", true)]
    private float stopTime;

    private RectTransform textRect;
    private float start;
    private float end;
    private float center;
    private bool isInit = false;

    private Sequence stopAtCenterSeq;

    private void Start()
    {
        if (!isInit)
            Init();
        
        if (PlayOnStart)
            StartRevolving();
    }

    #region Properties

    private bool IsLongText()
    {
        return text.text.Length > maxCharCount;
    }

    #endregion

    public void Init()
    {
        if (isInit)
            return;
        
        isInit = true;
        
        textRect = text.transform as RectTransform;
        
        float parentSize = parentRect.sizeDelta.x;
        float parentHalfSize = parentSize / 2;

        float textSize = textRect.sizeDelta.x;
        float textHalfSize = textSize / 2;

        start = 0 + parentHalfSize + textHalfSize + offset;
        end = 0 - parentHalfSize - textHalfSize - offset;

        center = textRect.anchoredPosition.x;
    }
    
    [Button]
    public void StopRevolving()
    {
        Init();
        
        if (!IsLongText())
        {
            text.alignment = TextAlignmentOptions.Center;
            return;
        }
        
        if (!stopAtCenter)
            textRect.DOKill();
        else
            stopAtCenterSeq.Kill();
        
        textRect.anchoredPosition = new Vector2(start, textRect.anchoredPosition.y);
    }

    [Button]
    public void StartRevolving()
    {
        Init();
        
        if (!IsLongText()) 
        {
            text.alignment = TextAlignmentOptions.Center;
            return;
        }

        text.alignment = TextAlignmentOptions.MidlineLeft;
        
        if (!stopAtCenter)
        {
            textRect.DOAnchorPosX(end, revolvingTime)
                .From(new Vector2(start, textRect.anchoredPosition.y))
                .SetEase(Ease.Linear).SetDelay(waitTime).SetLoops(-1);
        }
        else
        {
            stopAtCenterSeq = DOTween.Sequence();

            stopAtCenterSeq
                .Append(textRect.DOAnchorPosX(center, revolvingTime)
                    .From(new Vector2(start, textRect.anchoredPosition.y))
                    .SetEase(Ease.Linear).SetDelay(waitTime))
                .AppendInterval(stopTime)
                .Append(textRect.DOAnchorPosX(end, revolvingTime)
                    .From(new Vector2(center, textRect.anchoredPosition.y))
                    .SetEase(Ease.Linear).SetDelay(waitTime))
                .SetLoops(-1);
        }
    }

    private void OnDestroy()
    {
        textRect.DOKill();
        stopAtCenterSeq.Kill();
    }
}
