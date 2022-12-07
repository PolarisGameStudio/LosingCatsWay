using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Toggle))]
public class ToggleEvent : MonoBehaviour
{
    public UnityEvent IsOn;
    public UnityEvent IsOff;

    Toggle toggle;

    private void Start()
    {
        toggle = GetComponent<Toggle>();

        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener((bool isOn) => Toggle(isOn));
    }

    void Toggle(bool isOn)
    {
        if (isOn)
            IsOn?.Invoke();
        else
            IsOff?.Invoke();
    }
}
