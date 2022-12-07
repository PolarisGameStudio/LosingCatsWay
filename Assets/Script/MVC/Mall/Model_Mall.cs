using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Mall : ModelBehavior
{
    public Dictionary<string, PurchaseRecord> PurchaseRecords;

    private int selectedPageIndex = -1;

    private Reward[] previewPackageRewards;

    public int SelectedPageIndex
    {
        get => selectedPageIndex;
        set
        {
            onSelectedPageIndexChange?.Invoke(selectedPageIndex, value);
            selectedPageIndex = value;
        }
    }

    public Reward[] PreviewPackageRewards
    {
        get => previewPackageRewards;
        set
        {
            onPreviewPackageRewardsChange?.Invoke(value);
            previewPackageRewards = value;
        }
    }

    public ValueFromToChange onSelectedPageIndexChange;
    public ValueChange onPreviewPackageRewardsChange;

}