using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Google;
using Sirenix.OdinInspector;
using TMPro;

//debug
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [Title("UI")] 
    public GameObject startGameButton;
    [SerializeField] TextMeshProUGUI idText;

    [Title("NoticeSystem")] public NoticeSystem notice;

    [Title("Login")] public UIView loginView;
    [Title("Confirm")] [SerializeField] private UIView confirmView;

    private bool isRequest = false;
    
    private void Start()
    {
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            WebClientId = "869333271731-41to0lgea513sqgreo46dq6m658hfnml.apps.googleusercontent.com"
        };

        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        if (auth.CurrentUser != null) // 已登入
        {
            idText.text = $"UID: {auth.CurrentUser.UserId}";
            startGameButton.SetActive(true);
            DOVirtual.DelayedCall(1f, notice.Open);
        }
        else
        {
            idText.text = $"UID: -";
            startGameButton.SetActive(false);
            loginView.InstantShow();
        }
    }

    public async void LoginByGoogle()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        var googleSignInResult = await GoogleSignIn.DefaultInstance.SignIn();

        if (googleSignInResult == null)
        {
            print("登入失敗");
            return;
        }

        var credential = Firebase.Auth.GoogleAuthProvider.GetCredential(googleSignInResult.IdToken, null);
        var result = auth.SignInWithCredentialAsync(credential);

        if (result == null)
        {
            print("登入失敗");
            return;
        }

        idText.text = $"UID: {auth.CurrentUser.UserId}";
        notice.Open();
        loginView.InstantHide();
        startGameButton.SetActive(true);
    }

    public async void LoginByVisitor()
    {
        if (isRequest)
            return;

        isRequest = true;
        
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        var result = await auth.SignInAnonymouslyAsync();

        if (result == null)
        {
            print("登入失敗");
            isRequest = false;
            return;
        }

        idText.text = $"UID: {auth.CurrentUser.UserId}";
        notice.Open();
        loginView.InstantHide();
        startGameButton.SetActive(true);
    }

    public void SignOut()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignOut();

        startGameButton.SetActive(false);
        loginView.InstantShow();
        idText.text = $"UID: -";
        
        CloseConfirm();
    }

    public void OpenAnnouncement()
    {
        notice.Open();
    }

    public void OpenConfirm()
    {
        confirmView.Show();
    }

    public void CloseConfirm()
    {
        confirmView.InstantHide();
    }
}