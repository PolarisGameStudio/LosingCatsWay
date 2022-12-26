using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TutorialActor_SoundEffect : TutorialActor
{
    [Title("SoundEffect")] [SerializeField]
    private string soundName;

    public override void Enter()
    {
        base.Enter();
        App.system.soundEffect.Play(soundName);
    }
}