using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MySwipeListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Title("TriggerRange")] [SerializeField] private float minTriggerRangeX;
    [SerializeField] private float minTriggerRangeY;
    
    [Title("Usage")] [SerializeField] private bool useLeft;
    [SerializeField] private bool useRight;
    [SerializeField] private bool useUp;
    [SerializeField] private bool useDown;
    
    [PropertySpace(5), SerializeField, ShowIf("useLeft")] private UnityEvent OnSwipeLeft;
    [PropertySpace(5), SerializeField, ShowIf("useRight")] private UnityEvent OnSwipeRight;
    [PropertySpace(5), SerializeField, ShowIf("useUp")] private UnityEvent OnSwipeUp;
    [PropertySpace(5), SerializeField, ShowIf("useDown")] private UnityEvent OnSwipeDown;
    
    private Vector2 downPos, upPos;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        downPos = eventData.position;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        upPos = eventData.position;
        TriggerSwipe();
    }

    private void ResetSwipe()
    {
        upPos = Vector2.zero;
        downPos = Vector2.zero;
    }

    private void TriggerSwipe()
    {
        if (upPos == downPos)
            return;
        
        float offsetX = Mathf.Abs(upPos.x - downPos.x);
        float offsetY = Mathf.Abs(upPos.y - downPos.y);
        
        // Horizontal
        if (offsetX > offsetY && offsetX >= Mathf.Abs(minTriggerRangeX))
        {
            if (upPos.x > downPos.x)
                SwipeLeft();
            else
                SwipeRight();
            return;
        }

        if (offsetY < Mathf.Abs(minTriggerRangeY))
            return;
        
        // Vertical
        if (upPos.y > downPos.y)
            SwipeDown();
        else
            SwipeUp();
    }

    private void SwipeLeft()
    {
        if (!useLeft) return;
        OnSwipeLeft?.Invoke();
        ResetSwipe();
    }
    
    private void SwipeRight()
    {
        if (!useRight) return;
        OnSwipeRight?.Invoke();
        ResetSwipe();
    }
    
    private void SwipeUp()
    {
        if (!useUp) return;
        OnSwipeUp?.Invoke();
        ResetSwipe();
    }
    
    private void SwipeDown()
    {
        if (!useDown) return;
        OnSwipeDown?.Invoke();
        ResetSwipe();
    }
}
