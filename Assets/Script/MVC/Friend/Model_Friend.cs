using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Friend : ModelBehavior
{
    public List<string> myInvites;
    private List<FriendData> friends;
    private List<FriendData> invites;
    
    private int selectedContainer;
    private int selectedFriendIndex;
    private int selectedMyFavoriteCatIndex;

    private List<Cat> myCats;
    private Cat nowFavouriteCat;

    public int SelectedContainer
    {
        get => selectedContainer;
        set
        {
            selectedContainer = value;
            OnSelectedContainerChange(value);
        }
    }

    public List<FriendData> Friends
    {
        get => friends;
        set
        {
            friends = value;
            OnFriendsChange?.Invoke(value);
        }
    }
    
    public List<FriendData> Invites
    {
        get => invites;
        set
        {
            invites = value;
            OnInvitesChange?.Invoke(value);
        }
    }

    public int SelectedFriendIndex
    {
        get => selectedFriendIndex;
        set
        {
            selectedFriendIndex = value;
            OnSelectedFriendIndexChange?.Invoke(value);
        }
    }

    public int SelectedMyFavoriteCatIndex
    {
        get => selectedMyFavoriteCatIndex;
        set
        {
            selectedMyFavoriteCatIndex = value;
            OnSelectedMyFavoriteCatIndexChange?.Invoke(value);
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

    public Cat NowFavouriteCat
    {
        get => nowFavouriteCat;
        set
        {
            nowFavouriteCat = value;
            OnNowFavouriteCatChange(value);
        }
    }

    public ValueChange OnSelectedContainerChange;
    public ValueChange OnFriendsChange;
    public ValueChange OnInvitesChange;
    public ValueChange OnSelectedFriendIndexChange;
    public ValueChange OnSelectedMyFavoriteCatIndexChange;
    public ValueChange OnMyCatsChange;
    public ValueChange OnNowFavouriteCatChange;
}
