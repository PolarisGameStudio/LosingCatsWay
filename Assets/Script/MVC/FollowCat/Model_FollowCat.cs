using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_FollowCat : ModelBehavior
{
    private Cat selectedCat;

    public Cat SelectedCat
    {
        get => selectedCat;
        set
        {
            selectedCat = value;
            // TODO ValueChange
        }
    }
}
