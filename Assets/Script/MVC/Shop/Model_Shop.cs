using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Shop : ModelBehavior
{
    private int selectedType = -1;
    private List<Item> selectedItems = new List<Item>();
    private Item selectedItem;
    private int buyCount = 0;
    private int totalAmount = 0;

    #region Properties

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

    public Item SelectedItem
    {
        get => selectedItem;
        set
        {
            selectedItem = value;
            OnSelectedItemChange(value);
        }
    }

    public int BuyCount
    {
        get => buyCount;
        set
        {
            buyCount = value;
            OnBuyCountChange(value);
        }
    }

    public int TotalAmount
    {
        get => totalAmount;
        set
        {
            totalAmount = value;
            OnTotalAmountChange(value);
        }
    }

    #endregion

    #region ValueChange

    public ValueChange OnSelectedTypeChange;
    public ValueChange OnSelectedItemsChange;
    public ValueChange OnSelectedItemChange;
    public ValueChange OnBuyCountChange;
    public ValueChange OnTotalAmountChange;

    #endregion
}
