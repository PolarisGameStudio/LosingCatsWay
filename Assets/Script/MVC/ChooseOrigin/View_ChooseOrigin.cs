using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class View_ChooseOrigin : ViewBehaviour
{
    [Title("UI")]
    public RectTransform underBar;

    [Title("Selector")]
    public Transform selector;
    public Transform pointer;
    
    [Title("Card")] [SerializeField] private Card_ChooseOrigin _cardChooseOrigin;
    [SerializeField] private Transform content;

    public override void Init()
    {
        base.Init();
        App.model.chooseOrigin.OnUsingRoomIndexChange += OnUsingRoomIndexChange;
        App.model.chooseOrigin.OnPreviewRoomIndexChange += OnPreviewRoomIndexChange;
        App.model.chooseOrigin.OnCenterRoomsChange += OnCenterRoomsChange;
    }

    private void OnCenterRoomsChange(object value)
    {
        List<Room> rooms = (List<Room>)value;

        for (int i = 0; i < content.childCount; i++)
            Destroy(content.GetChild(i).gameObject);

        for (int i = 0; i < rooms.Count; i++)
        {
            var card = Instantiate(_cardChooseOrigin, content);
            card.SetData(rooms[i]);
        }
    }

    private void OnUsingRoomIndexChange(object value)
    {
        int index = (int)value;
        var targetPosition = content.GetChild(index).transform.position;
        selector.transform.DOMoveX(targetPosition.x, 0);
        pointer.transform.DOMoveX(targetPosition.x, 0);
    }

    private void OnPreviewRoomIndexChange(object from, object to)
    {
        int fromValue = (int)from;
        int toValue = (int)to;

        var toCard = content.GetChild(toValue).GetComponent<Card_ChooseOrigin>();
        var targetPosition = toCard.transform.position;

        selector.DOMoveX(targetPosition.x, 0.25f);
        pointer.DOMoveX(targetPosition.x, 0.25f);
        
        toCard.selectedCanvasGroup.DOFade(1, 0.25f).From(0);
        toCard.deselectCanvasGroup.DOFade(0, 0.25f).From(1);
        
        if (fromValue == -1)
            return;

        var fromCard = content.GetChild(fromValue).GetComponent<Card_ChooseOrigin>();
        fromCard.selectedCanvasGroup.DOFade(0, 0.25f).From(1);
        fromCard.deselectCanvasGroup.DOFade(1, 0.25f).From(0);
    }

    public override void Open()
    {
        UIView.InstantShow();
        underBar.DOAnchorPosY(0, .5f).SetEase(Ease.OutExpo).From(new Vector2(0, -450));
    }

    public override void Close()
    {
        underBar.DOAnchorPosY(-450, .5f).SetEase(Ease.OutExpo).From(new Vector2(0, 0)).OnComplete(() =>
        {
            UIView.InstantHide();
        });
    }
}
