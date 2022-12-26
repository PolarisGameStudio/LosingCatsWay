using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DataVisualization_Bar : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField] private float minX = -245f;
    [SerializeField] private float maxX = 245f;
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
        float totalX = Mathf.Abs(minX) + Mathf.Abs(maxX);
        
        for (int i = 0; i < values.Length; i++)
        {
            float percent = values[i] / total;
            texts[i].text = (percent * 100).ToString("0") + "%";

            images[i].fillAmount = prevPercent + percent;
            
            // 字的位置
            float x = minX + (prevPercent + (prevPercent + percent)) * 0.5f * totalX;
            texts[i].transform.localPosition = new Vector2(x, 0);

            prevPercent += percent;
        }
    }
}
