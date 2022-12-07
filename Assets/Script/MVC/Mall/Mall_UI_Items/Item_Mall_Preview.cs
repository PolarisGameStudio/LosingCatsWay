using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item_Mall_Preview : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemCountText;
    public Image itemImage;

    public void SetData(Reward reward)
    {
        itemNameText.text = reward.item.Name;
        itemCountText.text = "x" + reward.count;
        itemImage.sprite = reward.item.content;
    }
}
