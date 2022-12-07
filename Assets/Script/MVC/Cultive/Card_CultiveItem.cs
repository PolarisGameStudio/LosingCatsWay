using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card_CultiveItem : MvcBehaviour
{
    [Title("StatusIcon")]
    [SerializeField] private GameObject satietyIcon;
    [SerializeField] private GameObject moistureIcon;
    [SerializeField] private GameObject funIcon;

    [Title("Item")]
    [SerializeField] private Image itemIcon; //物件圖示
    [SerializeField] private GameObject countTextBg;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI nameText;

    [Title("Sensor")]
    public Drag_CultiveItem dragSensor;

    public void SetData(Item item)
    {
        dragSensor.icon.sprite = item.icon;
        dragSensor.item = item;

        itemIcon.sprite = item.icon;

        countTextBg.SetActive(item.itemType != ItemType.Play);
        countText.text = item.Count.ToString("00");
        nameText.text = item.Name;

        satietyIcon.SetActive(item.ForSatiety);
        moistureIcon.SetActive(item.ForMoisture);
        funIcon.SetActive(item.ForFun);
    }
}
