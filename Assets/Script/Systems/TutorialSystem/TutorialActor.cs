using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;

public class TutorialActor : SerializedMonoBehaviour
{
    protected MyApplication App => FindObjectOfType<MyApplication>();

    [SerializeField] private UIView uiView;
    [SerializeField] private float exitDelay;
    [SerializeField] private bool useBlackBg;

    [Title("Camera")] 
    [SerializeField] private bool cameraDrag;
    [SerializeField] private bool cameraPinch;

    public virtual void Enter()
    {
        //
    }

    public virtual void Exit()
    {
        //
    }
}
