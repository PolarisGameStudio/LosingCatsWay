using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Runtime.UIManager.Containers;

public class AutoHideView : MonoBehaviour
{
    public UIView mainView;
    public float triggerHideTime = 5f;
    public float delayShowTime = 0f;

    public UIView[] uiViewsToPause;

    float currTime;
    bool isHiding = false;

    //private bool pauseAutoHide = false;
    private bool pause = false;

    WaitForSecondsRealtime delayRealtime;

    #region Init

    private void OnEnable()
    {
        Lean.Touch.LeanTouch.OnFingerDown += ShowViews;
    }

    private void OnDisable()
    {
        Lean.Touch.LeanTouch.OnFingerDown -= ShowViews;
    }

    private void Start()
    {
        delayRealtime = new WaitForSecondsRealtime(delayShowTime);
    }

    #endregion

    private void FixedUpdate()
    {
        if (isHiding)
        {
            currTime = 0;
            return;
        }

        //if (pauseAutoHide) return;

        if (Pause)
        {
            currTime = 0;
            return;
        }

        currTime += Time.fixedDeltaTime;

        if (currTime >= triggerHideTime)
            HideViews();
    }

    void HideViews()
    {
        mainView.Hide();
        isHiding = true;
    }

    void ShowViews(Lean.Touch.LeanFinger finger)
    {
        //If touch, reset timer
        currTime = 0;

        if (!isHiding) return;

        //if (pauseAutoHide) return;

        if (Pause) return;

        StartCoroutine(Show());
    }

    IEnumerator Show()
    {
        yield return delayRealtime;

        mainView.Show();

        currTime = 0;

        isHiding = false;
    }

    //public void PauseAutoHide(bool pause)
    //{
    //    this.pauseAutoHide = pause;

    //    if (pause)
    //    {
    //        StopAllCoroutines();
    //    }
    //    else
    //    {
    //        currTime = 0;
    //        isHiding = false;
    //    }
    //}

    bool Pause
    {
        get
        {
            pause = false;

            for (int i = 0; i < uiViewsToPause.Length; i++)
            {
                if (uiViewsToPause[i].isShowing)
                {
                    pause = true;
                    break;
                }
                if (uiViewsToPause[i].isVisible)
                {
                    pause = true;
                    break;
                }
            }

            return pause;
        }
    }
}
