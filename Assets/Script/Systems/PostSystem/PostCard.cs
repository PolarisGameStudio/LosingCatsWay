using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostCard : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Image titleIcon;

    [Title("Bg")]
    public GameObject selected;
    public GameObject deselect;

    [Title("Text")] 
    public Color32 selectedColor;
    public Color32 deselectColor;

    private PostSystem _postSystem;
    private int _index;
    
    public void SetData(int index, string title, PostSystem postSystem)
    {
        _index = index;
        _postSystem = postSystem;
        titleText.text = title;
    }

    public void Select()
    {
        selected.SetActive(true);
        deselect.SetActive(false);
        
        _postSystem.Select(_index);

        titleIcon.color = selectedColor;
        titleText.color = selectedColor;
    }

    public void Deselect()
    {
        selected.SetActive(false);
        deselect.SetActive(true);

        titleIcon.color = deselectColor;
        titleText.color = deselectColor;
    }
}
