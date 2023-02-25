using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PriceTextHelper : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private TextMeshProUGUI tmpText;
    [SerializeField] private bool showText;

    [Button]
    public void SetText()
    {
        tmpText.text = text.text;
        text.gameObject.SetActive(showText);
    }
}
