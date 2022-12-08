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
    
    public override void Init()
    {
        base.Init();
        App.model.pedia.OnSelectPediaIdChange += OnSelectPediaIdChange;
    }

    private void OnSelectPediaIdChange(object value)
    {
        string id = value.ToString();

        icon.sprite = App.factory.pediaFactory.GetPediaSprite(id);
        
        bool unlock = App.factory.pediaFactory.GetPediaUnlock(id);
        pawObject.SetActive(unlock);

        titleText.text = App.factory.stringFactory.GetPediaTitle(id);
        contentText.text = App.factory.stringFactory.GetPediaContent(id).Replace(" ", "");
    }
}
