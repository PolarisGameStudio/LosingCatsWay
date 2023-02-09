using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class CatchCatLove : MvcBehaviour
{
    [Title("UIParticle")]
    [SerializeField] private UIParticle hateEffect;
    [SerializeField] private UIParticle loveEffect;
    [SerializeField] private UIParticle bigLoveEffect;

    public void PlayHate()
    {
        hateEffect.Play();
    }

    public void PlayLove()
    {
        loveEffect.Play();
    }

    public void PlayBigLove()
    {
        bigLoveEffect.Play();
    }
}
