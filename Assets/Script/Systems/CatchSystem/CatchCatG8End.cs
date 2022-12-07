using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;
using UnityEngine.UI;

public class CatchCatG8End : MvcBehaviour
{
    public Image bg;
    public Sprite[] bgSprites;
    public UIView uiView;
    public CatSkin catSkin;

    private Callback _callback;

    public void Active(int index, CloudCatData cloudCatData, Callback callback)
    {
        catSkin.ChangeSkin(cloudCatData);
        bg.sprite = bgSprites[index];
        _callback = callback;
        
        uiView.Show();
    }

    public void Close()
    {
        uiView.InstantHide();
        _callback?.Invoke();
    }
}
