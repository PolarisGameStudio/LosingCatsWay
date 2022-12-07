using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

public class ViewBehaviour : MvcBehaviour
{
    protected UIView UIView;
    
    // Start is called before the first frame update
    public void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        UIView = GetComponent<UIView>();
    }

    public virtual void Open()
    {
        UIView.Show();
    }

    public virtual void Close()
    {
        UIView.InstantHide();
    }
}