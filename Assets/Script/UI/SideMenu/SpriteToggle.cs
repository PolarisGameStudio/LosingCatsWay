using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle), typeof(Image))]
public class SpriteToggle : MonoBehaviour
{
    public Sprite onSprite;
    public Sprite offSprite;

    public Toggle toggle;
    public Image image;

    void Start()
    {
        //toggle = GetComponent<Toggle>();
        //image = GetComponent<Image>();
    }

    public void ToggleSprite()
    {
        if (toggle == null || image == null)
        {
            Debug.LogError("Object doesn't have toggle or image component.");
            return;
        }

        if (toggle.isOn)
        {
            image.sprite = onSprite;
            return;
        }

        image.sprite = offSprite;
    }

    public void ToggleSpriteOn()
    {
        if (toggle == null || image == null)
        {
            Debug.LogError("Object doesn't have toggle or image component.");
            return;
        }

        toggle.isOn = true;
        image.sprite = onSprite;
    }

    public void ToggleSpriteOff()
    {
        if (toggle == null || image == null)
        {
            Debug.LogError("Object doesn't have toggle or image component.");
            return;
        }

        toggle.isOn = false;
        image.sprite = offSprite;
    }
}
