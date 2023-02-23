using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;

public class PlayerRenameSystem : MvcBehaviour
{
    public TMP_InputField inputField;
    public GameObject cancelButton;
    public GameObject toolTipObject;
    public UIView uIView;

    [Title("Title")]
    [SerializeField] private GameObject normalTitle;
    [SerializeField] private GameObject tutorialTitle;

    private bool isFreeRename;

    public Callback OnRenameComplete;

    public void Open(bool isFree = false, bool canCancel = true)
    {
        normalTitle.SetActive(!App.system.tutorial.isTutorial);
        tutorialTitle.SetActive(App.system.tutorial.isTutorial);
        
        isFreeRename = isFree;
        cancelButton.SetActive(canCancel);
        toolTipObject.SetActive(!isFree);
        
        inputField.text = App.system.player.PlayerName;
        
        uIView.Show();
    }

    public void Close()
    {
        uIView.Hide();
    }

    public void Rename()
    {
        Item renameItem = App.factory.itemFactory.GetItem("ISL00003");
        
        if (!isFreeRename)
        {
            if (renameItem.Count <= 0)
            {
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_NoProps);
                return;
            }
        }

        if (!CheckInputExtension.CheckInputNameCanUse(inputField.text))
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_NullValue);
            return;
        }

        if (inputField.text == App.system.player.PlayerName)
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_NoSameName);
            return;
        }

        Close();

        ConfirmTable confirmTable = ConfirmTable.Hints_Rename;
        if (App.system.tutorial.isTutorial)
            confirmTable = ConfirmTable.Hints_Name;
        
        App.system.confirm.Active(confirmTable, () => 
        {
            App.system.player.PlayerName = inputField.text;
            if (!isFreeRename)
                renameItem.Count -= 1;
            App.SaveData();
            OnRenameComplete?.Invoke();
        }, uIView.Show);
    }
}
