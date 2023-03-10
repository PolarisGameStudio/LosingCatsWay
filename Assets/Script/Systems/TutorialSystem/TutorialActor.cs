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
    private MyApplication app;

    protected MyApplication App
    {
        get
        {
            if (app == null)
                app = FindObjectOfType<MyApplication>();
            return app;
        }
    }

    [SerializeField] private UIView uiView;
    [SerializeField] private float exitDelay;
    [SerializeField] private bool instantHide;
    
    [Title("Bg")]
    [SerializeField] private bool useBlackBg;
    [SerializeField] private Sprite bgSprite;

    [Title("Camera")] 
    [SerializeField] private bool cameraDrag;
    [SerializeField] private bool cameraPinch;

    [Title("Sound")]
    [SerializeField] private string bgmString;
    [SerializeField] private string soundString;

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
        
        if (!string.IsNullOrEmpty(soundString))
            App.system.soundEffect.Play(soundString);
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
