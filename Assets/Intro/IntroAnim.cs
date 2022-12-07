using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnim : MonoBehaviour
{
    public Animator animator;

    public void Loop01()
    {
        animator.Play("02_cloud");
    }
    public void Loop02()
    {
        animator.Play("09");
    }

    public void Next()
    {
        animator.SetTrigger("Next");
    }
}
