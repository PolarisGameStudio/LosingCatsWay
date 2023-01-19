using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseRoomItem : MvcBehaviour
{
    public Image roomImage;

    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI roomCountText;

    public Image gameTagImage;

    [Title("Mask")]
    [SerializeField] private GameObject mask;
    [SerializeField] private GameObject countMask;

    public void SetData(Room room)
    {
        roomImage.sprite = room.Image;
        roomNameText.text = room.Name;

        int count = room.Count;
        gameObject.GetComponent<Button>().enabled = count > 0;

        roomCountText.text = room.Count.ToString();

        if (room.roomData.roomType == RoomType.Game && room.roomData.roomGamesType != RoomGameType.None) gameTagImage.gameObject.SetActive(true);
        
        mask.SetActive(room.Count <= 0);
        countMask.SetActive(room.Count <= 0);
    }

    public void Select()
    {
        int index = transform.GetSiblingIndex();
        App.controller.chooseBuild.Select(index);
    }
}