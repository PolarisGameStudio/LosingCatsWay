using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

public class View_PediaCats : ViewBehaviour
{
    [SerializeField] private UIView chooseCatView;
    [SerializeField] private UIView readCatView;

    public override void Open()
    {
        base.Open();
        CloseReadCat();
        OpenChooseCat();
    }

    public override void Close()
    {
        CloseChooseCat();
        CloseReadCat();
        base.Close();
    }

    private void OpenChooseCat()
    {
        chooseCatView.Show();
    }

    private void OpenReadCat()
    {
        readCatView.Show();
    }

    private void CloseChooseCat()
    {
        chooseCatView.InstantHide();
    }

    private void CloseReadCat()
    {
        readCatView.InstantHide();
    }
}
