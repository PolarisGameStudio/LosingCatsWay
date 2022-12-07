using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_Cloister : ViewBehaviour
{
    [SerializeField] private CatFlower catFlower;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardContent;
    [SerializeField] private GameObject noDataObject;

    public override void Init()
    {
        base.Init();
        App.model.cloister.OnSelectedLosingCatChange += OnSelectedLosingCatChange;
        App.model.cloister.OnLosingCatDatasChange += OnLosingCatDatasChange;
    }

    public override void Open()
    {
        base.Open();
        catFlower.gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        catFlower.gameObject.SetActive(false);
    }

    private void OnSelectedLosingCatChange(object value)
    {
        var data = (CloudLosingCatData)value;
        catFlower.ChangeSkin(data);
        catFlower.DoAnimation(data.CatDiaryData.UsedFlower);
        catFlower.gameObject.SetActive(true);
    }
    
    private void OnLosingCatDatasChange(object value)
    {
        List<CloudLosingCatData> datas = (List<CloudLosingCatData>)value;

        for (int i = 0; i < cardContent.childCount; i++)
        {
            Destroy(cardContent.GetChild(i).gameObject);
        }

        noDataObject.SetActive(datas.Count <= 0);
        
        for (int i = 0; i < datas.Count; i++)
        {
            GameObject tmp = Instantiate(cardPrefab, cardContent);
            Card_Cloister card = tmp.GetComponent<Card_Cloister>();
            card.SetData(datas[i]);
        }
    }
}
