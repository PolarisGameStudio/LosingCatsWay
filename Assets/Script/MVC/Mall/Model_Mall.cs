using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Mall : ModelBehavior
{
    public Dictionary<string, PurchaseRecord> PurchaseRecords;

    private int selectedPageIndex = -1;

    private Reward[] previewPackageRewards;

    private string previewPackageId;

    private int ruleIndex = -1;

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

    public int RuleIndex
    {
        get => ruleIndex;
        set
        {
            ruleIndex = value;
            OnRuleIndexChange(value);
        }
    }

    public string PreviewPackageId
    {
        get => previewPackageId;
        set
        {
            previewPackageId = value;
            OnPreviewPackageIdChange?.Invoke(value);
        }
    }

    public ValueFromToChange onSelectedPageIndexChange;
    public ValueChange onPreviewPackageRewardsChange;
    public ValueChange OnRuleIndexChange;
    public ValueChange OnPreviewPackageIdChange;
}