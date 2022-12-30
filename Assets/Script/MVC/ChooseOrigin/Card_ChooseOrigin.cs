using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_ChooseOrigin : MvcBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI selectedText;
    [SerializeField] private TextMeshProUGUI deselectText;
    public CanvasGroup selectedCanvasGroup;
    public CanvasGroup deselectCanvasGroup;
    
    public void SetData(Room room)
    {
        icon.sprite = room.Image;
        selectedText.text = room.Name;
        deselectText.text = room.Name;
    }

    public void Select()
    {
        int index = transform.GetSiblingIndex();
        App.controller.chooseOrigin.Select(index);
    }
}
