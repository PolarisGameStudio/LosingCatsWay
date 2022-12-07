using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIView))]
public class NoticeSystem : MvcBehaviour
{
    public UIView uiView;

    public void Open()
    {
        uiView.Show();
    }

    public void Close()
    {
        uiView.Hide();
    }
}
