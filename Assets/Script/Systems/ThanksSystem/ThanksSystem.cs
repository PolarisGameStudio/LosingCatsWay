using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.UI;

public class ThanksSystem : MvcBehaviour
{
    [SerializeField] private UIView view;
    [SerializeField] private Scrollbar scrollbar;

    public void Open()
    {
        scrollbar.value = 1;
        view.Show();
    }

    public void Close()
    {
        view.InstantHide();
    }
}
