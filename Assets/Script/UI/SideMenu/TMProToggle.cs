using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Toggle), typeof(TextMeshProUGUI))]
public class TMProToggle : MonoBehaviour
{
    public Toggle toggle;
    public TextMeshProUGUI text;

    #region Toggle text

    public bool toggleText = false;

    [ShowIf("toggleText")] public string isOnText = "";
    [ShowIf("toggleText")] public string isOffText = "";

    #endregion

    #region Toggle color

    public bool toggleColor = false;

    [ShowIf("toggleColor")] public Color32 isOnColor = Color.white;
    [ShowIf("toggleColor")] public Color32 isOffColor = Color.white;

    #endregion

    public void ToggleText()
    {
        if (toggle == null || text == null)
        {
            Debug.LogError("Object doesn't have toggle or TMPro component.");
            return;
        }

        if (toggle.isOn)
        {
            text.text = isOnText;
            return;
        }

        text.text = isOffText;
    }

    public void ToggleColor()
    {
        if (toggle == null || text == null)
        {
            Debug.LogError("Object doesn't have toggle or TMPro component.");
            return;
        }

        if (toggle.isOn)
        {
            text.color = isOnColor;
            return;
        }

        text.color = isOffColor;
    }
}
