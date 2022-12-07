using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.ComponentModel;

[ExecuteInEditMode]
public class TmpValueChange : MonoBehaviour
{
    public TextMeshProUGUI mainText;
    public TextMeshProUGUI[] updateTexts;

    public bool useLateUpdate = false;

    private void Update()
    {
        if (updateTexts.Length <= 0) return;
        if (useLateUpdate) return;

        for (int i = 0; i < updateTexts.Length; i++)
        {
            updateTexts[i].text = mainText.text;
        }
    }

    private void LateUpdate()
    {
        if (updateTexts.Length <= 0) return;
        if (!useLateUpdate) return;

        for (int i = 0; i < updateTexts.Length; i++)
        {
            updateTexts[i].text = mainText.text;
        }
    }
}
