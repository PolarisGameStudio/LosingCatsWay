using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

public class InternetChecker
{
    public bool CheckInternetStatus()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return false;
        return true;
    }
}
