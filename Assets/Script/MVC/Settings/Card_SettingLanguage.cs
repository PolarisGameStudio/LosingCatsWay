using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_SettingLanguage : MonoBehaviour
{
    [SerializeField] private GameObject selectedObject;

    public void Select()
    {
        selectedObject.SetActive(true);
    }

    public void Deselect()
    {
        selectedObject.SetActive(false);
    }
}
