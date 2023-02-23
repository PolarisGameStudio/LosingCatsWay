using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnlockGridSystem : MvcBehaviour
{
    [Title("View")] [SerializeField] private UIView uiView;

    [Title("UI")] [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemCountText;

    [Title("Unlock")] public int[] unlockLevels;
    public Reward[] unlockItems;

    public void Init()
    {
        RefreshEffect();
    }

    public void Active()
    {
        if (!IsCanUnlock())
            return;

        int gridLevel = App.system.player.GridSizeLevel;
        Item item = unlockItems[gridLevel].item;

        int count = App.system.player.CatMemory;

        if (item.id == "Diamond")
            count = App.system.player.Diamond;

        int needCount = unlockItems[gridLevel].count;

        itemNameText.text = item.Name;
        itemCountText.text = $"{count}/{needCount}";
        itemIcon.sprite = item.icon;

        uiView.Show();
    }

    public void Confirm()
    {
        int gridLevel = App.system.player.GridSizeLevel;

        Item item = unlockItems[gridLevel].item;
        int needCount = unlockItems[gridLevel].count;

        if (item.id == "Diamond")
        {
            if (!App.system.player.ReduceDiamond(needCount))
            {
                App.system.confirm.Active(ConfirmTable.Hints_NoDiamond);
                return;
            }
        }
        if (item.id == "CatMemory")
        {
            if (!App.system.player.ReduceCatMemory(needCount))
            {
                App.system.confirm.Active(ConfirmTable.Hints_NoMemory);
                return;
            }
        }

        App.system.player.GridSizeLevel++;

        App.SaveData();

        App.system.transition.OnlyOpen(() =>
        {
            PlayerPrefs.SetString("FriendRoomId", "JUSTBUILD");
            SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
        });
    }

    public void Cancel()
    {
        uiView.Hide();
    }

    private bool IsCanUnlock()
    {
        int gridLevel = App.system.player.GridSizeLevel;

        if (gridLevel == 13)
            return false;

        int unlockLevel = unlockLevels[gridLevel];
        int level = App.system.player.Level;

        return level >= unlockLevel;
    }

    private void Close()
    {
        uiView.InstantHide();
    }

    public void RefreshEffect()
    {
        if (!IsCanUnlock())
            return;
        List<OutSideSensor> sensors = App.system.grid.OutSideSensors;
        for (int i = 0; i < sensors.Count; i++)
            sensors[i].effect.SetActive(true);
    }
}