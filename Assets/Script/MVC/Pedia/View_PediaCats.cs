using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

public class View_PediaCats : ViewBehaviour
{
    [SerializeField] private UIView chooseCatView;
    [SerializeField] private UIView readCatView;
    [SerializeField] private Card_ChooseCat[] cards;
    
    public override void Init()
    {
        base.Init();
        App.model.pedia.OnUsingCasIdsChange += OnUsingCasIdsChange;
    }
    
    public override void Open()
    {
        base.Open();
        CloseReadCat();
        OpenChooseCat();
    }

    public override void Close()
    {
        CloseChooseCat();
        CloseReadCat();
        base.Close();
    }

    private void OpenChooseCat()
    {
        chooseCatView.Show();
    }

    private void OpenReadCat()
    {
        readCatView.Show();
    }

    private void CloseChooseCat()
    {
        chooseCatView.InstantHide();
    }

    private void CloseReadCat()
    {
        readCatView.InstantHide();
    }

    private void OnUsingCasIdsChange(object value)
    {
        List<string> usingCasIds = (List<string>)value;

        for (int i = 0; i < cards.Length; i++)
        {
            if (i >= usingCasIds.Count)
            {
                cards[i].gameObject.SetActive(false);
                continue;
            }
            
            cards[i].gameObject.SetActive(true);
            cards[i].SetData(usingCasIds[i]);
        }
    }
}
