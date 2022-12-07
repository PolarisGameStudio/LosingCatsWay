using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBehavior : MvcBehaviour
{
    public delegate void ValueChange(object value);
    public delegate void ValueFromToChange(object from,object to);
}
