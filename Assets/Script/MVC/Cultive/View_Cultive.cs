using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class View_Cultive : ViewBehaviour
{
    #region Variables

    public View_CultiveInfo cultiveInfo;

    [Title("Closet")]
    [SerializeField] private GameObject[] buttonMasks; //TypeButton前面的圖
    [SerializeField] private Transform layerContent;
    [SerializeField] private GameObject layerPrefab;
    [SerializeField] private Transform itemContent;
    [SerializeField] private Card_CultiveItem cultiveItem;

    [Title("DragDrop")]
    public Canvas canvas;

    [Title("Litter")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private Button cleanLitterButton;
    [SerializeField] private TextMeshProUGUI cleanCountText;
    [SerializeField] private Image litterImage;
    [SerializeField] private Sprite emptyLitterSprite;
    [SerializeField] private Sprite fullLitterSprite;
    public GameObject noLitterObject;
    public GameObject catDialogObject;

    [Title("Cat")]
    public CatSkin catSkin;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject[] genderImages;

    [Title("Status")]
    [SerializeField] private Image satietyFill;
    [SerializeField] private Image moistureFill;
    [SerializeField] private Image funFill;
    [SerializeField] private Image satietyFillInner;
    [SerializeField] private Image moistureFillInner;
    [SerializeField] private Image funFillInner;
    [SerializeField] private TextMeshProUGUI satietyText;
    [SerializeField] private TextMeshProUGUI moistureText;
    [SerializeField] private TextMeshProUGUI funText;
    [SerializeField] private Sprite lowValueSprite;
    [SerializeField] private Sprite highValueSprite;

    [Title("DoTween")]
    [SerializeField] private RectTransform rightRect;
    [SerializeField] private RectTransform topRect;
    [SerializeField] private RectTransform leftBotStart; //開始位置
    [SerializeField] private RectTransform[] leftBotButtons;

    [Title("UI")] [SerializeField] private UIButton openChooseSkinButton;
    [SerializeField] private GameObject openChooseSkinMask;

    [Title("Debug")]
    [SerializeField] private TextMeshProUGUI realSatietyText;
    [SerializeField] private TextMeshProUGUI realMoistureText;
    [SerializeField] private TextMeshProUGUI realFunText;

    private Vector2 topOrigin;
    private Vector2 topOffset;
    private Vector2 rightOrigin;
    private Vector2 rightOffset;
    private List<Vector2> leftBotOrigins;

    private Sequence openSequence;
    private Sequence closeSequence;

    private int _count;

    #endregion

    #region Override

    public override void Open()
    {
        base.Open();
        catSkin.SetActive(true);

        catDialogObject.transform.localScale = Vector2.zero;

        TweenConfig();
        TweenIn();
    }

    public override void Close()
    {
        base.Close();
        catSkin.SetActive(false);
    }

    public override void Init()
    {
        base.Init();
        canvas = GetComponent<Canvas>();

        App.model.cultive.OnSelectedCatChange += OnSelectedCatChange;
        App.model.cultive.OnSelectedTypeChange += OnSelectedTypeChange;
        App.model.cultive.OnSelectedItemsChange += OnSelectedItemsChange;
        App.model.cultive.OnNextCleanDateTimeChange += OnNextCleanDateTimeChange;
        App.model.cultive.OnCleanLitterCountChange += OnCleanLitterCountChange;
    }

    #endregion

    #region ValueChange

    private void OnSelectedCatChange(object value)
    {
        Cat cat = (Cat)value;
        if (cat == null) return;

        catSkin.ChangeSkin(cat.cloudCatData);
        nameText.text = cat.cloudCatData.CatData.CatName;
        
        for (int i = 0; i < genderImages.Length; i++)
        {
            if (i == cat.cloudCatData.CatData.Sex)
                genderImages[i].SetActive(true);
            else
                genderImages[i].SetActive(false);
        }

        RefreshStatus(cat.cloudCatData);
    }

    private void OnSelectedTypeChange(object value)
    {
        int index = Convert.ToInt32(value);
        index--;

        #region TypeButton白圖切換

        for (int i = 0; i < buttonMasks.Length; i++)
        {
            if (i == index)
                buttonMasks[i].SetActive(true);
            else
                buttonMasks[i].SetActive(false);
        }

        #endregion
    }

    private void OnSelectedItemsChange(object value)
    {
        List<Item> items = (List<Item>)value;

        #region 生架子層數

        for (int i = 0; i < layerContent.childCount; i++)
        {
            Destroy(layerContent.GetChild(i).gameObject);
        }

        float f = items.Count / 3f;
        int layerCount = (int)Mathf.Ceil(f);

        for (int i = 0; i < layerCount; i++)
        {
            Instantiate(layerPrefab, layerContent);
        }

        #endregion

        #region 生Items

        for (int i = 1; i < itemContent.childCount; i++)
        {
            GameObject tmp = itemContent.GetChild(i).gameObject;
            tmp.transform.DOKill();
            Destroy(tmp);
        }

        for (int i = 0; i < items.Count; i++)
        {
            var card = Instantiate(cultiveItem, itemContent);

            card.SetData(items[i]);
            card.dragSensor.canvas = canvas;

            card.transform.DOScale(0, 0);
            card.transform.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetDelay(i * 0.09375f);
        }

        #endregion
    }

    private void OnNextCleanDateTimeChange(object value)
    {
        DateTime nextDate = (DateTime)value;
        TimeSpan ts = nextDate - App.system.myTime.MyTimeNow;
        
        //時候未到
        if (nextDate > App.system.myTime.MyTimeNow)
        {
            cleanLitterButton.gameObject.SetActive(false);
            noLitterObject.SetActive(false);
            timerObject.SetActive(true);
            
            string str = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
            timerText.text = str;
            litterImage.sprite = fullLitterSprite;
        }
        else
        {
            timerObject.SetActive(false);
            
            if (_count > 0)
            {
                cleanLitterButton.gameObject.SetActive(true);
                noLitterObject.SetActive(false);
                litterImage.sprite = fullLitterSprite;
            }
            else
            {
                cleanLitterButton.gameObject.SetActive(false);
                noLitterObject.SetActive(true);
                litterImage.sprite = emptyLitterSprite;
            }
        }
            
    }

    private void OnCleanLitterCountChange(object value)
    {
        int index = Convert.ToInt32(value);
        _count = index;
        cleanCountText.text = index.ToString();
    }

    #endregion

    #region DoTween

    private void TweenConfig()
    {
        //Top Config
        topOrigin = topRect.anchoredPosition;
        topOffset = new Vector2(topOrigin.x, topOrigin.y * 2 + topRect.sizeDelta.y);

        //Right Config
        rightOrigin = rightRect.anchoredPosition;
        rightOffset = new Vector2(rightRect.sizeDelta.x + 60f, rightOrigin.y);

        //LeftBot Config
        leftBotOrigins = new List<Vector2>();
        for (int i = 0; i < leftBotButtons.Length; i++)
        {
            leftBotOrigins.Add(leftBotButtons[i].anchoredPosition);
            leftBotButtons[i].anchoredPosition = leftBotStart.anchoredPosition;
        }
    }

    [Button]
    public void TweenIn()
    {
        if (openSequence != null) openSequence.Kill();
        openSequence = DOTween.Sequence();

        openSequence
            .AppendInterval(0.3f)
            .Join(topRect.DOAnchorPos(topOrigin, 0.25f).From(topOffset).SetEase(Ease.OutBack).SetDelay(0.0625f))
            .Join(rightRect.DOAnchorPos(rightOrigin, 0.25f).From(rightOffset).SetEase(Ease.OutBack).SetDelay(0.0625f))
            .OnStart(() =>
            {
                leftBotStart.DOScale(Vector2.zero, 0);
            })
            .OnComplete(() =>
            {
                leftBotStart.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack).SetDelay(0.0625f);

                for (int i = 0; i < leftBotButtons.Length; i++)
                {
                    leftBotButtons[i]
                        .DOAnchorPos(leftBotOrigins[i], 0.25f)
                        .From(leftBotStart.anchoredPosition)
                        .SetEase(Ease.OutBack)
                        .SetDelay(0.25f + i * 0.0625f);
                }
            });

        if (noLitterObject.activeSelf)
            noLitterObject.transform.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.25f);
        if (cleanLitterButton.gameObject.activeSelf)
            cleanLitterButton.transform.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.25f);
        if (timerObject.activeSelf)
            timerObject.transform.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.25f);
    }

    [Button]
    public void TweenOut()
    {
        if (closeSequence != null) closeSequence.Kill();
        closeSequence = DOTween.Sequence();

        closeSequence
            .OnStart(() =>
            {
                for (int i = 0; i < leftBotButtons.Length; i++)
                {
                    leftBotButtons[i].DOAnchorPos(leftBotStart.anchoredPosition, 0.25f)
                        .SetEase(Ease.InBack)
                        .SetDelay(i * 0.0625f);
                }

                leftBotStart.DOScale(Vector2.zero, 0.25f).SetEase(Ease.InBack).SetDelay((leftBotButtons.Length + 1) * 0.0625f);
            })
            .AppendInterval(0.25f + (leftBotButtons.Length + 1) * 0.0625f)
            .Append(rightRect.DOAnchorPos(rightOffset, 0.25f).From(rightOrigin).SetEase(Ease.InBack).SetDelay(0.0625f))
            .Join(topRect.DOAnchorPos(topOffset, 0.25f).From(topOrigin).SetEase(Ease.InBack).SetDelay(0.0625f));
    }

    #endregion

    public void RefreshStatus(CloudCatData cloudCatData)
    {
        satietyFillInner.sprite = cloudCatData.CatSurviveData.Satiety > 20 ? highValueSprite : lowValueSprite;
        moistureFillInner.sprite = cloudCatData.CatSurviveData.Moisture > 20 ? highValueSprite : lowValueSprite;
        funFillInner.sprite = cloudCatData.CatSurviveData.Favourbility > 20 ? highValueSprite : lowValueSprite;

        satietyFill.DOFillAmount(cloudCatData.CatSurviveData.Satiety / 100, 0.25f).SetEase(Ease.OutExpo);
        moistureFill.DOFillAmount(cloudCatData.CatSurviveData.Moisture / 100, 0.25f).SetEase(Ease.OutExpo);
        funFill.DOFillAmount(cloudCatData.CatSurviveData.Favourbility / 100, 0.25f).SetEase(Ease.OutExpo);

        satietyText.text = $"{cloudCatData.CatSurviveData.Satiety:0} / 100";
        moistureText.text = $"{cloudCatData.CatSurviveData.Moisture:0} / 100";
        funText.text = $"{cloudCatData.CatSurviveData.Favourbility:0} / 100";

        realSatietyText.text = cloudCatData.CatSurviveData.RealSatiety.ToString("0.00");
        realMoistureText.text = cloudCatData.CatSurviveData.RealMoisture.ToString("0.00");
        realFunText.text = cloudCatData.CatSurviveData.RealFavourbility.ToString("0.00");

        openChooseSkinButton.interactable = false;
        openChooseSkinMask.SetActive(true);
        if (CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays) == 0)
            return;
        if (!string.IsNullOrEmpty(cloudCatData.CatHealthData.SickId) || cloudCatData.CatHealthData.IsBug)
            return;
        openChooseSkinButton.interactable = true;
        openChooseSkinMask.SetActive(false);
    }
}
