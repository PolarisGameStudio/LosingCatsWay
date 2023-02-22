using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MEQ0003_Card : MonoBehaviour
{
    public GameObject canGet;
    public GameObject receive;
    
    public void SetData(bool hasFund, bool isRecived)
    {
        if (hasFund)
        {
            if (isRecived)
            {
                canGet.SetActive(false);
                receive.SetActive(true);
            }
            else
            {
                canGet.SetActive(true);
                receive.SetActive(false);
            }
        }
        else
        {
            canGet.SetActive(false);
            receive.SetActive(false);
        }
    }
}
