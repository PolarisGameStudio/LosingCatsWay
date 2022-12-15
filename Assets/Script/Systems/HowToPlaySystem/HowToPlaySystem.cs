using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlaySystem : MvcBehaviour
{
    [Title("Text")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptText;
    [SerializeField] private TextMeshProUGUI numberText;

    [Title("UI")] [SerializeField] private UIButton startButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private UIView uiView;
    [SerializeField] private Image tutorialImage;
    [SerializeField] private Transform[] circles;
    
    private int index;
    private string[] descriptStrings;
    private Sprite[] tutorialSprites;
    private bool IsTutorial;

    private Callback OnOpen;
    private Callback OnClose;

    public HowToPlaySystem SetData(string title, string[] descripts, Sprite[] sprites)
    {
        nameText.text = title;
        descriptStrings = descripts;
        tutorialSprites = sprites;

        for (int i = 0; i < circles.Length; i++)
        {
            if (i >= sprites.Length)
                circles[i].gameObject.SetActive(false);
            else
                circles[i].gameObject.SetActive(true);
        }

        return this;
    }

    public void Open(bool isTutorial, Callback onOpen = null, Callback onClose = null)
    {
        IsTutorial = isTutorial;
        OnOpen = onOpen;
        OnClose = onClose;
        
        closeButton.gameObject.SetActive(!IsTutorial);
        startButton.gameObject.SetActive(IsTutorial);
        uiView.Show();
        OnOpen?.Invoke();
        index = -1;
        ToRight();
    }

    public void Close()
    {
        uiView.InstantHide();
        OnClose?.Invoke();
        OnClose = null;
        App.system.soundEffect.Play("Button");
    }

    public void ToLeft()
    {
        if (index <= 0)
            return;
        
        App.system.soundEffect.Play("Button");
        
        index = Mathf.Clamp(index - 1, 0, descriptStrings.Length - 1);
        CheckContent();
        CheckButton();
    }

    public void ToRight()
    {
        if (index >= descriptStrings.Length - 1)
            return;
        
        App.system.soundEffect.Play("Button");

        index = Mathf.Clamp(index + 1, 0, descriptStrings.Length - 1);
        CheckContent();
        CheckButton();
    }

    private void CheckContent()
    {
        numberText.text = (index + 1).ToString();
        tutorialImage.sprite = tutorialSprites[index];
        descriptText.text = descriptStrings[index];
    }

    private void CheckButton()
    {
        for (int i = 0; i < circles.Length; i++)
        {
            if (i == index)
                circles[i].GetChild(0).gameObject.SetActive(true);
            else
                circles[i].GetChild(0).gameObject.SetActive(false);
        }
        
        // leftButton.gameObject.SetActive(index > 0);
        // rightButton.gameObject.SetActive(index < descriptStrings.Length - 1);
        if (!IsTutorial) return;
        startButton.gameObject.SetActive(index == descriptStrings.Length - 1);
    }
}
