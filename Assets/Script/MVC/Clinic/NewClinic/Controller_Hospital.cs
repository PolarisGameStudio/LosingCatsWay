using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Hospital : ControllerBehavior
{
    public void Open()
    {
        App.view.hospital.Open();
        OpenChooseFunction();
    }

    public void Close()
    {
        App.view.hospital.Close();
    }

    public void OpenChooseFunction()
    {
        App.view.hospital.OpenChooseFunction();
    }

    public void CloseChooseFunction()
    {
        App.view.hospital.CloseChooseFunction();
    }

    public void SelectFunction(int index)
    {
        App.model.hospital.FunctionIndex = index;
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
        OpenDoctorCheck();
    }

    public void OpenDoctorCheck()
    {
        App.view.hospital.OpenDoctorCheck();
    }

    public void DoctorCheckEnd()
    {
        CloseDoctorCheck();
        
        //todo 討論看診之外的功能在生病時能不能不讓生病貓咪使用
        
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

    public void CancelPay()
    {
        //
    }

    public void Pay()
    {
        //
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
        App.view.hospital.CloseDoctorResult();
    }

    public void NextDoctorResult()
    {
        App.view.hospital.NextDoctorResult();
    }
}
