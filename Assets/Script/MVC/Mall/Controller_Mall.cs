using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Controller_Mall : ControllerBehavior
{
    public void Open()
    {
        App.view.mall.Open();
    }

    public void Close()
    {
        App.view.mall.Close();
    }

    public void SelectPage(int index)
    {
        App.model.mall.SelectedPageIndex = index;
    }

    public void OpenPreviewPackageView(Reward[] rewards)
    {
        App.model.mall.PreviewPackageRewards = rewards;
        App.view.mall.OpenPreviewPackageView();
    }

    public void ClosePreviewPackageView()
    {
        App.view.mall.ClosePreviewPackageView();
    }
}
