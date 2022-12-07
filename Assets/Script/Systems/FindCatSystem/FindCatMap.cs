using System.Collections;
using System.Collections.Generic;
using Coffee.UIExtensions;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FindCatMap : MvcBehaviour
{
    [SerializeField] private int mapIndex;
    [SerializeField] private HowToPlayData howToPlayData;
    
    [Title("UI")] 
    [SerializeField] private UIView uiView;
    public Image timer;
    [SerializeField] private TextMeshProUGUI timerText;
    public GameObject[] hearts;

    private int heart = 0;
    private float countDown = 15f;
    
    [ReadOnly] public bool IsTutorial;

    [Title("Cat")] public FindCatObject[] cats;
    [SerializeField] private UIParticle[] catLoves;

    [Title("G8")] [SerializeField] private bool isG8;

    CloudCatData cloudCatData;

    public Callback OnGameEnd;

    #region OnApplication

    private void OnApplicationQuit()
    {
        if (cloudCatData == null) return;
        SetCloudCatDataToUse(false);
    }

    #endregion
    
    public void Open()
    {
        App.system.bgm.FadeIn().Play("Street");
        uiView.InstantShow();
        
        string country = App.factory.stringFactory.GetCountryByLocaleIndex();
        string title = howToPlayData.titleData[country];
        string[] descripts = howToPlayData.descriptData[country];
        Sprite[] sprites = howToPlayData.sprites;
        App.system.howToPlay.SetData(title, descripts, sprites).Open(true, null, StartGame);
        
        Init();
    }

    public void SetCloudCatData(CloudCatData cloudCatData)
    {
        if (cloudCatData == null) return;
        
        this.cloudCatData = cloudCatData;
        SetCloudCatDataToUse(true);
    }
    
    void Init() //初始化遊戲
    {
        countDown = 15f;
        heart = 0;

        for (int i = 0; i < hearts.Length; i++)
            hearts[i].SetActive(false);

        Stop();
        RefreshTimeImage();
    }

    public void StartGame()
    {
        Play();
    }

    public void NextCat()
    {
        float waitTime = Random.Range(0.5f, 1.5f);
        Invoke(nameof(DrawCat), waitTime);
    }

    private void DrawCat()
    {
        int randomIndex = Random.Range(0, cats.Length);
        cats[randomIndex].Active();
    }

    private void TimeCount()
    {
        countDown -= 0.01f;
        RefreshTimeImage();

        if (countDown <= 0)
        {
            Stop();
            Failed();
        }
    }

    private void RefreshTimeImage()
    {
        float value = countDown / 15f;
        timer.fillAmount = value;
        timerText.text = countDown.ToString("0");
    }

    public void Click(int index)
    {
        if (heart >= 3)
            return;
        if (countDown <= 0)
            return;
        
        VibrateExtension.Vibrate(VibrateType.Nope);
        
        FindCatObject findCatObject = cats[index];

        if (!findCatObject.isShowing)
        {
            countDown -= 1;
            return;
        }

        cats[index].Stop();
        heart++;
        
        catLoves[index].Play();
        
        if (heart >= 3)
            Stop();

        // tween
        DOVirtual.DelayedCall(1f, () =>
        {
            var heartTmp = hearts[heart - 1];
            heartTmp.SetActive(true);
            heartTmp.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutExpo);
            
            if (heart >= 3)
                Success();
        });
    }

    public void Close()
    {
        if (isG8)
        {
            SetCloudCatDataToUse(false);
            App.system.bgm.FadeOut();
            App.system.transition.Active(0, () =>
            {
                OnGameEnd?.Invoke();
                App.system.findCat.ActiveCurrentMap();
            });
            return;
        }
        
        SetCloudCatDataToUse(false);
        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            OnGameEnd?.Invoke();
            uiView.InstantHide();
            App.controller.map.Open();
            App.system.findCat.Close();
        });
    }

    void Success()
    {
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.FindGameSuccess, () =>
        {
            App.system.bgm.FadeOut();
            App.system.transition.Active(0, () =>
            {
                OnGameEnd?.Invoke();
                uiView.Hide();
                App.system.findCat.Close();
                App.system.catchCat.Active(mapIndex, cloudCatData, IsTutorial);
            });
        });
    }

    void Failed()
    {
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.FindGameFailed, () =>
        {
            OnGameEnd?.Invoke();
            Close();
        });
    }

    private void Stop()
    {
        CancelInvoke(nameof(TimeCount));
        CancelInvoke(nameof(DrawCat));

        for (int i = 0; i < cats.Length; i++)
            cats[i].QuickHide();
    }

    private void Play()
    {
        NextCat();
        InvokeRepeating(nameof(TimeCount), 0, 0.01f);
    }

    public void Pause()
    {
        Stop();
        App.system.howToPlay.Open(false, null, Play);
    }

    public void Exit()
    {
        Stop();
        App.system.confirm.Active(ConfirmTable.ExitComfirm, okEvent: Close, cancelEvent: Play);
    }

    private void SetCloudCatDataToUse(bool value)
    {
        cloudCatData.CatSurviveData.IsUseToFind = value;
        App.system.cloudSave.UpdateCloudCatSurviveData(cloudCatData);
    }
}
