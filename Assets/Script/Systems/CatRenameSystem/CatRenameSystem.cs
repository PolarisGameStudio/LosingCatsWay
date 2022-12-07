using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Doozy.Runtime.UIManager.Containers;

public class CatRenameSystem : MvcBehaviour
{
    [SerializeField] private CatSkin catSkin;
    [Space(20)]

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private UIView view;

    [HideInInspector] public CloudCatData cloudCatData;

    private UnityAction onConfirm;
    private UnityAction onCancel;

    public CatRenameSystem CantCancel()
    {
        closeButton.SetActive(false);
        return this;
    }

    public void Active(CloudCatData cloudCatData, UnityAction OnConfirm = null, UnityAction OnCancel = null)
    {
        this.cloudCatData = cloudCatData;
        catSkin.ChangeSkin(cloudCatData);

        inputField.text = cloudCatData.CatData.CatName;

        onConfirm = OnConfirm;
        onCancel = OnCancel;

        view.Show();
        catSkin.SetActive(true);
    }

    public void Cancel()
    {
        view.InstantHide();
        onCancel?.Invoke();
    }

    //確認取名
    public void Confirm()
    {
        if (!CheckInputExtension.CheckInputNameCanUse(inputField.text)) //檢查名字
        {
            //不可用
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.NotAllowBanWord);
            return;
        }
        
        // TODO 價錢

        App.system.confirm.Active(ConfirmTable.RenameConfirm, () =>
        {
            cloudCatData.CatData.CatName = inputField.text;
            App.system.cloudSave.UpdateCloudCatData(cloudCatData);

            onConfirm?.Invoke();
            view.Hide();
            catSkin.SetActive(false);
        });
    }
}
