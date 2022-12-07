using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BigGamesTutorial : MvcBehaviour
{
    [Title("Text")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptText;
    [SerializeField] private TextMeshProUGUI numberText;

    [Title("UI")] [SerializeField] private Button startButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private UIView uiView;
    [SerializeField] private Image tutorialImage;
    
    private int index;
    private string[] descriptStrings;
    private Sprite[] tutorialSprites;
    private bool isTutorial;

    private Callback OnOpen;
    private Callback OnClose;

    public void SetData(string title, string[] descripts, Sprite[] sprites, Callback onOpen = null, Callback onClose = null)
    {
        OnOpen = onOpen;
        OnClose = onClose;

        nameText.text = title;
        descriptStrings = descripts;
        tutorialSprites = sprites;
    }

    public void OpenTutorial()
    {
        closeButton.gameObject.SetActive(false);
        isTutorial = true;
        OnOpen?.Invoke();

        index = -1;
        ToRight();
    }

    public void OpenAbout()
    {
        closeButton.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        isTutorial = false;
        OnOpen?.Invoke();

        index = -1;
        ToRight();
    }

    public void Close()
    {
        uiView.InstantHide();
        OnClose?.Invoke();
    }

    public void ToLeft()
    {
        index = Mathf.Clamp(index - 1, 0, descriptStrings.Length);
        tutorialImage.sprite = tutorialSprites[index];
        descriptText.text = descriptStrings[index];
        CheckButton();
    }

    public void ToRight()
    {
        index = Mathf.Clamp(index + 1, 0, descriptStrings.Length);
        tutorialImage.sprite = tutorialSprites[index];
        descriptText.text = descriptStrings[index];
        CheckButton();
    }

    private void CheckButton()
    {
        if (!isTutorial) return;
        startButton.gameObject.SetActive(index == descriptStrings.Length);
        leftButton.gameObject.SetActive(index > 0);
        rightButton.gameObject.SetActive(index < descriptStrings.Length);
    } 
}
