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

    [Title("Unlock")] 
    public int[] unlockLevels;
    public Reward[] unlockItems;

    private int count = 0;

    public void Init()
    {
        if (IsCanUnlock())
            print("開特效");
    }

    public void Active()
    {
        // if (!IsCanUnlock())
        //     return;

        int gridLevel = App.system.player.GridSizeLevel - 1;
        Item item = unlockItems[gridLevel].item;

        if (item.id == "Money")
            count = App.system.player.Coin;
        else
            count = App.system.player.Diamond;

        int needCount = unlockItems[gridLevel].count;
        
        itemNameText.text = item.Name;
        itemCountText.text = $"{count}/{needCount}";
        itemIcon.sprite = item.icon;

        uiView.Show();
    }

    public void Confirm()
    {
        int gridLevel = App.system.player.GridSizeLevel - 1;
        int needCount = unlockItems[gridLevel].count;
        
        if (count < needCount)
        {
            App.system.confirm.Active(ConfirmTable.NotEnoughCoin);
            return;
        }

        App.system.player.GridSizeLevel++;
        App.system.cloudSave.SaveCloudSaveData();

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

        if (gridLevel == 11)
            return false;

        int unlockLevel = unlockLevels[gridLevel - 1];
        int level = App.system.player.Level;

        return level >= unlockLevel;
    }

    private void Close()
    {
        uiView.InstantHide();
    }
}