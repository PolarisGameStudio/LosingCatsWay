using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_CatchItem : MvcBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject mask;
    public CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI countText;
    public RectTransform rectTransform;

    private bool isCanShow;

    public void SetData(Item_CatchCat itemCatchCat, int tmpCount)
    {
        iconImage.sprite = itemCatchCat.icon;
        nameText.text = App.factory.stringFactory.GetItemName(itemCatchCat.id);
        countText.text = itemCatchCat.level == 0 ? "∞" : tmpCount.ToString();

        isCanShow = itemCatchCat.level == 0 || tmpCount > 0;
    }

    /// 是否被遮罩無法點擊
    public void SetInteractable(bool value)
    {
        if (!isCanShow)
        {
            mask.SetActive(true);
            return;
        }
        
        mask.SetActive(!value);
        canvasGroup.interactable = value;
    }
}
