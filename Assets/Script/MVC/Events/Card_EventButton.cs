using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_EventButton : MvcBehaviour
{
    public GameObject SelectedObject;
    public GameObject dot;

    public void Init()
    {
        dot.SetActive(true);
    }

    public void MarkAsRead()
    {
        dot.SetActive(false);
    }

    public void Select()
    {
        SelectedObject.SetActive(true);
    }

    public void Deselect()
    {
        SelectedObject.SetActive(false);
    }
}
