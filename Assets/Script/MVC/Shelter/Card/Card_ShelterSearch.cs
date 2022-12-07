using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card_ShelterSearch : MvcBehaviour
{
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI idText;
    [SerializeField] private Button button;

    public void SetData(CloudCatData cloudCatData)
    {
        catSkin.ChangeSkin(cloudCatData);
        nameText.text = cloudCatData.CatData.CatName;
        idText.text = $"ID: {cloudCatData.CatData.CatId}";
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        catSkin.SetActive(active);
    }
}
