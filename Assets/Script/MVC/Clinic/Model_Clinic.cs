using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Clinic : ModelBehavior
{
    private int viewIndex = 0;
    private int functionIndex;
    private List<Cat> myCats = new List<Cat>();
    private int catIndex;
    private Cat selectedCat;
    private Dictionary<string, int> payment = new Dictionary<string, int>();

    /// <summary>
    /// 返回鍵回去哪個介面
    /// </summary>
    public int ViewIndex
    {
        get => viewIndex;
        set => viewIndex = value;
    }

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

    public ValueChange OnFunctionIndexChange;
    public ValueChange OnMyCatsChange;
    public ValueChange OnCatIndexChange;
    public ValueChange OnSelectedCatChange;
    public ValueChange OnPaymentChange;
}
