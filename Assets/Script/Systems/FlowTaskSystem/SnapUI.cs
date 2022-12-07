using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapUI : MonoBehaviour
{
    public RectTransform toSnapTarget;

    public bool useLateUpdate;
    public bool followSizeDelta;
    
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (toSnapTarget == null) return;
        if (useLateUpdate) return;

        rectTransform.anchoredPosition = toSnapTarget.anchoredPosition;

        if (!followSizeDelta) return;
        rectTransform.sizeDelta = toSnapTarget.sizeDelta;
    }

    private void LateUpdate()
    {
        if (toSnapTarget == null) return;
        if (!useLateUpdate) return;

        rectTransform.anchoredPosition = toSnapTarget.anchoredPosition;

        if (!followSizeDelta) return;
        rectTransform.sizeDelta = toSnapTarget.sizeDelta;
    }
}
