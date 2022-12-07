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
        inputField.text = "InputYourName...";
        uIView.Show();

        IsFreeRename = false;
        CanCancel = true;
    }

    public void Close()
    {
        uIView.Hide();
    }

    //TODO OnSelect �M�ſ�J��
    public void Rename()
    {
        if (!IsFreeRename)
        {
            if (App.system.player.Diamond < 2000)
            {
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.NotEnoughDiamond);
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

        App.system.confirm.Active(ConfirmTable.RenameConfirm, () => 
        {
            App.system.player.PlayerName = inputField.text;
            if (!IsFreeRename) App.system.player.Diamond -= 2000;
            uIView.Hide();
            OnRenameComplete?.Invoke();
        });
    }
}
