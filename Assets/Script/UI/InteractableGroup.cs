using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[ExecuteInEditMode]
public class InteractableGroup : MonoBehaviour
{
    public bool interactable = true;
    [Range(0, 1)] public float disableAlpha;

    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (interactable)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
        }
        else
        {
            canvasGroup.alpha = disableAlpha;
            canvasGroup.interactable = false;
        }
    }
}
