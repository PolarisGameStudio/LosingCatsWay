using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;
using I2.Loc;
using UnityEngine.SceneManagement;

public class Controller_Settings : ControllerBehavior
{
    public void Init()
    {
        LoadSettings();
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

        App.system.confirm.Active(ConfirmTable.Fix, async () =>
        {
            ReleaseCats();
            
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            DocumentReference cityRef = db.Collection("Players").Document(userId);
            await cityRef.DeleteAsync();

            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            auth.SignOut();
            
            App.system.confirm.OnlyConfirm().Active(ConfirmTable.Fix, () =>
            {
                var tmp = FindObjectOfType<LoadScene>(); 
                if (tmp != null)
                    Destroy(tmp);
                StartCoroutine(LoadLoginScene());
            });
        });
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
}
