using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Hospital : ModelBehavior
{
    private int functionIndex = -1;
    private List<Cat> cats;
    private int catIndex = -1;
    private Cat selectedCat;

    public int FunctionIndex
    {
        get => functionIndex;
        set
        {
            functionIndex = value;
            OnFunctionIndexChange?.Invoke(value);
        }
    }

    public List<Cat> Cats
    {
        get => cats;
        set
        {
            cats = value;
            OnCatsChange?.Invoke(value);
        }
    }

    public int CatIndex
    {
        get => catIndex;
        set
        {
            catIndex = value;
            OnCatIndexChange?.Invoke(value);
        }
    }

    public Cat SelectedCat
    {
        get => selectedCat;
        set
        {
            selectedCat = value;
            OnSelectedCatChange?.Invoke(value);
        }
    }

    public ValueChange OnFunctionIndexChange;
    public ValueChange OnCatsChange;
    public ValueChange OnCatIndexChange;
    public ValueChange OnSelectedCatChange;
}
