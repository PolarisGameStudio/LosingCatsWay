using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class CatchCatSystem : MvcBehaviour
{
    [SerializeField] private UIView view;

    [SerializeField] private Image catchCatBg;
    [SerializeField] private Sprite[] mapSprites;
    public CatchCatMap map;
    public CatchCatRunAway runAway;

    [ReadOnly] public bool IsWatchAd;
    [ReadOnly] public string Location;

    public void Active(int index, CloudCatData cloudCatData)
    {
        IsWatchAd = false;
        
        catchCatBg.sprite = mapSprites[index];
        Location = $"Location" + index;
        
        map.Open(cloudCatData);
        view.Show();
    }

    public void Close()
    {
        view.InstantHide();
    }
}
