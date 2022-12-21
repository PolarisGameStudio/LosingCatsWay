using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_ChoosePedia : MvcBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject pawObject;
    [SerializeField] private GameObject lockObject;
    [SerializeField] private Image border;

    [Title("Color")] [SerializeField] private Sprite[] borderColors;
    [SerializeField] private Material[] textMaterials;
    [SerializeField] private Color32[] textColors;
    
    public void SetData(Sprite sprite, string title, bool isUnlock)
    {
        icon.sprite = sprite;
        titleText.text = title;
        pawObject.SetActive(isUnlock);
        lockObject.SetActive(!isUnlock);
    }

    public void ChangeColor(int index)
    {
        border.sprite = borderColors[index];
        titleText.fontMaterial = textMaterials[index];
        titleText.color = textColors[index];
    }

    public void Select()
    {
        int index = transform.GetSiblingIndex();
        App.controller.pedia.ChoosePedia(index);
    }
}
