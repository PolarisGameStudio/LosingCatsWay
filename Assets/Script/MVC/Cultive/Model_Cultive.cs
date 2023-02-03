using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Cultive : ModelBehavior
{
    private Cat selectedCat;
    private int selectedType = -1;
    private List<Item> selectedItems = new List<Item>();
    private Item dragItem;
    private int usingLitterIndex = -1;
    private DateTime nextCleanDateTime;
    private int cleanLitterCount;
    private int openFromIndex;
    private int selectedTab = -1;
    private List<Item> skinItems;
    private int selectedSkinIndex;

    public string dontLikePlayId;
    
    public Cat SelectedCat
    {
        get => selectedCat;
        set
        {
            selectedCat = value;
            OnSelectedCatChange(value);
        }
    }

    public int SelectedType
    {
        get => selectedType;
        set
        {
            selectedType = value;
            OnSelectedTypeChange(value);
        }
    }

    public List<Item> SelectedItems
    {
        get => selectedItems;
        set
        {
            selectedItems = value;
            OnSelectedItemsChange(value);
        }
    }

    public Item DragItem
    {
        get => dragItem;
        set
        {
            dragItem = value;
        }
    }

    public int UsingLitterIndex
    {
        get => usingLitterIndex;
        set => usingLitterIndex = value;
    }

    public DateTime NextCleanDateTime
    {
        get => nextCleanDateTime;
        set
        {
            nextCleanDateTime = value;
            OnNextCleanDateTimeChange(value);
        }
    }

    public int CleanLitterCount
    {
        get => cleanLitterCount;
        set
        {
            cleanLitterCount = value;
            OnCleanLitterCountChange(value);
        }
    }

    public int OpenFromIndex
    {
        get => openFromIndex;
        set => openFromIndex = value;
    }

    public int SelectedTab
    {
        get => selectedTab;
        set
        {
            selectedTab = value;
            OnSelectedTabChange?.Invoke(value);
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

    public ValueChange OnSelectedCatChange;
    public ValueChange OnSelectedTypeChange;
    public ValueChange OnSelectedItemsChange;
    public ValueChange OnNextCleanDateTimeChange;
    public ValueChange OnCleanLitterCountChange;
    public ValueChange OnSelectedTabChange;
    public ValueChange OnSkinItemsChange;
    public ValueChange OnSelectedSkinIndexChange;
}
