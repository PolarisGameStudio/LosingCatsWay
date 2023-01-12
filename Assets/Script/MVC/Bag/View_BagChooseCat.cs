using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

public class View_BagChooseCat : MvcBehaviour
{
    [SerializeField] private UIView view;
    [SerializeField] private Card_BagChooseCat card;
    [SerializeField] private Transform content;

    private List<Card_BagChooseCat> _cards;
    private List<string> _catIds;
    private int _selectIndex;

    public void Open(BagChooseCatType bagChooseCatType)
    {
        RefreshUI(bagChooseCatType);
        view.Show();

        if (_cards.Count == 0)
            return;
        
        _selectIndex = -1;
        Select(0);
    }

    public void Close()
    {
        view.InstantHide();
    }

    public void Select(int index)
    {
        if (_selectIndex != -1)
            _cards[_selectIndex].SetSelect(false);

        _cards[index].SetSelect(true);
        _selectIndex = index;
    }

    public void Ok()
    {
        App.controller.bag.ChooseCatOk(_catIds[_selectIndex]);
        Close();
    }

    private void RefreshUI(BagChooseCatType bagChooseCatType)
    {
        _cards = new List<Card_BagChooseCat>();
        _catIds = new List<string>();

        for (int i = 0; i < content.childCount; i++)
            Destroy(content.GetChild(i).gameObject);

        if (bagChooseCatType == BagChooseCatType.Cat)
        {
            List<Cat> cats = App.system.cat.GetCats();

            for (int i = 0; i < cats.Count; i++)
            {
                Card_BagChooseCat cardBagChooseCat = Instantiate(card, content);
                cardBagChooseCat.SetData(cats[i].cloudCatData);

                var index = i;
                cardBagChooseCat.button.onClick.AddListener(() =>
                {
                    Select(index);
                });
                
                _cards.Add(cardBagChooseCat);
                _catIds.Add(cats[i].cloudCatData.CatData.CatId);
            }
        }

        if (bagChooseCatType == BagChooseCatType.LosingCat)
        {
            List<CloudLosingCatData> cats = App.model.cloister.LosingCatDatas;

            for (int i = 0; i < cats.Count; i++)
            {
                Card_BagChooseCat cardBagChooseCat = Instantiate(card, content);
                cardBagChooseCat.SetData(cats[i]);

                var index = i;
                cardBagChooseCat.button.onClick.AddListener(() =>
                {
                    Select(index);
                });
                
                _cards.Add(cardBagChooseCat);
                _catIds.Add(cats[i].CatData.CatId);
            }
        }
    }
}

public enum BagChooseCatType
{
    Cat,
    LosingCat
}