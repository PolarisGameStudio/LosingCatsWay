using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mall_CatSkin_FAA00002 : Mall_CatSkin
{
    public override void ChangeSkin()
    {
        base.ChangeSkin();
        CloudCatData cloudCatData = new CloudCatData();
        cloudCatData.CatSkinData = new CloudSave_CatSkinData();
        cloudCatData.CatSkinData.UseSkinId = "Magic_Hat";
        catSkin.SetSkin(cloudCatData);
    }
}
