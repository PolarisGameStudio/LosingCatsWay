using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Firebase.Firestore;
using UnityEngine;

public class Controller_Hospital : ControllerBehavior
{
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
        App.system.bgm.FadeIn().Play("Hospital");
        App.view.hospital.Open();
        DOVirtual.DelayedCall(0.25f, () =>
        {
            OpenChooseFunction();
            App.view.hospital.npc.Click();
        });
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
        App.system.soundEffect.Play("Button");
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
        App.system.soundEffect.Play("Button");
        App.model.hospital.CatIndex = index;
    }

    public void ConfirmCat()
    {
        App.system.soundEffect.Play("Button");
        int index = App.model.hospital.CatIndex;
        App.model.hospital.SelectedCat = App.model.hospital.Cats[index];
        App.model.hospital.TmpCat = App.model.hospital.Cats[index];
        App.model.hospital.IsCatHasWorm = App.model.hospital.SelectedCat.cloudCatData.CatHealthData.IsBug;
        CloseChooseCat();
        OpenDoctorCheck();
    }

    public void OpenDoctorCheck()
    {
        App.system.soundEffect.Play("ED00023");
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
        App.system.soundEffect.Stop("ED00023");
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
        App.system.soundEffect.Stop("ED00025");
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
                result = 400;
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
        
        App.system.soundEffect.Play("ED00026");
        App.system.confirm.Active(ConfirmTable.Hints_CancelPay, () =>
        {
            CloseInvoice();
            OpenChooseFunction();
        });
    }

    public void Pay()
    {
        App.system.soundEffect.Play("ED00026");
        App.system.confirm.Active(ConfirmTable.Hints_Pay, () =>
        {
            if (App.system.player.ReduceMoney(GetInvoiceMoney()))
            {
                ActiveFunction();
                CloseInvoice();
                OpenDoctorFuntion();
            }
            else
            {
                App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_NoMoney);
            }
        });
    }

    private void ActiveFunction()
    {
        Cat cat = App.model.hospital.SelectedCat;
        int index = App.model.hospital.FunctionIndex;

        if (index == 0)
        {
            // ??????
            cat.cloudCatData.CatHealthData.IsMetDoctor = true;
            
            int metCount = cat.cloudCatData.CatHealthData.MetDoctorCount;
            metCount -= 1;
            
            if (metCount <= 0)
            {
                metCount = 0;
                cat.cloudCatData.CatHealthData.SickId = string.Empty;
                cat.cloudCatData.CatHealthData.IsMetDoctor = false;

                cat.cloudCatData.CatSurviveData.ChangeRealSatiety(20);
                cat.cloudCatData.CatSurviveData.ChangeRealMoisture(20);
                cat.cloudCatData.CatSurviveData.ChangeRealFavourbility(20);
            }
            
            cat.cloudCatData.CatHealthData.MetDoctorCount = metCount;
            
            // ??????
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
            DateTime expired = App.system.myTime.MyTimeNow.AddDays(5);
            cat.cloudCatData.CatHealthData.NoBugExpireTimestamp = Timestamp.FromDateTime(expired);
        }

        if (index == 3)
        {
            cat.cloudCatData.CatHealthData.IsChip = true;
            cat.cloudCatData.CatData.ChipId = App.system.player.PlayerId;
        }

        if (index == 4)
        {
            App.controller.pedia.AddLigationCount(cat.cloudCatData.CatData.Variety);
            cat.cloudCatData.CatHealthData.IsLigation = true;
        }
        
        App.model.hospital.SelectedCat = cat;
        App.SaveData();
    }

    public void OpenDoctorFuntion() // ????????????
    {
        App.system.soundEffect.Play("ED00024");
        App.view.hospital.OpenDoctorFunction();
    }

    public void CloseDoctorFuntion()
    {
        App.view.hospital.CloseDoctorFunction();
        App.system.soundEffect.Stop("ED00024");
    }

    public void SkipDoctorFunction()
    {
        App.view.hospital.SkipDoctorFunction();
    }

    public void OpenDoctorResult()
    {
        App.system.soundEffect.Play("ED00063");
        App.view.hospital.OpenDoctorResult();
    }

    public void CloseDoctorResult()
    {
        App.system.soundEffect.Play("Button");
        App.model.hospital.SelectedCat.ChangeSkin();
        App.view.hospital.CloseDoctorResult();
    }

    public void NextDoctorResult()
    {
        App.system.soundEffect.Play("Button");
        App.view.hospital.NextDoctorResult();
    }

    public void ToggleChooseCatQuest()
    {
        App.system.soundEffect.Play("Button");
        App.view.hospital.ToggleChooseCatQuest();
    }

    public void OpenAboutPotion()
    {
        App.view.hospital.OpenAboutPotion();
    }

    public void CloseAboutPotion()
    {
        App.view.hospital.CloseAboutPotion();
    }
}
