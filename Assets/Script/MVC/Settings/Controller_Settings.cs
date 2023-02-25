using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using DG.Tweening;
using Firebase.Auth;
using Firebase.Firestore;
using Google;
using UnityEngine;
using I2.Loc;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Controller_Settings : ControllerBehavior
{
    private IAppleAuthManager appleAuthManager;

    public void Init()
    {
        LoadSettings();
        
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            appleAuthManager = new AppleAuthManager(deserializer);
        }

        if (SceneManager.GetActiveScene().name == "SampleScene")
            CheckLinkStatus();
    }

    public void Open()
    {
        App.view.settings.Open();
    }

    public void Close()
    {
        SaveSettings();
        App.view.settings.Close();
    }

    public void ChangeSoundEffectVolume(float volume)
    {
        App.model.settings.SeVolume = volume;
    }

    public void ChangeBgmVolume(float volume)
    {
        App.model.settings.BgmVolume = volume;
    }

    public void SelectLanguage(int index)
    {
        App.model.settings.LanguageIndex = index;
        //TODO L2
        var languages = LocalizationManager.Sources[0].GetLanguages();
        LocalizationManager.CurrentLanguage = languages[index];
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SeVolume", App.model.settings.SeVolume);
        PlayerPrefs.SetFloat("BgmVolume", App.model.settings.BgmVolume);
        PlayerPrefs.SetInt("LanguageIndex", App.model.settings.LanguageIndex);
    }

    public void DeleteAccount()
    {
        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        App.system.confirm.Active(ConfirmTable.Hints_DeleteAccount, async () =>
        {
            ReleaseCats();
            
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference cityRef = db.Collection("Players").Document(userId);
            await cityRef.DeleteAsync();

            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            auth.SignOut();
            
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Hints_AlreadyDeleteAccount, () =>
            {
                var tmp = FindObjectOfType<LoadScene>(); 
                if (tmp != null)
                    Destroy(tmp);
                StartCoroutine(LoadLoginScene());
            });
        });
    }

    private void CheckLinkStatus()
    {
        bool flag = PlayerPrefs.HasKey("IsVisitor");
        App.view.settings.SetLinkStatus(flag);
    }

    private void ReleaseCats()
    {
        var cats = App.system.cat.GetCats();
        for (int i = 0; i < cats.Count; i++)
            App.system.abandon.AbandonCat(cats[i], $"Location_{Random.Range(0, 2)}"); // todo 更多Location
        App.system.cloudSave.SaveCloudCatDatas();
    }

    IEnumerator LoadLoginScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Login");
        while (!asyncLoad.isDone)
            yield return null;
    }
    
    private void LoadSettings()
    {
        if (!PlayerPrefs.HasKey("SeVolume"))
            ChangeSoundEffectVolume(1);
        else
            ChangeSoundEffectVolume(PlayerPrefs.GetFloat("SeVolume"));

        if (!PlayerPrefs.HasKey("BgmVolume"))
            ChangeBgmVolume(1);
        else
            ChangeBgmVolume(PlayerPrefs.GetFloat("BgmVolume"));

        if (!PlayerPrefs.HasKey("LanguageIndex"))
            SelectLanguage(0);
        else
            SelectLanguage(PlayerPrefs.GetInt("LanguageIndex"));
        
        SaveSettings();
    }

    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }

    public void OpenThanks()
    {
        App.system.thanks.Open();
    }

    #region Link

    public void LinkByApple()
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

                    auth.CurrentUser.LinkWithCredentialAsync(firebaseCredential).ContinueWith(task =>
                    {
                        if (task.IsFaulted || task.IsCanceled)
                        {
                            DOVirtual.DelayedCall(0.25f, () =>
                            {
                                App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix);
                            });
                            return;
                        }

                        DOVirtual.DelayedCall(0.25f, () =>
                        {
                            PlayerPrefs.DeleteKey("IsVisitor");
                            CheckLinkStatus();
                        });
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

    public async void LinkByGoogle()
    {
        print("T1");
        GoogleSignIn.DefaultInstance.SignOut();
        
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        var googleSignInResult = await GoogleSignIn.DefaultInstance.SignIn();

        if (googleSignInResult == null)
        {
            print("登入失敗");
            return;
        }
        
        var credential = Firebase.Auth.GoogleAuthProvider.GetCredential(googleSignInResult.IdToken, null);
        var result = auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(task => {
            
            if (task.IsFaulted || task.IsCanceled)
            {
                DOVirtual.DelayedCall(0.25f, () =>
                {
                    App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix);
                });
                return;
            }
            
            DOVirtual.DelayedCall(0.25f, () =>
            {
                PlayerPrefs.DeleteKey("IsVisitor");
                CheckLinkStatus();
            });
        });


        //TODO 補上登入後
    }

    #endregion
}
