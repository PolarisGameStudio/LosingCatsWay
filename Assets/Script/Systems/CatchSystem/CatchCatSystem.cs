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
    [SerializeField] private CatchCatG8End G8End;

    [ReadOnly] public bool IsWatchAd = false;
    private int g8Index;

    public void Active(int index, CloudCatData cloudCatData, bool isTutorial = false)
    {
        IsWatchAd = false;
        
        catchCatBg.sprite = mapSprites[index];
        map.IsTutorial = isTutorial;
        g8Index = index;
        
        map.Open(cloudCatData);
        view.Show();
    }

    public void Close()
    {
        view.InstantHide();
    }

    public void ActiveG8End(CloudCatData cloudCatData, Callback callback)
    {
        G8End.Active(g8Index, cloudCatData, callback);
    }
}
