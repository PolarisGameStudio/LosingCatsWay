using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class TutorialActor : SerializedMonoBehaviour
{
    protected MyApplication App => FindObjectOfType<MyApplication>();

    // public int order;
    [SerializeField] private UIView uiView;
    [SerializeField] private float exitDelay;
    [SerializeField] private bool instantHide;
    
    [Title("Bg")]
    [SerializeField] private bool useBlackBg;
    [SerializeField] private Sprite bgSprite;

    [Title("Camera")] 
    [SerializeField] private bool cameraDrag;
    [SerializeField] private bool cameraPinch;

    [Title("Bgm")] [SerializeField] private string bgmString;

    public virtual void Enter()
    {
        App.system.tutorial.SetCameraDrag(cameraDrag);
        App.system.tutorial.SetCameraPinch(cameraPinch);
        App.system.tutorial.SetBlackBg(useBlackBg);
        
        if (uiView != null)
            uiView.Show();
        
        App.system.tutorial.SetBg(bgSprite);
        
        if (!string.IsNullOrEmpty(bgmString))
            App.system.bgm.Play(bgmString);
    }

    public virtual void Exit()
    {
        DOVirtual.DelayedCall(exitDelay, () =>
        {
            App.system.tutorial.currentDirector.NextAction();
            
            if (uiView != null)
                uiView.InstantHide();
            if (!instantHide)
                gameObject.SetActive(false);
        });

        if (instantHide)
            gameObject.SetActive(false);
    }
}
