using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlaySystem : MvcBehaviour
{
    [Title("Text")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptText;

    [Title("UI")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private UIView uiView;
    [SerializeField] private Image tutorialImage;
    [SerializeField] private Transform[] circles;

    [Title("Tween")]
    [SerializeField] private RectTransform iconRect;
    [SerializeField] private RectTransform nameRect;
    [SerializeField] private RectTransform centerRect;
    [SerializeField] private RectTransform leftRect;
    [SerializeField] private RectTransform rightRect;
    [SerializeField] private RectTransform startRect;
    [SerializeField] private CanvasGroup leftCanvasGroup;
    [SerializeField] private CanvasGroup rightCanvasGroup;
    [SerializeField] private CanvasGroup startCanvasGroup;
    [SerializeField] private RectTransform bottomRect;
    [SerializeField] private RectTransform skipRect;

    [Title("Tween/Origin")]
    [SerializeField] private Vector2 iconOrigin;
    [SerializeField] private Vector2 leftOrigin;
    [SerializeField] private Vector2 rightOrigin;
    [SerializeField] private Vector2 bottomOrigin;
    
    [Title("Tween/Offset")]
    [SerializeField] private Vector2 iconOffset;
    [SerializeField] private Vector2 leftOffset;
    [SerializeField] private Vector2 rightOffset;
    [SerializeField] private Vector2 bottomOffset;

    private Sequence startSequence;
    
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
        
        InitTween();
        PlayStartTween();
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
        
        PlayLeftRightTween();
        App.system.soundEffect.Play("Button");
        
        index = Mathf.Clamp(index - 1, 0, descriptStrings.Length - 1);
        CheckContent();
        CheckButton();
    }

    public void ToRight()
    {
        if (index >= descriptStrings.Length - 1)
            return;
        
        PlayLeftRightTween();
        App.system.soundEffect.Play("Button");

        index = Mathf.Clamp(index + 1, 0, descriptStrings.Length - 1);
        CheckContent();
        CheckButton();
    }

    private void CheckContent()
    {
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
        
        leftButton.gameObject.SetActive(index > 0);
        rightButton.gameObject.SetActive(index < descriptStrings.Length - 1);
        if (!IsTutorial)
            return;
        startButton.gameObject.SetActive(index == descriptStrings.Length - 1);
    }

    private void InitTween()
    {
        iconRect.anchoredPosition = iconOffset;
        nameRect.localScale = Vector2.zero;
        centerRect.localScale = Vector2.zero;
        bottomRect.anchoredPosition = bottomOffset;
        skipRect.localScale = Vector2.zero;
        leftRect.anchoredPosition = leftOffset;
        rightRect.anchoredPosition = rightOffset;
        startRect.anchoredPosition = rightOffset;
        leftCanvasGroup.alpha = 0;
        rightCanvasGroup.alpha = 0;
        startCanvasGroup.alpha = 0;
    }

    private void PlayStartTween()
    {
        startSequence?.Kill();
        startSequence = DOTween.Sequence();

        startSequence
            .AppendInterval(0.2f)
            .AppendCallback(ToRight)
            .Append(centerRect.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutExpo))
            .Join(iconRect.DOAnchorPos(iconOrigin, 0.4f).SetEase(Ease.OutExpo))
            .Join(bottomRect.DOAnchorPos(bottomOrigin, 0.25f).SetEase(Ease.OutBack))
            .PrependInterval(0.1f)
            .Append(nameRect.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutExpo))
            .Join(skipRect.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack));
    }

    private void PlayLeftRightTween()
    {
        leftRect.DOKill(true);
        rightRect.DOKill(true);
        startRect.DOKill(true);
        leftCanvasGroup.DOKill(true);
        rightCanvasGroup.DOKill(true);
        startCanvasGroup.DOKill(true);
        
        leftRect.DOAnchorPos(leftOrigin, .35f).From(leftOffset).SetEase(Ease.OutBack);
        leftCanvasGroup.DOFade(1, .3f).From(0);
        
        rightRect.DOAnchorPos(rightOrigin, .35f).From(rightOffset).SetEase(Ease.OutBack);
        rightCanvasGroup.DOFade(1, .3f).From(0);
        
        startRect.DOAnchorPos(rightOrigin, .35f).From(rightOffset).SetEase(Ease.OutBack);
        startCanvasGroup.DOFade(1, .3f).From(0);
    }
}
