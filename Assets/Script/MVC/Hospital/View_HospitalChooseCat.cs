using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class View_HospitalChooseCat : ViewBehaviour
{
    [SerializeField] private Card_HospitalChooseCat[] cards;
    [SerializeField] private GameObject[] titles;
    [SerializeField] private GameObject[] questTitles;

    [Title("Tween")]
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private Transform okButtonTransform;
    [SerializeField] private RectTransform questTransform;

    private bool isQuestShow;
    
    public override void Open()
    {
        base.Open();
        isQuestShow = false;
        questTransform.localScale = new Vector2(0, 1);
        panelTransform.DOScale(Vector2.one, 0.4f).From(Vector2.zero).SetEase(Ease.OutExpo);
        okButtonTransform.localScale = Vector2.zero;
    }
    
    public override void Close()
    {
        base.Close();
        for (int i = 0; i < cards.Length; i++)
            DOTween.Kill(cards[i].transform, true);
    }

    public override void Init()
    {
        base.Init();
        App.model.hospital.OnFunctionIndexChange += OnFunctionIndexChange;
        App.model.hospital.OnCatsChange += OnCatsChange;
        App.model.hospital.OnCatIndexChange += OnCatIndexChange;
    }

    private void OnCatIndexChange(object value)
    {
        int index = (int)value;

        if (index < 0)
            return;
        
        for (int i = 0; i < cards.Length; i++)
            cards[i].SetSelect(i == index);
        okButtonTransform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack);
    }

    private void OnCatsChange(object value)
    {
        List<Cat> cats = (List<Cat>)value;
        for (int i = 0; i < cards.Length; i++)
        {
            if (i >= cats.Count)
            {
                cards[i].gameObject.SetActive(false);
                continue;
            }

            cards[i].gameObject.SetActive(true);
            cards[i].SetData(cats[i].cloudCatData);
            cards[i].SetSelect(false);
            var tmp = cards[i].transform;
            tmp.DOScale(Vector2.one, 0.15f).From(Vector3.zero)
                .SetDelay(i * 0.1f);
        }
    }

    private void OnFunctionIndexChange(object value)
    {
        int index = (int)value;
        for (int i = 0; i < cards.Length; i++)
            cards[i].functionIndex = index;
        for (int i = 0; i < titles.Length; i++)
            titles[i].SetActive(i == index);
        for (int i = 0; i < questTitles.Length; i++)
            questTitles[i].SetActive(i == index);
    }

    public void ToggleChooseCatQuest()
    {
        isQuestShow = !isQuestShow;
        if (isQuestShow)
            questTransform.DOScaleX(1, 0.25f).SetEase(Ease.OutBack);
        else
            questTransform.DOScaleX(0, 0.25f).SetEase(Ease.InQuint);
    }
}
