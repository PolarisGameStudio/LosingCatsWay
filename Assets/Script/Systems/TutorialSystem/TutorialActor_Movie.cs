using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;

public class TutorialActor_Movie : TutorialActor
{
    [Title("MovieSetup")] [SerializeField] private UIView movieView;
    [SerializeField] private GameObject movieObject;
    public GameObject flag;

    public override void Enter()
    {
        base.Enter();
        movieView.InstantShow();
        movieObject.SetActive(true);
        InvokeRepeating("CheckFlag", 30, 0.25f);
    }

    private void CheckFlag()
    {
        if (flag.activeSelf)
        {
            CancelInvoke("CheckFlag");
            movieView.InstantHide();
            movieObject.SetActive(false);
            Exit();
        }
    }
}
