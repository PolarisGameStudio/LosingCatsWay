using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UseItemSystem : MvcBehaviour
{
    [Title("View")] [SerializeField] private UIView uiView;
    
    [Title("UI")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptText;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemCountText;

    public Callback OnCancel;
    public Callback OnConfirm;

    public void Active(Item item, int useCount, Callback onConfirm = null, Callback onCancel = null)
    {
        titleText.text = App.factory.stringFactory.GetUseItemTitle(item.id);
        descriptText.text = item.Description;

        itemNameText.text = item.Name;
        itemCountText.text = $"{item.Count}/{useCount}";

        itemIcon.sprite = item.icon;

        OnConfirm = onConfirm;
        OnCancel = onCancel;
        
        Open();
    }

    private void Open()
    {
        uiView.Show();
    }

    private void Close()
    {
        uiView.InstantHide();
    }

    public void Confirm()
    {
        OnConfirm?.Invoke();
        OnConfirm = null;
        Close();
    }

    public void Cancel()
    {
        OnCancel?.Invoke();
        OnCancel = null;
        Close();
    }
}
