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
    
    public void SetData(string id)
    {
        Sprite sprite = App.factory.pediaFactory.GetPediaSprite(id);
        string title = App.factory.stringFactory.GetPediaTitle(id);
        bool unlock = App.factory.pediaFactory.GetPediaUnlock(id);
        
        //Paw
        if (id.Contains("WCI") || id.Contains("WCH") || id.Contains("WSK"))
            pawObject.SetActive(false);
        else
            pawObject.SetActive(unlock);
        
        //Color
        if (id.Contains("WCI"))
            ChangeColor(1);
        else if (id.Contains("WCH"))
            ChangeColor(2);
        else if (id.Contains("WSK"))
            ChangeColor(3);
        else
            ChangeColor(0);
        
        icon.sprite = sprite;
        titleText.text = title;
        lockObject.SetActive(!unlock);
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
