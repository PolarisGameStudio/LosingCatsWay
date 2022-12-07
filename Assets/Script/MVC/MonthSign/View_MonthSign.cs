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
        App.model.monthSign.OnSignIndexsChange += OnSignIndexsChange;
        App.model.monthSign.OnMonthChange += OnMonthChange;
        App.model.monthSign.OnSelectedMonthSignRewardDataChange += OnSelectedMonthSignRewardDataChange;
        App.model.monthSign.OnResignCountChange += OnResignCountChange;
    }

    private void OnMonthChange(object value)
    {
        int month = Convert.ToInt32(value);
        monthText.text = month.ToString("00");
        string monthName = new DateTimeFormatInfo().GetMonthName(month).ToString();
        monthNameText.text = monthName.Substring(0, 3).ToUpper();
    }

    public void OnSignIndexsChange(object value)
    {
        List<int> signIndexs = (List<int>)value;

        if (signIndexs.Count <= 0) return; //ªÅ­È

        #region CreateCalender/Sign

        for (int i = 0; i < dateObjects.Length; i++)
        {
            int index = i;

            if (index < signIndexs.Count)
            {
                dateObjects[index].SetActive(true);
                dateObjects[index].SetDate(index + 1);
                
                if (signIndexs[index] == 1)
                    dateObjects[index].IsSign = true;
                else
                    dateObjects[index].IsSign = false;
            }
            else
            {
                dateObjects[index].SetActive(false);
            }
        }

        int day = DateTime.Now.Day;
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

    private void OnSelectedMonthSignRewardDataChange(object value)
    {
        MonthSignRewardData data = (MonthSignRewardData)value;

        for (int i = 0; i < data.signRewards.Count; i++)
        {
            if (i >= dateObjects.Length) break;

            var reward = data.GetReward(i);
            dateObjects[i].SetReward(reward);
        }
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
}