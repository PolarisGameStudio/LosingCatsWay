using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Doozy.Runtime.UIManager.Containers;

public class PlayerRenameSystem : MvcBehaviour
{
    public TMP_InputField inputField;
    public GameObject cancelButton;
    public GameObject toolTipObject;
    public UIView uIView;

    bool canCancel;
    bool isFreeRename;

    public Callback OnRenameComplete;

    public bool IsFreeRename
    {
        get => isFreeRename;
        set
        {
            isFreeRename = value;
            toolTipObject.SetActive(!isFreeRename);
        }
    }

    public bool CanCancel
    {
        get => canCancel;
        set
        {
            canCancel = value;
            cancelButton.SetActive(value);
        }
    }

    public void Open()
    {
        uIView.Show();
        IsFreeRename = false;
        CanCancel = true;
        inputField.text = App.system.player.PlayerName;
    }

    public void Close()
    {
        uIView.Hide();
    }

    public void Rename()
    {
        Item renameItem = App.factory.itemFactory.GetItem("ISL00003");
        
        if (!IsFreeRename)
        {
            if (renameItem.Count <= 0)
            {
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix);
                return;
            }
        }

        if (!CheckInputExtension.CheckInputNameCanUse(inputField.text))
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.NotAllowBanWord);
            return;
        }

        if (inputField.text == App.system.player.PlayerName)
        {
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.NotAllowBanWord);
            return;
        }

        Close();
        
        App.system.confirm.Active(ConfirmTable.RenameConfirm, () => 
        {
            App.system.player.PlayerName = inputField.text;
            if (!IsFreeRename)
                renameItem.Count -= 1;
            OnRenameComplete?.Invoke();
        }, uIView.Show);
    }
}
