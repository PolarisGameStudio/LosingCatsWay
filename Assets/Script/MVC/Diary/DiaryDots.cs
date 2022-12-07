using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryDots : MvcBehaviour
{
    [SerializeField] private GameObject dot;

    public void Active(bool value)
    {
        dot.SetActive(value);
    }
}
