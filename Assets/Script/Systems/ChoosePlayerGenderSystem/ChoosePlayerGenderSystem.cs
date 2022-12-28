using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;

public class ChoosePlayerGenderSystem : MvcBehaviour
{
    [SerializeField] private UIView uiView;
    [SerializeField] private GameObject[] selectedObjects;
    [SerializeField] private GameObject[] selectedCorners;

    private int genderIndex = -1;

    public void Init()
    {
        int gender = App.system.player.PlayerGender;
        if (gender != -1)
            return;
        Open();
    }

    [Button]
    public void Open()
    {
        uiView.Show();
    }

    private void Close()
    {
        uiView.InstantHide();
        App.system.tutorial.Init();
    }

    public void SelectGender(int index)
    {
        App.system.soundEffect.Play("Button");
        genderIndex = index;

        for (int i = 0; i < selectedObjects.Length; i++)
        {
            if (i == index)
            {
                selectedObjects[i].SetActive(true);
                selectedCorners[i].transform.DOScale(Vector2.one, 0.25f).From(new Vector2(1.1f, 1.1f));
            }
            else
            {
                selectedCorners[i].transform.DOKill();
                selectedObjects[i].SetActive(false);
            }
        }
    }

    public void ConfirmGender()
    {
        App.system.soundEffect.Play("Button");
        
        if (genderIndex == -1)
            return;

        string genderString = string.Empty;
        switch (genderIndex)
        {
            case 0:
                genderString = App.factory.stringFactory.GetBoyString();
                break;
            case 1:
                genderString = App.factory.stringFactory.GetGirlString();
                break;
        }
        
        App.system.confirm.ActiveByInsert(ConfirmTable.ChooseGenderConfirm, null, genderString, () =>
        {
            App.system.player.PlayerGender = genderIndex;
            RefreshPlayerIcon(genderIndex);
            Close();
        });
    }

    private void RefreshPlayerIcon(int index)
    {
        string id = string.Empty;
        switch (index)
        {
            case 0:
                id = "PIC001";
                break;
            case 1:
                id = "PIC002";
                break;
        }

        App.system.player.UsingIcon = id;
    }
}
