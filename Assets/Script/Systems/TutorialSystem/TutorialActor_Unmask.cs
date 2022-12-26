using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class TutorialActor_Unmask : TutorialActor
{
    [Title("Unmask")] [SerializeField] private Unmask unmask;
    [SerializeField] private RectTransform targetRect;

    public override void Enter()
    {
        unmask.fitTarget = targetRect;
        base.Enter();
    }
}
