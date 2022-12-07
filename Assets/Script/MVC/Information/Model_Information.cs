using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Information : ModelBehavior
{
    private List<Cat> myCats = new List<Cat>();
    private Cat selectedCat;
    private int tabIndex = -1;
    private List<Item> skinItems;
    private int selectedSkinIndex;

    public List<Cat> MyCats
    {
        get => myCats;
        set
        {
            myCats = value;
            OnMyCatsChange(value);
        }
    }

    public Cat SelectedCat
    {
        get => selectedCat;
        set
        {
            selectedCat = value;
            OnSelectedCatChange(value);
        }
    }

    public int TabIndex
    {
        get => tabIndex;
        set
        {
            tabIndex = value;
            OnTabIndexChange(value);
        }
    }

    public List<Item> SkinItems
    {
        get => skinItems;
        set
        {
            skinItems = value;
            OnSkinItemsChange?.Invoke(value);
        }
    }

    public int SelectedSkinIndex
    {
        get => selectedSkinIndex;
        set
        {
            selectedSkinIndex = value;
            OnSelectedSkinIndexChange?.Invoke(value);
        }
    }

    public ValueChange OnMyCatsChange;
    public ValueChange OnSelectedCatChange;
    public ValueChange OnTabIndexChange;
    public ValueChange OnSkinItemsChange;
    public ValueChange OnSelectedSkinIndexChange;
}
