using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Google;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Serialization;

#if UNITY_IOS && !UNITY_EDITOR
using Unity.Advertisement.IosSupport;
using UnityEngine.tvOS;
#endif

//debug
using UnityEngine.UI;

public class Login : MyApplication
{
    [Title("UI")] public GameObject startGameButton;
    [SerializeField] TextMeshProUGUI idText;

    [Title("Login")] 
    public UIView loginView;
    public Image bgMask;

    [Title("LoginButtons")] [SerializeField]
    private GameObject googleButton;
    [SerializeField] private GameObject appleButton;

    private bool isRequest = false;
    private IAppleAuthManager appleAuthManager;

    private void Init()
    {
        system.bgm.Init();
        system.soundEffect.Init();
        controller.settings.Init();
        
        system.bgm.FadeOut(0).FadeIn(8).Play("Login");
    }
    
    private async void Start()
    {
        Init();

        InternetChecker internetChecker = new InternetChecker();
        if (!internetChecker.CheckInternetStatus())
        {
            system.confirm.ActiveByBlock(ConfirmTable.Hints_Network);
            return;
        }
        
        VersionChecker versionChecker = new VersionChecker();
        bool isActive = await versionChecker.Check();
        int status = await versionChecker.CheckStatus();

        if (!isActive)
        {
            ConfirmTable confirmTable = ConfirmTable.Hints_Maintain;

            if (status == 0)
                confirmTable = ConfirmTable.Hints_Maintain;
            else if (status == 1)
                confirmTable = ConfirmTable.Hints_Maintain;
            else if (status == 2)
                confirmTable = ConfirmTable.Hints_VersionNotSame;
            
            system.confirm.ActiveByBlock(confirmTable);
            
            return;
        }

        googleButton.SetActive(false);
        appleButton.SetActive(false);
        
#if UNITY_IOS && !UNITY_EDITOR

        appleButton.SetActive(true);

        var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
        Version currentVersion = new Version(Device.systemVersion);
        Version ios14 = new Version("14.5");

        if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED && currentVersion >= ios14)
        {
            Debug.Log("申請廣告權限");
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
#else
        
        googleButton.SetActive(true);
        
#endif

        await FindObjectOfType<PostSystem>().Init();

        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            appleAuthManager = new AppleAuthManager(deserializer);
        }

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
            system.post.Open(true);
        }
        else
        {
            idText.text = $"UID: -";
            startGameButton.SetActive(false);
            loginView.InstantShow();
        }
        
        bgMask.DOFade(0, 1).From(1);
    }

    private void Update()
    {
        if (appleAuthManager != null)
        {
            appleAuthManager.Update();
        }
    }

    public void LoginByApple()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        var rawNonce = MathfExtension.GenerateRandomString(32);
        var nonce = MathfExtension.GenerateSHA256NonceFromRawNonce(rawNonce);

        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName, nonce);

        this.appleAuthManager.LoginWithAppleId(
            loginArgs,
            credential =>
            {
                // Obtained credential, cast it to IAppleIDCredential
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    var identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken);
                    var authorizationCode = Encoding.UTF8.GetString(appleIdCredential.IdentityToken);

                    // And now you have all the information to create/login a user in your system
                    Firebase.Auth.Credential firebaseCredential =
                        Firebase.Auth.OAuthProvider.GetCredential("apple.com", identityToken, rawNonce, authorizationCode);

                    auth.SignInWithCredentialAsync(firebaseCredential).ContinueWith(task =>
                    {
                        if (task.IsCanceled)
                        {
                            Debug.LogError("SignInWithCredentialAsync was canceled.");
                            return;
                        }

                        if (task.IsFaulted)
                        {
                            Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                            return;
                        }
                        
                        if (task.IsCompletedSuccessfully)
                        {
                            PlayerPrefs.DeleteKey("IsVisitor");
                            idText.text = $"UID: {auth.CurrentUser.UserId}";
                            system.post.Open();
                            loginView.InstantHide();
                            startGameButton.SetActive(true);
                        }
                    });
                }
            },
            error =>
            {
                // Something went wrong
                var authorizationErrorCode = error.GetAuthorizationErrorCode();
                print(authorizationErrorCode);
            });
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
        var result = await auth.SignInWithCredentialAsync(credential);

        if (result == null)
            return;

        PlayerPrefs.DeleteKey("IsVisitor");

        idText.text = $"UID: {auth.CurrentUser.UserId}";
        system.post.Open();
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

        PlayerPrefs.SetInt("IsVisitor", 1);
        
        idText.text = $"UID: {auth.CurrentUser.UserId}";
        system.post.Open();
        loginView.InstantHide();
        startGameButton.SetActive(true);
    }

    public void SignOut()
    {
        system.confirm.Active(ConfirmTable.Hints_LogOut, () =>
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.SignOut();

#if UNITY_ANDROID && !UNITY_EDITOR
            GoogleSignIn.DefaultInstance.SignOut();
#endif
            
            PlayerPrefs.DeleteKey("IsVisitor");
            startGameButton.SetActive(false);
            loginView.InstantShow();
            idText.text = $"UID: -";
            isRequest = false;
        });
    }

    public void OpenAnnouncement()
    {
        system.soundEffect.Play("ED00004");
        system.post.Open();
    }

    public void OpenSettings()
    {
        system.soundEffect.Play("ED00004");
        controller.settings.Open();
    }
}