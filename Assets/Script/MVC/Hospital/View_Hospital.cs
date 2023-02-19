using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class View_Hospital : ViewBehaviour
{
    [SerializeField] private GameObject bgMask;
    public NPC npc;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI diamondText;

    [Title("SubView")]
    [SerializeField] private View_HospitalChooseFunction ChooseFunction;
    [SerializeField] private View_HospitalChooseCat ChooseCat;
    [SerializeField] private View_HospitalDoctorCheck DoctorCheck;
    [SerializeField] private View_HospitalInvoice Invoice;
    [SerializeField] private View_HospitalDoctorFunction DoctorFunction;
    [SerializeField] private View_HospitalDoctorResult DoctorResult;
    
    // todo parallex

    public override void Init()
    {
        base.Init();
        App.system.player.OnCoinChange += OnCoinChange;
        App.system.player.OnDiamondChange += OnDiamondChange;
    }

    private void OnDiamondChange(object value)
    {
        int diamond = (int)value;
        diamondText.text = diamond.ToString();
    }

    private void OnCoinChange(object value)
    {
        int money = (int)value;
        moneyText.text = money.ToString();
    }

    public override void Open()
    {
        base.Open();
        npc.gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        npc.gameObject.SetActive(false);
    }
    
    public void OpenChooseFunction()
    {
        bgMask.SetActive(false);
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
        bgMask.SetActive(true);
        DoctorCheck.Open();
    }

    public void CloseDoctorCheck()
    {
        DoctorCheck.Close();
    }

    public void SkipDoctorCheck()
    {
        DoctorCheck.SkipDoctorCheck();
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
        Invoice.SkipTween();
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
        DoctorFunction.SkipDoctorFunction();
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
        DoctorResult.NextDoctorResult();
    }

    public void ToggleChooseCatQuest()
    {
        ChooseCat.ToggleChooseCatQuest();
    }
}
