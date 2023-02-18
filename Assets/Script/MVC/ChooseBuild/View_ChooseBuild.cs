using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_ChooseBuild : ViewBehaviour
{
    public Transform content;
    public ChooseRoomItem chooseRoomItem;

    [Title("Tween")] public Transform bg;
    
    //Top
    [Title("Top")] [SerializeField] private RectTransform topSelectBlock;
    [SerializeField] private Image topSelectIcon;
    [SerializeField] private TextMeshProUGUI topSelectText;
    [SerializeField] private RectTransform[] topRects;
    [SerializeField] private Image[] topIcons;
    [SerializeField] private TextMeshProUGUI[] topTexts;
    
    //Left
    [Title("Left")] [SerializeField] private RectTransform[] leftFronts;
    [SerializeField] private RectTransform[] leftBacks;

    [Title("Scroll")] [SerializeField] private Scrollbar scrollbar;
    
    public override void Open()
    {
        UIView.InstantShow();
        bg.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutExpo);
    }

    public override void Close()
    {
        base.Close();
        for (int i = 0; i < content.childCount; i++)
        {
            GameObject childObj = content.GetChild(i).gameObject;
            DOTween.Kill(childObj.transform, true);
            Destroy(childObj);
        }
    }

    public override void Init()
    {
        base.Init();

        App.model.chooseBuild.SelectedRoomsChange += OnSelectedRoomsValueChange;
        App.model.chooseBuild.OnRoomSortTypeChange += OnRoomSortTypeChange;
        App.model.chooseBuild.RoomTypeChange += OnRoomTypeChange;
    }

    public void OnSelectedRoomsValueChange(object tmp)
    {
        List<Room> selectedRooms = (List<Room>) tmp;

        //清除
        for (int i = 0; i < content.childCount; i++)
        {
            GameObject childObj = content.GetChild(i).gameObject;
            DOTween.Kill(childObj.transform, true);
            Destroy(childObj);
        }

        //生成
        for (int i = 0; i < selectedRooms.Count; i++)
        {
            var buffer = Instantiate(chooseRoomItem, content);
            if (i < 8)
                buffer.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBack).SetDelay(i * 0.09375f);
            buffer.SetData(selectedRooms[i]);
        }
    }

    public void OnRoomTypeChange(object value)
    {
        int index = (int) value;

        scrollbar.value = 1;
        
        for (int i = 0; i < leftFronts.Length; i++)
        {
            if (i == index)
            {
                leftBacks[i].gameObject.SetActive(false);
                leftFronts[i].gameObject.SetActive(true);
                
                //Tween
                leftFronts[i].DOScaleX(1, 0.25f).From(0).SetEase(Ease.OutBack);
            }
            else
            {
                leftBacks[i].gameObject.SetActive(true);
                leftFronts[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnRoomSortTypeChange(object value)
    {
        int index = (int) value;

        scrollbar.value = 1;

        topSelectBlock.DOAnchorPos(topRects[index].anchoredPosition, 0.25f).SetEase(Ease.OutExpo);
        topSelectIcon.sprite = topIcons[index].sprite;
        topSelectText.text = topTexts[index].text;
    }

    public RectTransform GetChooseRoomItem(int index)
    {
        return content.GetChild(index).GetComponent<RectTransform>();
    }
}