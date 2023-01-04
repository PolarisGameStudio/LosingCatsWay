using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PolyNav;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JustTestRoom : MonoBehaviour
{
    public Text text;
    private int i;

    private void Start()
    {
        i = PlayerPrefs.GetInt("Test");
        text.text = i.ToString();
    }

    [Button]
    public void Test()
    {
        i++;
        PlayerPrefs.SetInt("Test", i);

        SceneManager.LoadScene("Test");
    }
}