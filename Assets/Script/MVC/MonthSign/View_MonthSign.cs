using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class View_MonthSign : ViewBehaviour
{
    [SerializeField] private Button signButton;
    [SerializeField] private Button resignButton;
    [SerializeField] private TextMeshProUGUI monthText;
    [SerializeField] private TextMeshProUGUI monthNameText;
    [SerializeField] private TextMeshProUGUI missDayText;
    [SerializeField] private TextMeshProUGUI continueDayText;
    [SerializeField] private Card_MonthSign[] dateObjects;

    public override void Init()
    {
        base.Init();
        
        RefreshDateObject();
        
        App.model.monthSign.OnSignIndexsChange += OnSignIndexsChange;
        App.model.monthSign.OnMonthChange += OnMonthChange;
        App.model.monthSign.OnResignCountChange += OnResignCountChange;
        App.model.monthSign.OnMonthRewardsChange += OnMonthRewardsChange;
    }

    private void OnMonthRewardsChange(object value)
    {
        List<Reward> rewards = (List<Reward>)value;
        for (int i = 0; i < rewards.Count; i++)
        {
            if (i >= dateObjects.Length)
                break;
            dateObjects[i].SetReward(rewards[i]);
        }
    }

    private void OnMonthChange(object value)
    {
        int month = Convert.ToInt32(value);
        
        monthText.text = month.ToString("00");
        string monthName = new DateTimeFormatInfo().GetMonthName(month);
        monthNameText.text = monthName.Substring(0, 3).ToUpper();
    }

    public void OnSignIndexsChange(object value)
    {
        List<int> signIndexs = (List<int>)value;

        if (signIndexs.Count <= 0)
            return;

        #region CreateCalender/Sign

        for (int i = 0; i < dateObjects.Length; i++)
        {
            int index = i;

            dateObjects[index].SetActive(true);
            dateObjects[index].SetDate(index + 1);
                
            if (signIndexs[index] == 1)
                dateObjects[index].IsSign = true;
            else
                dateObjects[index].IsSign = false;
        }

        int day = App.system.myTime.MyTimeNow.Day;
        if (dateObjects[day - 1].IsSign) signButton.interactable = false;

        #endregion

        #region NoSignDays

        int missDay = 0;
        for (int i = 0; i < signIndexs.Count; i++)
        {
            if (i >= day - 1) break;
            if (signIndexs[i] == 0) missDay++;
        }

        missDayText.text = missDay.ToString("00");

        #endregion

        #region ContinueSignDays

        int continueDays = 0;

        for (int i = day - 1; i > 0; i--)
        {
            int sign = signIndexs[i];
            if (sign == 1) continueDays++;
            else break;
        }

        continueDayText.text = continueDays.ToString("00");

        #endregion
    }

    private void OnResignCountChange(object value)
    {
        int count = Convert.ToInt32(value);

        if (count <= 0)
        {
            resignButton.interactable = false;
            return;
        }

        resignButton.interactable = true;
    }

    private void RefreshDateObject()
    {
        var nowDate = App.system.myTime.MyTimeNow;
        int dayInMonth = DateTime.DaysInMonth(nowDate.Year, nowDate.Month);

        for (int i = 0; i < dateObjects.Length; i++)
            dateObjects[i].SetActive(i < dayInMonth);
        
        //TODO 沒月卡return

        List<int> vipDays = new List<int> { 4, 7, 11, 14, 18, 21, 25, 28 };
        for (int i = 0; i < dateObjects.Length; i++)
            dateObjects[i].SetDouble(vipDays.Contains(i + 1));
    }
}