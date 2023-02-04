using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_Hospital : ViewBehaviour
{
    [SerializeField] private View_HospitalChooseFunction ChooseFunction;
    [SerializeField] private View_HospitalChooseCat ChooseCat;
    [SerializeField] private View_HospitalDoctorCheck DoctorCheck;
    [SerializeField] private View_HospitalInvoice Invoice;
    [SerializeField] private View_HospitalDoctorFunction DoctorFunction;
    [SerializeField] private View_HospitalDoctorResult DoctorResult;
    
    public void OpenChooseFunction()
    {
        ChooseFunction.Open();
    }

    public void CloseChooseFunction()
    {
        ChooseFunction.Close();
    }

    public void OpenChooseCat()
    {
        ChooseCat.Open();
    }

    public void CloseChooseCat()
    {
        ChooseCat.Close();
    }

    public void OpenDoctorCheck()
    {
        DoctorCheck.Open();
    }

    public void CloseDoctorCheck()
    {
        DoctorCheck.Close();
    }

    public void SkipDoctorCheck()
    {
        //
    }

    public void OpenInvoice()
    {
        Invoice.Open();
    }

    public void CloseInvoice()
    {
        Invoice.Close();
    }

    public void SkipInvoice()
    {
        //
    }

    public void OpenDoctorFunction()
    {
        DoctorFunction.Open();
    }

    public void CloseDoctorFunction()
    {
        DoctorFunction.Close();
    }

    public void SkipDoctorFunction()
    {
        //
    }

    public void OpenDoctorResult()
    {
        DoctorResult.Open();
    }

    public void CloseDoctorResult()
    {
        DoctorResult.Close();
    }

    public void NextDoctorResult()
    {
        //
    }
}
