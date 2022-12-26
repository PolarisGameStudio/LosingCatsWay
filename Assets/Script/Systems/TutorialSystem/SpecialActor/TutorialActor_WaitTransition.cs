using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class TutorialActor_WaitTransition : TutorialActor
{
    [Title("WaitTransition")] [SerializeField]
    private UnityEvent OnTransitionStart;
    [SerializeField] private UnityEvent OnTransition;
    [SerializeField] private float waitTime;

    public override void Enter()
    {
        base.Enter();
        OnTransitionStart?.Invoke();
        App.system.transition.Active(waitTime, () => OnTransition?.Invoke());
    }
}
