using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MEQ00002_Card : MonoBehaviour
{
    public GameObject notComplete;
    public GameObject complete;

    public TextMeshProUGUI progressText;

    public GameObject receive;

    public void SetData(Quest quest)
    {
        bool isReach = quest.IsReach;

        notComplete.SetActive(!isReach);
        complete.SetActive(isReach);

        progressText.text = "(" + quest.Progress + "/" + quest.TargetCount + ")";
        
        receive.SetActive(quest.IsReceived);
    }
}