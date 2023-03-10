using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Pedia : ModelBehavior
{
    #region Variable

    private int tabIndex = -1;
    
    //Pedia
    private int selectedPediaType = -1;
    private int pediaPageIndex = -1;
    private List<string> usingPediaIds;
    private string selectedPediaId;

    //Archive
    private int selectedArchiveType = -1;
    private List<Quest> archiveQuests;
    private int archivePageIndex = -1;

    //Cats
    private List<string> usingCatIds;
    private int catPageIndex = -1;
    private string selectedCatId;

    #endregion

    #region Properties

    public int TabIndex
    {
        get => tabIndex;
        set
        {
            tabIndex = value;
            OnTabIndexChange?.Invoke(value);
        }
    }

    //Pedia
    public int SelectedPediaType
    {
        get => selectedPediaType;
        set
        {
            selectedPediaType = value;
            OnSelectedPediaTypeChange?.Invoke(value);
        }
    }

    public int PediaPageIndex
    {
        get => pediaPageIndex;
        set
        {
            pediaPageIndex = value;
        }
    }

    public List<string> UsingPediaIds
    {
        get => usingPediaIds;
        set
        {
            usingPediaIds = value;
            OnUsingPediaIdsChange?.Invoke(value);
        }
    }

    public string SelectedPediaId
    {
        get => selectedPediaId;
        set
        {
            selectedPediaId = value;
            OnSelectPediaIdChange?.Invoke(value);
        }
    }

    //Archive
    public int SelectedArchiveType
    {
        get => selectedArchiveType;
        set
        {
            selectedArchiveType = value;
            OnSelectedArchiveTypeChange?.Invoke(value);
        }
    }

    public List<Quest> ArchiveQuests
    {
        get => archiveQuests;
        set
        {
            archiveQuests = value;
            OnArchiveQuestsChange?.Invoke(value);
        }
    }

    public int ArchivePageIndex
    {
        get => archivePageIndex;
        set
        {
            archivePageIndex = value;
        }
    }
    
    //Cat
    public List<string> UsingCatIds
    {
        get => usingCatIds;
        set
        {
            usingCatIds = value;
            OnUsingCasIdsChange?.Invoke(value);
        }
    }

    public int CatPageIndex
    {
        get => catPageIndex;
        set => catPageIndex = value;
    }

    public string SelectedCatId
    {
        get => selectedCatId;
        set
        {
            selectedCatId = value;
            OnSelectedCatIdChange?.Invoke(value);
        }
    }

    #endregion

    #region ValueChange

    public ValueChange OnTabIndexChange;
    
    //Pedia
    public ValueChange OnSelectedPediaTypeChange;
    public ValueChange OnUsingPediaIdsChange;
    public ValueChange OnSelectPediaIdChange;
    
    //Archive
    public ValueChange OnSelectedArchiveTypeChange;
    public ValueChange OnArchiveQuestsChange;
    
    //Cats
    public ValueChange OnUsingCasIdsChange;
    public ValueChange OnSelectedCatIdChange;

    #endregion
}
