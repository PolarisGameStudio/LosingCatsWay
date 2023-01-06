using System.Collections.Generic;
using UnityEngine;

public class View_Cloister : ViewBehaviour
{
    [SerializeField] private CatFlower catFlower;
    [SerializeField] private Card_Cloister cardCloister;
    [SerializeField] private Transform cardContent;
    [SerializeField] private GameObject noDataObject;
    [SerializeField] private GameObject zeroPointFiveCard;
    [SerializeField] private GameObject zeroPointFiveSelectObject;
    [SerializeField] private GameObject useFlowerButton;
    [SerializeField] private GameObject useFlowerMask;

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
    
    public override void Init()
    {
        base.Init();
        App.model.cloister.OnSelectedLosingCatChange += OnSelectedLosingCatChange;
        App.model.cloister.OnLosingCatDatasChange += OnLosingCatDatasChange;
        App.model.cloister.OnSelectedIndexChange += OnSelectedIndexChange;
        
        App.system.player.OnCatDeadCountChange += OnCatDeadCountChange;
    }

    private void OnCatDeadCountChange(object value)
    {
        int index = (int)value;
        zeroPointFiveCard.SetActive(index > 0);
    }

    private void OnSelectedIndexChange(object value)
    {
        int index = (int)value;
        
        zeroPointFiveSelectObject.SetActive(false);
        List<Card_Cloister> cards = new List<Card_Cloister>();
        for (int i = 0; i < cardContent.childCount; i++)
        {
            var tmp = cardContent.GetChild(i).GetComponent<Card_Cloister>();
            if (tmp == null)
                continue;
            tmp.SetSelect(false);
            cards.Add(tmp);
        }

        if (index < 0)
            return;
        
        if (index == 0)
        {
            zeroPointFiveSelectObject.SetActive(true);
            
            useFlowerButton.SetActive(false);
            useFlowerMask.SetActive(false);
            
            return;
        }

        useFlowerButton.SetActive(true);
        cards[index - 1].SetSelect(true);
    }

    private void OnSelectedLosingCatChange(object value)
    {
        var data = (CloudLosingCatData)value;

        catFlower.ChangeSkin(data);
        catFlower.DoAnimation(data.CatDiaryData.UsedFlower);
        catFlower.gameObject.SetActive(true);

        useFlowerMask.SetActive(data.CatDiaryData.UsedFlower);
    }
    
    private void OnLosingCatDatasChange(object value)
    {
        List<CloudLosingCatData> datas = (List<CloudLosingCatData>)value;

        for (int i = 1; i < cardContent.childCount; i++)
            Destroy(cardContent.GetChild(i).gameObject);

        noDataObject.SetActive(datas.Count <= 0 && App.system.player.CatDeadCount <= 0);
        
        for (int i = 0; i < datas.Count; i++)
        {
            var card = Instantiate(cardCloister, cardContent);
            card.SetData(datas[i]);
        }
    }
}
