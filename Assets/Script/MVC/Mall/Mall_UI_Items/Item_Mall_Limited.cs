using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item_Mall_Limited : MonoBehaviour
{
    public GameObject mask;

    public void Open()
    {
        mask.SetActive(true);
    }

    public void Close()
    {
        mask.SetActive(false);
    }
}
