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
    [ReadOnly] public int dollCount;
    
    [Title("Cat")] public FindCatObject[] cats;
    [SerializeField] private UIParticle[] catLoves;

    CloudCatData cloudCatData;

    public Callback OnGameEnd;

    public void Open()
    {
        uiView.InstantShow();
        
        if (!App.system.tutorial.isTutorial)
            ShowHowToPlay();
        
        Init();
    }

    public void ShowHowToPlay()
    {
        string country = App.factory.stringFactory.GetCountryByLocaleIndex();
        string title = howToPlayData.titleData[country];
        string[] descripts = howToPlayData.descriptData[country];
        Sprite[] sprites = howToPlayData.sprites;
        App.system.howToPlay.SetData(title, descripts, sprites).Open(true, null, StartGame);
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
        dollCount = 0;

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
        float waitTime = Random.Range(0.25f, 0.75f);
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
        if (!IsAnyCatShowing())
            return;
        
        VibrateExtension.Vibrate(VibrateType.Nope);
        App.system.soundEffect.Play("Button");
        
        FindCatObject findCatObject = cats[index];

        if (!findCatObject.isShowing || findCatObject.isDoll)
        {
            countDown -= 1;
            Hide();
            return;
        }

        catLoves[index].Play();
        cats[index].Stop();
        heart++;
        
        if (heart >= 3)
        {
            Stop();
            DOVirtual.DelayedCall(1f, Success);
        }

        // tween
        var heartTmp = hearts[heart - 1];
        heartTmp.transform.DOScale(Vector3.one, 0.25f).From(Vector3.zero).SetEase(Ease.OutExpo).SetDelay(1).OnStart(() =>
        {
            heartTmp.SetActive(true);
        });
    }

    public void Close()
    {
        SetCloudCatDataToUse(false);
        App.system.transition.OnlyOpen();
        DOVirtual.DelayedCall(1f, () =>
        {
            OnGameEnd?.Invoke();
            App.system.findCat.ActiveCurrentGate();
            uiView.InstantHide();
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
                App.system.catchCat.Active(mapIndex, cloudCatData);
                cloudCatData = null;

                if (App.system.tutorial.isTutorial)
                    App.system.tutorial.NextDirector();
            });
        });
    }

    void Failed()
    {
        App.system.confirm.OnlyConfirm().Active(ConfirmTable.FindGameFailed, () =>
        {
            if (App.system.tutorial.isTutorial)
            {
                App.system.transition.Active(1f, () =>
                {
                    Open();
                    ShowHowToPlay();
                });
                return;
            }
            
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

    private void Hide()
    {
        for (int i = 0; i < cats.Length; i++)
            if (cats[i].isShowing)
                cats[i].Hide();
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
        if (cloudCatData == null)
            return;
        cloudCatData.CatSurviveData.IsUseToFind = value;
        App.system.cloudSave.UpdateCloudCatSurviveData(cloudCatData);
    }

    private bool IsAnyCatShowing()
    {
        for (int i = 0; i < cats.Length; i++)
        {
            if (cats[i].isShowing)
                return true;
        }

        return false;
    }
    
    #region ApplicationProcess

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
            SetCloudCatDataToUse(false);
        else
            SetCloudCatDataToUse(true);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SetCloudCatDataToUse(false);
        else
            SetCloudCatDataToUse(true);
    }

    private void OnApplicationQuit()
    {
        SetCloudCatDataToUse(false);
    }
    
    #endregion
}
