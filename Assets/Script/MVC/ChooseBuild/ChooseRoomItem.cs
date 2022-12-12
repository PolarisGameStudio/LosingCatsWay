using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseRoomItem : MvcBehaviour
{
    public Image roomImage;

    public TextMeshProUGUI roomNameText;
    public TextMeshProUGUI roomCountText;

    public Image gameTagImage;

    public void SetData(Room room)
    {
        roomImage.sprite = room.Image;
        roomNameText.text = room.Name;

        int count = room.Count;
        gameObject.GetComponent<Button>().enabled = count > 0;

        roomCountText.text = room.Count.ToString();

        if (room.roomData.roomType == RoomType.Game && room.roomData.roomGamesType != RoomGameType.None) gameTagImage.gameObject.SetActive(true);
    }

    public void Select()
    {
        int index = transform.GetSiblingIndex();
        App.controller.chooseBuild.Select(index);
    }
}