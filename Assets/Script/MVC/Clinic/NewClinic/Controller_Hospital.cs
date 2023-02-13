using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Firebase.Firestore;
using UnityEngine;

public class Controller_Hospital : ControllerBehavior
{
    //todo tutorial接上

    public void CloseToMap()
    {
        App.system.bgm.FadeOut();
        App.system.transition.Active(0, () =>
        {
            Close();
            App.controller.map.Open();
        });
    }
    
    public void Open()
    {
        App.view.hospital.Open();
        DOVirtual.DelayedCall(0.25f, OpenChooseFunction);
    }

    public void Close()
    {
        CloseChooseCat();
        App.view.hospital.Close();
    }

    public void OpenChooseFunction()
    {
        App.view.hospital.OpenChooseFunction();
    }

    public void CloseToFunction()
    {
        if (App.system.tutorial.isTutorial)
            return;
        CloseChooseCat();
        OpenChooseFunction();
    }
    
    public void CloseChooseFunction()
    {
        App.view.hospital.CloseChooseFunction();
    }

    public void SelectFunction(int index)
    {
        App.model.hospital.FunctionIndex = index;
        CloseChooseFunction();
        OpenChooseCat();
    }

    public void OpenChooseCat()
    {
        App.model.hospital.Cats = App.system.cat.GetCats();
        App.view.hospital.OpenChooseCat();
    }

    public void CloseChooseCat()
    {
        App.view.hospital.CloseChooseCat();
    }

    public void SelectCat(int index)
    {
        App.model.hospital.CatIndex = index;
    }

    public void ConfirmCat()
    {
        int index = App.model.hospital.CatIndex;
        App.model.hospital.SelectedCat = App.model.hospital.Cats[index];
        App.model.hospital.TmpCat = App.model.hospital.Cats[index];
        App.model.hospital.IsCatHasWorm = App.model.hospital.SelectedCat.cloudCatData.CatHealthData.IsBug;
        CloseChooseCat();
        OpenDoctorCheck();
    }

    public void OpenDoctorCheck()
    {
        App.view.hospital.OpenDoctorCheck();
    }

    public void DoctorCheckEnd()
    {
        CloseDoctorCheck();
        
        string sickId = App.model.hospital.SelectedCat.cloudCatData.CatHealthData.SickId;
        if (sickId is "SK001" or "SK002")
            OpenDoctorResult();
        else
            OpenInvoice();
    }
    
    public void CloseDoctorCheck()
    {
        App.view.hospital.CloseDoctorCheck();
    }

    public void SkipDoctorCheck()
    {
        App.view.hospital.SkipDoctorCheck();
    }

    public void OpenInvoice()
    {
        App.system.soundEffect.Play("ED00025");
        App.model.hospital.InvoiceMoney = GetInvoiceMoney();
        App.view.hospital.OpenInvoice();
    }

    public void CloseInvoice()
    {
        App.view.hospital.CloseInvoice();
    }

    public void SkipInvoice()
    {
        App.view.hospital.SkipInvoice();
    }

    private int GetInvoiceMoney()
    {
        int result = 0;
        int index = App.model.hospital.FunctionIndex;

        switch (index)
        {
            case 0:
                result = 300;
                break;
            case 1:
                result = 200;
                break;
            case 2:
                result = 150;
                break;
            case 3:
                result = 100;
                break;
            case 4:
                result = 500;
                break;
        }

        if (App.model.hospital.IsCatHasWorm)
            result += 150;
        
        return result;
    }
    
    public void CancelPay()
    {
        if (App.system.tutorial.isTutorial)
            return;
        
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            CloseInvoice();
            OpenChooseFunction();
        });
    }

    public void Pay()
    {
        App.system.confirm.Active(ConfirmTable.Fix, () =>
        {
            if (App.system.player.ReduceMoney(GetInvoiceMoney()))
            {
                ActiveFunction();
                CloseInvoice();
                OpenDoctorFuntion();
            }
            else
            {
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.NotEnoughCoin);
            }
        });
    }

    private void ActiveFunction()
    {
        Cat cat = App.model.hospital.SelectedCat;
        int index = App.model.hospital.FunctionIndex;

        App.system.soundEffect.Play("ED00024");
        
        if (index == 0)
        {
            // 看診
            cat.cloudCatData.CatHealthData.IsMetDoctor = true;
            
            int metCount = cat.cloudCatData.CatHealthData.MetDoctorCount;
            metCount -= 1;
            
            if (metCount <= 0)
            {
                metCount = 0;
                cat.cloudCatData.CatHealthData.SickId = string.Empty;
                cat.cloudCatData.CatHealthData.IsMetDoctor = false;

                cat.cloudCatData.CatSurviveData.ChangeRealSatiety(50);
                cat.cloudCatData.CatSurviveData.ChangeRealMoisture(50);
                cat.cloudCatData.CatSurviveData.ChangeRealFavourbility(50);
            }
            
            cat.cloudCatData.CatHealthData.MetDoctorCount = metCount;
            
            // 除蟲
            bool hasWorm = App.model.hospital.IsCatHasWorm;
            if (hasWorm)
                cat.cloudCatData.CatHealthData.IsBug = false;
        }

        if (index == 1)
        {
            cat.cloudCatData.CatHealthData.IsVaccine = true;
        }

        if (index == 2)
        {
            DateTime expired = App.system.myTime.MyTimeNow.AddDays(3);
            cat.cloudCatData.CatHealthData.NoBugExpireTimestamp = Timestamp.FromDateTime(expired);
        }

        if (index == 3)
        {
            cat.cloudCatData.CatHealthData.IsChip = true;
        }

        if (index == 4)
        {
            App.controller.pedia.AddLigationCount(cat.cloudCatData.CatData.Variety);
            cat.cloudCatData.CatHealthData.IsLigation = true;
        }
        
        App.model.hospital.SelectedCat = cat;
    }

    public void OpenDoctorFuntion() // 醫生做事
    {
        App.view.hospital.OpenDoctorFunction();
    }

    public void CloseDoctorFuntion()
    {
        App.view.hospital.CloseDoctorFunction();
    }

    public void SkipDoctorFunction()
    {
        App.view.hospital.SkipDoctorFunction();
    }

    public void OpenDoctorResult()
    {
        App.view.hospital.OpenDoctorResult();
    }

    public void CloseDoctorResult()
    {
        App.model.hospital.SelectedCat.ChangeSkin();
        App.view.hospital.CloseDoctorResult();
    }

    public void NextDoctorResult()
    {
        App.view.hospital.NextDoctorResult();
    }
}
