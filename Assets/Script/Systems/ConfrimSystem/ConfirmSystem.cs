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

    [SerializeField] private GameObject cancelButton;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;

    private UnityAction _okEvent;
    private UnityAction _cancelEvent;

    public ConfirmSystem OnlyConfirm()
    {
        cancelButton.SetActive(false);
        return this;
    }

    public ConfirmSystem Active(ConfirmTable key, UnityAction okEvent = null, UnityAction cancelEvent = null)
    {
        UnityAction action = () =>
        {
            view.Show();

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

        return this;
    }
    
    public ConfirmSystem ActiveByInsert(ConfirmTable key, string titleInsert = "", string contentInsert = "", UnityAction okEvent = null, UnityAction cancelEvent = null)
    {
        UnityAction action = () =>
        {
            view.Show();

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

        return this;
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

    private void Close()
    {
        view.InstantHide();

        _okEvent = null;
        _cancelEvent = null;

        cancelButton.SetActive(true);
    }
}