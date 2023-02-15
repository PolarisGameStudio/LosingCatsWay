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
        if (IsCanUnlock())
            print("開特效");
    }

    public void Active()
    {
        if (!IsCanUnlock())
            return;

        int gridLevel = App.system.player.GridSizeLevel;
        Item item = unlockItems[gridLevel].item;

        int count = App.system.player.Diamond;

        if (item.id == "Money")
            count = App.system.player.Coin;

        int needCount = unlockItems[gridLevel].count;

        itemNameText.text = item.Name;
        itemCountText.text = $"{count}/{needCount}";
        itemIcon.sprite = item.icon;

        uiView.Show();
    }

    public async void Confirm()
    {
        int gridLevel = App.system.player.GridSizeLevel - 1;

        Item item = unlockItems[gridLevel].item;
        int needCount = unlockItems[gridLevel].count;

        if (item.id == "Money")
        {
            if (!App.system.player.ReduceMoney(needCount))
            {
                App.system.confirm.Active(ConfirmTable.NotEnoughCoin);
                return;
            }
        }
        else
        {
            if (!App.system.player.ReduceDiamond(needCount))
            {
                App.system.confirm.Active(ConfirmTable.NotEnoughDiamond);
                return;
            }
        }

        App.system.player.GridSizeLevel++;

        // 要對每個房間去往上移一格
        var rooms = App.system.room.MyRooms;
        for (int i = 0;
             i < rooms.Count;
             i++)
        {
            rooms[i].x++;
            rooms[i].y++;
        }

        App.system.cloudSave.SaveCloudSaveData();
        App.system.cloudSave.SaveCloudCatDatas();

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
}