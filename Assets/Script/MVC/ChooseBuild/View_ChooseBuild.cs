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
    public GameObject chooseRoomItem;

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
    
    public override void Open()
    {
        UIView.InstantShow();
        bg.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutExpo);
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
            childObj.transform.DOKill();
            Destroy(childObj);
        }

        //生成
        for (int i = 0; i < selectedRooms.Count; i++)
        {
            GameObject buffer = Instantiate(chooseRoomItem, content);

            buffer.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutBack).SetDelay(i * 0.09375f);
            
            int index = i;
            buffer.GetComponent<Button>().onClick.RemoveListener(() => { App.controller.chooseBuild.Select(index); });
            buffer.GetComponent<Button>().onClick.AddListener(() => { App.controller.chooseBuild.Select(index); });
            buffer.GetComponent<ChooseRoomItem>().Active(selectedRooms[i]);
        }
    }

    public void OnRoomTypeChange(object value)
    {
        int index = (int) value;

        for (int i = 0; i < leftFronts.Length; i++)
        {
            if (i == index)
            {
                leftBacks[i].gameObject.SetActive(false);
                leftFronts[i].gameObject.SetActive(true);
                
                //Tween
                leftFronts[i].DOScale(Vector2.one, 0.25f).From(Vector2.zero);
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

        topSelectBlock.DOAnchorPos(topRects[index].anchoredPosition, 0.25f).SetEase(Ease.OutExpo);
        topSelectIcon.sprite = topIcons[index].sprite;
        topSelectText.text = topTexts[index].text;
    }

    public RectTransform GetChooseRoomItem(int index)
    {
        return content.GetChild(index).GetComponent<RectTransform>();
    }
}