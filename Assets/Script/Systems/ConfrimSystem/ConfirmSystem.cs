using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ConfirmSystem : MvcBehaviour
{
    [SerializeField] private UIView view;

    [SerializeField] private GameObject okButton;
    [SerializeField] private GameObject cancelButton;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;

    private UnityAction _okEvent;
    private UnityAction _cancelEvent;

    private int siblingIndex = -1;
    private bool isOnlyConfirm = false;
    private bool isBlock = false;

    public ConfirmSystem OnlyConfirm()
    {
        cancelButton.SetActive(false);
        isOnlyConfirm = true;
        return this;
    }

    public void Active(ConfirmTable key, UnityAction okEvent = null, UnityAction cancelEvent = null)
    {
        UnityAction action = () =>
        {
            Open();

            string id = key.ToString();

            titleText.text = App.factory.confirmFactory.GetNormalTitle(id);
            contentText.text = App.factory.confirmFactory.GetNormalContent(id);

            _okEvent = okEvent;
            _cancelEvent = cancelEvent;
        };

        if (view.isVisible)
            StartCoroutine(WaitUntilClose(action));
        else
        {
            action.Invoke();
        }
    }
    
    public void ActiveByInsert(ConfirmTable key, string titleInsert = "", string contentInsert = "",
        UnityAction okEvent = null, UnityAction cancelEvent = null)
    {
        UnityAction action = () =>
        {
            Open();

            string id = key.ToString();

            string title = App.factory.confirmFactory.GetNormalTitle(id);
            string content = App.factory.confirmFactory.GetNormalContent(id);
            
            titleText.text = title.Replace("<insert>", titleInsert);
            contentText.text = content.Replace("<insert>", contentInsert);

            _okEvent = okEvent;
            _cancelEvent = cancelEvent;
        };

        if (view.isVisible)
            StartCoroutine(WaitUntilClose(action));
        else
        {
            action.Invoke();
        }
    }

    public void ActiveByBlock(ConfirmTable key)
    {
        UnityAction action = () =>
        {
            Open();

            string id = key.ToString();

            titleText.text = App.factory.confirmFactory.GetNormalTitle(id);
            contentText.text = App.factory.confirmFactory.GetNormalContent(id);

            okButton.SetActive(false);
            cancelButton.SetActive(false);

            isBlock = true;
        };

        if (view.isVisible)
            StartCoroutine(WaitUntilClose(action));
        else
        {
            action.Invoke();
        }
    }

    IEnumerator WaitUntilClose(UnityAction action)
    {
        action.Invoke();
        yield return new WaitUntil(() => view.isVisible == false);
    }

    public void Ok()
    {
        _okEvent?.Invoke();
        Close();
        App.system.soundEffect.Play("Button");
    }

    public void Cancel()
    {
        _cancelEvent?.Invoke();
        Close();
        App.system.soundEffect.Play("Button");
    }

    private void Open()
    {
        SetLastSibling();
        view.Show();
    }
    
    private void Close()
    {
        view.InstantHide();

        _okEvent = null;
        _cancelEvent = null;

        cancelButton.SetActive(true);
        isOnlyConfirm = false;
        ResetSibling();
    }

    private void SetLastSibling()
    {
        siblingIndex = transform.GetSiblingIndex();
        transform.SetAsLastSibling();
    }

    private void ResetSibling()
    {
        if (siblingIndex == -1)
            return;
        transform.SetSiblingIndex(siblingIndex);
        siblingIndex = -1;
    }

    public void Click()
    {
        if (view.isShowing)
            return;
        if (view.isHiding)
            return;
        
        if (isBlock)
            return;
        
        if (isOnlyConfirm)
            Ok();
        else
            Cancel();
    }
}