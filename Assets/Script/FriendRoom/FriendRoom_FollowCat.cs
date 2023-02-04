using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Lean.Touch;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendRoom_FollowCat : MonoBehaviour
{
    [Title("Require")]
    public FactoryContainer factory;
    public FriendRoom friendRoom;

    [Title("Basic")]
    public Camera mainCamera;
    public LeanDragCamera leanDragCamera;
    public LeanPinchCamera leanPinchCamera;

    public bool isFollowing;
    private Cat followCat;

    [Title("UI")] 
    public UIView view;
    public CatSkin catSkin;

    //StatusUI
    public TextMeshProUGUI catNameText;
    public TextMeshProUGUI catAgeText;
    public Image catSexImage;
    public TextMeshProUGUI varietyText;

    public Image satietyValueImage;
    public Image favorabilityValueImage;
    public Image moistureValueImage;

    public Image moodImage;
    private void Update()
    {
        if (!isFollowing)
            return;

        Vector3 tmp = followCat.transform.position;
        tmp.y += 0.5f;
        tmp.z = -10;

        mainCamera.transform.position = tmp;
    }

    public void Select(Cat cat)
    {
        if (isFollowing)
            return;

        StartFollow(cat);
        SetUI(cat);

        catSkin.SetActive(true);
        view.Show();
        friendRoom.Close();
    }
    
    public void Close()
    {
        leanDragCamera.enabled = true;
        leanPinchCamera.enabled = true;
        isFollowing = false;

        catSkin.SetActive(false);
        view.Hide();
        friendRoom.Open();
    }
    
    private void StartFollow(Cat cat)
    {
        followCat = cat;

        leanDragCamera.enabled = false;
        leanPinchCamera.enabled = false;

        float zoom = mainCamera.orthographicSize;
        DOTween.To(() => zoom, x => zoom = x, 3.5f, 0.5f).OnUpdate(() => { mainCamera.orthographicSize = zoom; });

        isFollowing = true;
    }

    private void SetUI(Cat cat)
    {
        var cloudCatData = cat.cloudCatData;
        
        catNameText.text = cloudCatData.CatData.CatName;
        
        catAgeText.text = cloudCatData.CatData.CatAge.ToString();
        catSexImage.sprite = factory.catFactory.GetCatSexSpriteWhite(cloudCatData.CatData.Sex);

        satietyValueImage.fillAmount = cloudCatData.CatSurviveData.Satiety / 100;
        favorabilityValueImage.fillAmount = cloudCatData.CatSurviveData.Favourbility / 100;
        moistureValueImage.fillAmount = cloudCatData.CatSurviveData.Moisture / 100;

        int mood = CatExtension.GetCatMood(cloudCatData);
        moodImage.sprite = factory.catFactory.GetMoodSprite(mood);

        varietyText.text = factory.stringFactory.GetCatVariety(cloudCatData.CatData.Variety);

        bool isKitty = CatExtension.GetCatAgeLevel(cloudCatData.CatData.SurviveDays) == 0;
        varietyText.text = isKitty ? factory.stringFactory.GetKittyName() : factory.stringFactory.GetCatVariety(cloudCatData.CatData.Variety);

        catSkin.ChangeSkin(cloudCatData);
    }
}
