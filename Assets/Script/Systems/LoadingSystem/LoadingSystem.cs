using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingSystem : MvcBehaviour
{
    public UIView view;

    [Title("Setting")]
    public float waitTime;

    public void PlayLoading()
    {
        Open();
        Close();
    }

    public void Open()
    {
       view.Show();
    }

    public void Close()
    {
        DOVirtual.DelayedCall(waitTime, () =>
        {
            view.Hide();
        });
    }
}
