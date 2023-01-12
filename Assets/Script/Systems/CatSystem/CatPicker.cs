using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using PolyNav;

public class CatPicker : MvcBehaviour
{
    #region Variable

    [SerializeField] private float offsetY;
    [SerializeField] private PolyNavAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private Cat cat;

    private Camera cam;
    private LeanPinchCamera pinchCam;
    private LeanDragCamera dragCam;

    private Vector3 startPosition = Vector3.zero;
    private bool isPicking;

    #endregion

    #region Basic

    private void Start()
    {
        cam = Camera.main;
        pinchCam = cam.GetComponent<LeanPinchCamera>();
        dragCam = cam.GetComponent<LeanDragCamera>();
    }

    private void Active()
    {
        App.controller.lobby.Close();
        App.view.followCat.Close();
        
        //1.Stop AI
        agent.Stop();
        
        SetStartPosition();
        PlaySound();
        
        isPicking = true;

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

    private void Cancel()
    {
        isPicking = false;
        App.controller.lobby.Open();
        animator.SetTrigger("OnPick");
        CheckCatCanStand();
    }

    private void SetStartPosition()
    {
        if (isPicking)
            return;
        startPosition = cat.transform.position;
    }

    private void CheckCatCanStand()
    {
        var position = cat.transform.position;
        int gridX = (int)(position.x / 5.12);
        int gridY = (int)(position.y / 5.12);

        int gridValue = App.system.grid.GetGrid(gridX, gridY).Value;

        if (gridValue != 1)
        {
            cat.transform.position = startPosition;
            cat.Reset();
        }
    }

    private void PlaySound()
    {
        if (isPicking)
            return;
        App.system.soundEffect.Play("Button");
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
