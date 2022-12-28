using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_ReadPedia : ViewBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject pawObject;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private Image border;
    [SerializeField] private Sprite[] borderSprites;
    
    public override void Init()
    {
        base.Init();
        App.model.pedia.OnSelectPediaIdChange += OnSelectPediaIdChange;
        App.model.pedia.OnSelectedPediaTypeChange += OnSelectedPediaTypeChange;
    }

    private void OnSelectedPediaTypeChange(object value)
    {
        int index = (int)value;
        if (index < 0)
            return;
        border.sprite = borderSprites[index];
    }

    private void OnSelectPediaIdChange(object value)
    {
        string id = value.ToString();

        icon.sprite = App.factory.pediaFactory.GetPediaSprite(id);
        
        bool unlock = App.factory.pediaFactory.GetPediaUnlock(id);
        if (id.Contains("WCI") || id.Contains("WCH") || id.Contains("WSK"))
            pawObject.SetActive(false);
        else
            pawObject.SetActive(unlock);

        titleText.text = App.factory.stringFactory.GetPediaTitle(id);
        contentText.text = App.factory.stringFactory.GetPediaContent(id).Replace(" ", "");
    }
}
