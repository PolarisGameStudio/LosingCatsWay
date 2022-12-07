using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TempNote : MonoBehaviour
{
    [InfoBox("This script is just for writing something you may forgot.")]

    [MultiLineProperty(5)]
    public string[] Note;
}
