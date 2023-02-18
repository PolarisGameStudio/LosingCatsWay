using DG.Tweening;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Coffee.UIExtensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class Cat_Entrance : MvcBehaviour
{
    [SerializeField] private bool isFront;
    [SerializeField] private CatSkin catSkin;
    [SerializeField] private RectTransform catRect;
    [SerializeField] private GameObject bubbleObject;
    [SerializeField] private Image statusImage; //想喝水想吃飼料想玩的氣泡
    [SerializeField] private RectTransform diaryButtonRect; //死亡之後的按鈕
    [SerializeField] private GameObject clickButton;
    [SerializeField] private bool hasBubble;

    [Title("Sprites")]
    [SerializeField, HideIf("@hasBubble == false")] private Sprite angrySprite;
    [SerializeField, HideIf("@hasBubble == false")] private Sprite waterSprite;
    [SerializeField, HideIf("@hasBubble == false")] private Sprite sadSprite;
    [SerializeField, HideIf("@hasBubble == false")] private Sprite heartSprite;
    [SerializeField, HideIf("@hasBubble == false")] private Sprite brokenHeartSprite;
    
    private Vector2 startScale;
    private SkeletonGraphic skeletonGraphic;
    private bool isKitty;

    public void SetCatData(CloudCatData cloudCatData)
    {
        catSkin.ChangeSkin(cloudCatData);
        if (cloudCatData.CatData.SurviveDays > 3)
            catSkin.SetStatusFace(cloudCatData);
        startScale = catRect.localScale;
        CatIdle();

        int ageLevel = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays);
        isKitty = ageLevel == 0;

        bubbleObject.SetActive(hasBubble);
        if (!hasBubble) return;
        
        //Status
        CloudSave_CatSurviveData catSurviveData = cloudCatData.CatSurviveData;
        if (catSurviveData.Satiety >= 70f && catSurviveData.Moisture >= 70f && catSurviveData.Favourbility >= 70f)
        {
            statusImage.sprite = heartSprite;
            return;
        }
        if (catSurviveData.Satiety < 30f && catSurviveData.Moisture < 30f && catSurviveData.Favourbility < 30f)
        {
            statusImage.sprite = brokenHeartSprite;
            return;
        }

        List<float> status = new List<float>();
        status.Add(catSurviveData.Moisture);
        status.Add(catSurviveData.Favourbility);
        status.Add(catSurviveData.Satiety);
        float result = status.Min();
        
        if (catSurviveData.Moisture.Equals(result))
        {
            statusImage.sprite = waterSprite;
            return;
        }
        if (catSurviveData.Favourbility.Equals(result))
        {
            statusImage.sprite = sadSprite;
            return;
        }
        statusImage.sprite = angrySprite;
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
        catSkin.SetActive(active);
    }

    public void Click()
    {
        if (!isFront)
            return;
        
        bubbleObject.transform.DOScale(Vector2.zero, 0.25f).SetEase(Ease.OutExpo);

        int index = transform.GetSiblingIndex();
        App.controller.entrance.pitchShift.Play(index);
        
        skeletonGraphic = catSkin.skeletonGraphic;
        skeletonGraphic.AnimationState.ClearTracks();
        skeletonGraphic.AnimationState.Complete -= EndClick; //連續點
        skeletonGraphic.AnimationState.SetAnimation(0, "AI_Main/Entrance_Jump", false);
        skeletonGraphic.AnimationState.Complete += EndClick;
    }

    private void EndClick(TrackEntry trackEntry)
    {
        trackEntry.Complete -= EndClick;
        CatIdle();
    }

    private void CatIdle()
    {
        bubbleObject.transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.OutBack);
        
        skeletonGraphic = catSkin.skeletonGraphic;
        skeletonGraphic.AnimationState.ClearTracks();
        // int random = isKitty ? 1 : Random.Range(1, 3);
        skeletonGraphic.AnimationState.SetAnimation(0, "AI_Main/Sit_01", true); // TODO KittyBug
    }

    public void StartDead()
    {
        bubbleObject.SetActive(false);
        clickButton.SetActive(false);
        
        //先抓動畫
        Spine.Animation anim = catSkin.skeletonGraphic.SkeletonData.FindAnimation("Situation_Cat/Die");
        //播放動畫
        catSkin.skeletonGraphic.AnimationState.SetAnimation(0, anim, false);
        DOVirtual.DelayedCall(4.3f, ShowDiaryButton);
        DOVirtual.DelayedCall(10f, App.controller.entrance.CloseDeadEffect);
    }

    //啓動日記
    private void ShowDiaryButton()
    {
        // catRect.DOScale(Vector2.zero, 0.45f).From(startScale).SetEase(Ease.InBack);
        catRect.gameObject.SetActive(false);
        diaryButtonRect.DOScale(Vector2.one, 0.25f).From(Vector2.zero).SetEase(Ease.OutBack).SetDelay(0.45f);
    }

    /// 綁定在貓死亡後出現的日記按鈕上
    public void GetDiary()
    {
        diaryButtonRect.DOScale(Vector2.zero, 0.25f).From(Vector2.one).SetEase(Ease.InBack)
            .OnComplete(() => 
            {
                App.controller.entrance.OpenChooseDiary();
            });
    }
}
