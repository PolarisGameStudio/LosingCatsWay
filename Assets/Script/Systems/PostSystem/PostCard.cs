using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PostCard : MonoBehaviour
{
    public TextMeshProUGUI titleText;

    public GameObject selected;
    public GameObject deselect;

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
    }

    public void Deselect()
    {
        selected.SetActive(false);
        deselect.SetActive(true);
    }
}
