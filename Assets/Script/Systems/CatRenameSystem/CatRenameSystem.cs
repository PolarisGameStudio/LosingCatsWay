using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;

public class CatRenameSystem : MvcBehaviour
{
    [SerializeField] private CatSkin catSkin;

    [Title("UI")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private UIView view;

    [Title("Bg")] [SerializeField] private Image bgImage;

    [Title("Title")]
    [SerializeField] private GameObject normalTitle;
    [SerializeField] private GameObject firstTitle;

    [HideInInspector] public CloudCatData cloudCatData;

    private UnityAction onConfirm;
    private UnityAction onCancel;

    public CatRenameSystem CantCancel()
    {
        closeButton.SetActive(false);
        return this;
    }

    public void Active(CloudCatData cloudCatData, string location, UnityAction OnConfirm = null, UnityAction OnCancel = null)
    {
        this.cloudCatData = cloudCatData;
        catSkin.ChangeSkin(cloudCatData);

        inputField.text = cloudCatData.CatData.CatName;

        bgImage.sprite = App.factory.catFactory.GetCatLocationSprite(location);
        
        onConfirm = OnConfirm;
        onCancel = OnCancel;
        
        normalTitle.SetActive(cloudCatData.CatHealthData.IsChip);
        firstTitle.SetActive(!cloudCatData.CatHealthData.IsChip);

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
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_NullValue);
            return;
        }
        
        // TODO 價錢
        
        ConfirmTable confirmTable = ConfirmTable.Hints_Rename;
        if (App.system.tutorial.isTutorial)
            confirmTable = ConfirmTable.Hints_Name;

        App.system.confirm.Active(confirmTable, () =>
        {
            cloudCatData.CatData.CatName = inputField.text;
            onConfirm?.Invoke();
            view.Hide();
            catSkin.SetActive(false);
        });
    }
}
