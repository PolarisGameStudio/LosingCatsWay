using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class CatPicker : MvcBehaviour
{
    #region Variable

    [SerializeField] private float offsetY;

    Camera cam;
    LeanPinchCamera pinchCam;
    LeanDragCamera dragCam;

    PolyNavAgent agent;
    Animator animator;

    private Cat cat;

    #endregion

    #region Basic

    private void Start()
    {
        cam = Camera.main;
        cat = GetComponent<Cat>();
        
        pinchCam = cam.GetComponent<LeanPinchCamera>();
        dragCam = cam.GetComponent<LeanDragCamera>();

        agent = GetComponent<PolyNavAgent>();
        animator = GetComponent<Animator>();
    }

    void Active()
    {
        App.controller.lobby.Close();
        App.view.followCat.Close();
        
        App.system.soundEffect.Play("Button");

        //1.Stop AI
        agent.Stop();

        //2.Stop animator
        for (int i = 0; i < animator.parameters.Length; i++)
        {
            if (animator.parameters[i].type == AnimatorControllerParameterType.Bool)
                animator.SetBool(animator.parameters[i].name, false);

            if (animator.parameters[i].type == AnimatorControllerParameterType.Int)
                animator.SetInteger(animator.parameters[i].name, 0);
        }

        //3.Play pick animation
        animator.Play("PickCat");
    }

    void Cancel()
    {
        App.controller.lobby.Open();

        //1.Trigger animator
        animator.SetTrigger("OnPick");

        //2.Start AI
        agent.Stop();
    }

    #endregion

    #region Inspector

    public void Pick()
    {
        if (cat.isFriendMode)
            return;

        if (App.model.build.IsCanMoveOrRemove)
            return;

        if (App.controller.followCat.isFollowing)
            return;

        Active();

        pinchCam.enabled = false;
        dragCam.enabled = false;

        transform.position = FingerPosition();
    }

    public void ResetPicker()
    {
        if (cat.isFriendMode)
            return;
        
        if (App.model.build.IsCanMoveOrRemove)
            return;

        if (App.controller.followCat.isFollowing)
            return;

        pinchCam.enabled = true;
        dragCam.enabled = true;

        Cancel();
    }

    #endregion

    #region GetSet

    public Vector2 FingerPosition()
    {
        Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.y += offsetY;
        return pos;
    }

#endregion
}
