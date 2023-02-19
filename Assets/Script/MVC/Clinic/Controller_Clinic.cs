using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Firebase.Firestore;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Clinic : ControllerBehavior
{
    [SerializeField] private GameObject npcObject;
    [SerializeField] private GameObject chooseCatButton;
    [SerializeField] private GameObject maskObject;

    [Title("Tutorial")] [SerializeField] private Button closeButton;
    [SerializeField] private Button cancelPayButton;
    
    [Title("Debug")] [SerializeField] private SkeletonGraphic[] skippingGraphics;

    public Callback OnFunctionComplete;

    public void Open()
    {
        App.system.bgm.FadeIn().Play("Hospital");
        npcObject.SetActive(true);
        App.view.clinic.Open();

        DOVirtual.DelayedCall(0.3f, OpenChooseFunction);

        closeButton.interactable = !App.system.tutorial.isTutorial;
        cancelPayButton.interactable = !App.system.tutorial.isTutorial;
    }

    public void Close()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            App.view.clinic.Close();
            npcObject.SetActive(false);
            App.controller.map.Open();
        });
    }

    public void BackToFunction()
    {
        CloseChooseCat();
        OpenChooseFunction();
    }
    
    #region ChooseFunction

    public void OpenChooseFunction()
    {
        npcObject.SetActive(true);
        maskObject.SetActive(false);
        App.view.clinic.chooseFunction.Open();
    }

    private void CloseChooseFuntion()
    {
        App.view.clinic.chooseFunction.Close();
    }

    public void ChooseFunction(int index)
    {
        App.model.clinic.FunctionIndex = index;
        CloseChooseFuntion();
        OpenChooseCat();
    }

    #endregion

    #region ChooseCat

    private void OpenChooseCat()
    {
        App.view.clinic.chooseCat.Open();

        DOTween.Kill(chooseCatButton.transform);
        chooseCatButton.SetActive(false);

        var cats = new List<Cat>(App.system.cat.GetCats());
        App.model.clinic.MyCats = cats;
    }

    private void CloseChooseCat()
    {
        npcObject.SetActive(false);
        App.view.clinic.chooseCat.Close();
    }

    public void ChooseCat(int index)
    {
        App.model.clinic.CatIndex = index;
        // chooseCatButton.SetActive(true);
        if (chooseCatButton.activeSelf)
            return;
        chooseCatButton.transform.DOScale(Vector2.one, 0.2f).From(Vector2.zero)
            .OnStart(() => chooseCatButton.SetActive(true));
    }

    public void SelectCat()
    {
        var cats = App.model.clinic.MyCats;
        var cat = cats[App.model.clinic.CatIndex];
        int index = App.model.clinic.FunctionIndex;

        if (index == 0)
        {
            if (string.IsNullOrEmpty(cat.cloudCatData.CatHealthData.SickId) && !cat.cloudCatData.CatHealthData.IsBug)
                return;
        }
        
        if (index == 1)
        {
            if (cat.cloudCatData.CatHealthData.IsVaccine)
                return;
        }
        
        if (index == 2)
        {
            if (cat.cloudCatData.CatHealthData.IsBug)
                return;
        }
        
        if (index == 3)
        {
            if (cat.cloudCatData.CatHealthData.IsChip)
                return;
        }
        
        if (index == 4)
        {
            if (cat.cloudCatData.CatHealthData.IsLigation)
                return;
        }
        
        App.model.clinic.SelectedCat = cat;
        App.model.clinic.SickId = cat.cloudCatData.CatHealthData.SickId;
        CloseChooseCat();
        OpenCheck();
    }

    #endregion

    #region Check

    private void OpenCheck()
    {
        maskObject.SetActive(true);
        App.view.clinic.checkCat.Open();
    }

    #endregion

    #region Invoice

    public void OpenInvoice()
    {
        Cat cat = App.model.clinic.SelectedCat;
        
        int index = App.model.clinic.FunctionIndex;
        var payment = new Dictionary<string, int>();
        
        #region Invoice

        if (index == 0)
        {
            if (cat.cloudCatData.CatHealthData.IsBug)
            {
                payment.Add("CP007", 300);
            }
            payment.Add("CP001", 400);
        }

        if(index == 1)
        {
            payment.Add("CP002", 300);
        }

        if (index == 2)
        {
            payment.Add("CP003", 300);
        }

        if (index == 3)
        {
            payment.Add("CP004", 150);
        }

        if (index == 4)
        {
            if (cat.cloudCatData.CatData.Sex == 0)
                payment.Add("CP005", 200);
            else
                payment.Add("CP006", 200);
        }

        #endregion

        App.model.clinic.Payment = payment;
        App.view.clinic.invoice.Open();

    }

    private void CloseInvoice()
    {
        App.view.clinic.invoice.Close();
    }

    public void Pay()
    {
        var payment = App.model.clinic.Payment;
        int totalCost = 0;
        for (int i = 0; i < payment.Count; i++)
            totalCost += payment.ElementAt(i).Value;

        if (!App.system.tutorial.isTutorial)
        {
            if (!App.system.player.ReduceMoney(totalCost))
            {
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_NoMoney);
                return;
            }
        }

        CloseInvoice();
        DoFunction();
        ClearCatSick();
        OpenFunction();
    }

    // public void CancelPay()
    // {
    //     App.system.confirm.Active(ConfirmTable.Fix, () => 
    //     {
    //         App.view.clinic.invoice.Close();
    //         OpenChooseFunction();
    //     });
    // }

    #endregion

    #region Function

    public void DoFunction()
    {
        Cat cat = App.model.clinic.SelectedCat;
        int index = App.model.clinic.FunctionIndex;

        if (index == 0)
        {
            if (!string.IsNullOrEmpty(cat.cloudCatData.CatHealthData.SickId))
            {
                cat.cloudCatData.CatHealthData.IsMetDoctor = true;

                if (cat.cloudCatData.CatHealthData.SickId is not "SK001" and not "SK002")
                    cat.cloudCatData.CatHealthData.MetDoctorCount = Mathf.Clamp(cat.cloudCatData.CatHealthData.MetDoctorCount - 1, 0, 3);
            }

            cat.cloudCatData.CatHealthData.IsBug = false;
            App.model.clinic.SelectedCat.ChangeSkin();
        }
        else if (index == 1)
        {
            cat.cloudCatData.CatHealthData.IsVaccine = true;
        }
        else if(index == 2)
        {
            DateTime noBugExpireDate = App.system.myTime.MyTimeNow.AddDays(3);
            cat.cloudCatData.CatHealthData.NoBugExpireTimestamp = Timestamp.FromDateTime(noBugExpireDate);
        }
        else if (index == 3)
        {
            cat.cloudCatData.CatHealthData.IsChip = true;
            cat.cloudCatData.CatData.ChipId = App.system.player.PlayerId;
        }
        else if (index == 4)
        {
            cat.cloudCatData.CatHealthData.IsLigation = true;
        }

        App.model.clinic.SelectedCat = cat;
        App.model.clinic.MetCount = cat.cloudCatData.CatHealthData.MetDoctorCount;
    }

    private void OpenFunction()
    {
        App.view.clinic.function.Open();
    }

    #endregion

    #region Result

    public void OpenCheckResult()
    {
        // DoFunction();
        App.model.clinic.SelectedCat = App.model.clinic.SelectedCat; //ValueChange
        App.view.clinic.result.Open();
    }

    public void ReadResult()
    {
        App.view.clinic.result.ReadResult();
    }

    public void CloseCheckResult()
    {
        // DoFunction();
        // ClearCatSick();
        App.view.clinic.result.Close();
        
        OnFunctionComplete?.Invoke();
        if (App.system.tutorial.isTutorial)
            App.system.tutorial.Next();
    }

    private void ClearCatSick()
    {
        CloudCatData cloudCatData = App.model.clinic.SelectedCat.cloudCatData;
        
        if (cloudCatData.CatHealthData.SickId is "SK001" or "SK002")
            return;
        
        if (cloudCatData.CatHealthData.MetDoctorCount <= 0)
        {
            cloudCatData.CatHealthData.SickId = string.Empty;
            cloudCatData.CatHealthData.IsMetDoctor = false;
            
            cloudCatData.CatSurviveData.ChangeRealSatiety(50);
            cloudCatData.CatSurviveData.ChangeRealMoisture(50);
            cloudCatData.CatSurviveData.ChangeRealFavourbility(50);

            App.model.clinic.SelectedCat.isPauseGame = false;
        }
        
        App.model.clinic.SelectedCat.ChangeSkin();
    }

    #endregion

    public void SkipAnimation()
    {
        for (int i = 0; i < skippingGraphics.Length; i++)
        {
            if (!skippingGraphics[i].gameObject.activeInHierarchy) continue;
            var track = skippingGraphics[i].AnimationState.GetCurrent(0);
            track.TrackTime = track.Animation.Duration;
        }
    }

    public void SkipInvoice()
    {
        App.view.clinic.invoice.Skip();
    }
}
