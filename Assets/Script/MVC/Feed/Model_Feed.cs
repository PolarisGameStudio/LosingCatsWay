using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Feed : ModelBehavior
{
    private List<Cat> cats = new List<Cat>();

    public List<Cat> Cats
    {
        get => cats;
        set
        {
            cats = value;
            OnCatsChange(value);
        }
    }

    public ValueChange OnCatsChange;
}