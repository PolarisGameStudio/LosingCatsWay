using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DataVisualization_Circle : MonoBehaviour
{
    [Title("Setting")] [SerializeField] private float r;
    [SerializeField] private bool showValues;

    [Title("UI")] 
    [SerializeField] private Image[] images;
    [SerializeField] private TextMeshProUGUI[] texts;

    [Title("Value"), ShowIf("showValues")]
    [ShowIf("showValues"), SerializeField] private float[] values;

    [Button]
    public void Test()
    {
        SetData(values);
    }

    public void SetData(float[] values)
    {
        float total = 0;

        for (int i = 0; i < values.Length; i++)
        {
            total += values[i];
            // texts[i].text = (values[i] / total * 100).ToString("0") + "%";
        }

        float prevPercent = 0;

        for (int i = 0; i < values.Length; i++)
        {
            float percent = values[i] / total;
            texts[i].text = (percent * 100).ToString("0") + "%";
            
            images[i].fillAmount = prevPercent + percent;

            // 字的位置
            float angle = (prevPercent + (prevPercent + percent)) * 0.5f * 360f;
            angle = -(angle - 90);
            
            float x = r * Mathf.Cos(angle * Mathf.PI / 180);
            float y = r * Mathf.Sin(angle * Mathf.PI / 180);

            texts[i].transform.localPosition = new Vector2(x, y);

            prevPercent += percent;
        }
    }
}