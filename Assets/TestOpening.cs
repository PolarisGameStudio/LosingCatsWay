using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOpening : MvcBehaviour
{
    public Animator animator;

    public void Loop01()
    {
        App.system.soundEffect.Play("ED00052");
        animator.Play("08_Loop");
    } 

}
