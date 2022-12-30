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
using Unity.Advertisement.IosSupport;
using UnityEngine.Serialization;
using UnityEngine.tvOS;

//debug
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    [Title("UI")] 
    public GameObject startGameButton;
    [SerializeField] TextMeshProUGUI idText;

    [FormerlySerializedAs("notice")] [Title("NoticeSystem")] public PostSystem post;

    [Title("Login")] public UIView loginView;
    [Title("Confirm")] [SerializeField] private UIView confirmView;

    private bool isRequest = false;
    
    private async void Start()
    {
        #if UNITY_IOS

        var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
        Version currentVersion = new Version(Device.systemVersion);
        Version ios14 = new Version("14.5");

        if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED && currentVersion >= ios14)
        {
            Debug.Log("申請廣告權限");
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }

        #endif
        
        await FindObjectOfType<PostSystem>().Init();
        
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
            DOVirtual.DelayedCall(1f, post.Open);
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
        post.Open();
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
        post.Open();
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
        post.Open();
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