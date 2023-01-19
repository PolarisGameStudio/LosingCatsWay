using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Sirenix.OdinInspector;
using Spine.Unity;
using TMPro;
using UnityEngine;

public class Card_PediaCat : MvcBehaviour
{
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject lockObject;

    public void SetData(string id)
    {
        string title = App.factory.stringFactory.GetCatVariety(id);
        int level = App.system.inventory.KnowledgeCardDatas[id];
        bool unlock = level > 0;

        titleText.text = title;
        lockObject.SetActive(!unlock);
        catSkin.SetActive(unlock);

        if (unlock)
        {
            CloudCatData cloudCatData = new CloudCatData();
            cloudCatData.CatData = new CloudSave_CatData();
            cloudCatData.CatData.Variety = id;
            cloudCatData.CatData.BodyScale = 1;
            cloudCatData.CatData.BornTime = Timestamp.FromDateTime(DateTime.MinValue);

            cloudCatData.CatSkinData = new CloudSave_CatSkinData();
            cloudCatData.CatHealthData = new CloudSave_CatHealthData();
            
            catSkin.ChangeSkin(cloudCatData);
            
            catSkin.GetComponentInChildren<SkeletonGraphic>().timeScale =
                level >= 3 ? 1 : 0;
        }
    }

    public void Select()
    {
        int index = transform.GetSiblingIndex();
        App.controller.pedia.ChooseCat(index);
    }
}