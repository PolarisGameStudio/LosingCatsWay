using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.UI;

public class FlowTask_E21_RenameCat : FlowTask
{
    private UIView view;

    public override void Enter()
    {
        view = GetComponent<UIView>();
        view.Show();

        //var catData = CatExtension.GenerateCatData();

        //App.system.catRename.CantCancel().Active(catData, () =>
        //{
        //    //App.system.cat.CreateCat(catData);
        //    Exit();
        //});
    }

    public override void Exit()
    {
        view.InstantHide();

        DOVirtual.DelayedCall(0.4f, () =>
        {
            base.Exit();
        });
    }

}