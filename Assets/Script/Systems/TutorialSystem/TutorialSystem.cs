using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class TutorialSystem : MvcBehaviour
{
    [SerializeField] private LeanDragCamera dragCamera;
    [SerializeField] private LeanPinchCamera pinchCamera;

    public void SetCameraDrag(bool value)
    {
        dragCamera.enabled = value;
    }

    public void SetCameraPinch(bool value)
    {
        pinchCamera.enabled = value;
    }
}
