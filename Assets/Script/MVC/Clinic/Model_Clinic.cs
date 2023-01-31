using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Clinic : ModelBehavior
{
    private int functionIndex;
    private List<Cat> myCats = new List<Cat>();
    private int catIndex;
    private Cat selectedCat;
    private Dictionary<string, int> payment = new Dictionary<string, int>();
    private string sickId;
    private int metCount;

    public int FunctionIndex
    {
        get => functionIndex;
        set
        {
            functionIndex = value;
            OnFunctionIndexChange(value);
        }
    }

    public List<Cat> MyCats
    {
        get => myCats;
        set
        {
            myCats = value;
            OnMyCatsChange(value);
        }
    }

    public int CatIndex
    {
        get => catIndex;
        set
        {
            catIndex = value;
            OnCatIndexChange(value);
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

    public Dictionary<string, int> Payment
    {
        get => payment;
        set
        {
            payment = value;
            OnPaymentChange(value);
        }
    }

    public string SickId
    {
        get => sickId;
        set
        {
            sickId = value;
            OnSickIdChange?.Invoke(value);
        }
    }

    public int MetCount
    {
        get => metCount;
        set
        {
            metCount = value;
            OnMetCountChange?.Invoke(value);
        }
    }

    public ValueChange OnFunctionIndexChange;
    public ValueChange OnMyCatsChange;
    public ValueChange OnCatIndexChange;
    public ValueChange OnSelectedCatChange;
    public ValueChange OnPaymentChange;
    public ValueChange OnSickIdChange;
    public ValueChange OnMetCountChange;
}
