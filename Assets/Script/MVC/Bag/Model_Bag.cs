using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Bag : ModelBehavior
{
    private List<Item> selectedItems;
    private Item selectedItem;
    private int type = -1;

    public List<Item> SelectedItems
    {
        get => selectedItems;
        set
        {
            selectedItems = value;
            onSelectedItemsChange(value);
        }
    }

    public Item SelectedItem
    {
        get => selectedItem;
        set
        {
            selectedItem = value;
            onSelectedItemChange(value);
        }
    }

    public int Type
    {
        get => type;
        set
        {
            onTypeChange(type, value);
            type = value;
        }
    }

    public ValueChange onSelectedItemsChange;
    public ValueChange onSelectedItemChange;
    public ValueFromToChange onTypeChange;
}