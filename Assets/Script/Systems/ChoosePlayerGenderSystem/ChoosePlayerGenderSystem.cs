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

    [Button]
    public void Open()
    {
        uiView.Show();
    }

    private void Close()
    {
        uiView.InstantHide();
    }

    public void SelectGender(int index)
    {
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
        if (genderIndex == -1)
            return;
        
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            App.system.player.PlayerGender = genderIndex;
            Close();
        });
    }
}
