using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_ChoosePedia : MvcBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject pawObject;
    [SerializeField] private GameObject lockObject;
    
    public void SetData(Sprite sprite, string title, bool isUnlock)
    {
        icon.sprite = sprite;
        titleText.text = title;
        pawObject.SetActive(isUnlock);
        lockObject.SetActive(!isUnlock);
    }

    public void Select()
    {
        int index = transform.GetSiblingIndex();
        App.controller.pedia.ChoosePedia(index);
    }
}
