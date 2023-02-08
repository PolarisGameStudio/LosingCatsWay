using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Logo : MonoBehaviour
{
    public Image logoImage;
    public CanvasGroup headPhone;
    
    void Start()
    {
        PlayLogo(() =>
        {
            PlayHeadPhone(() =>
            {
                SceneManager.LoadScene("Login", LoadSceneMode.Single);
            });
        });
    }

    private void PlayLogo(UnityAction unityAction = null)
    {
        logoImage.DOFade(1, 1.5f).From(0).SetDelay(0.5f);
        logoImage.DOFade(0, 2f).SetDelay(4).OnComplete(() =>
        {
            unityAction?.Invoke();
        });
    }

    private void PlayHeadPhone(UnityAction unityAction = null)
    {
        headPhone.DOFade(1, 1.5f).From(0).SetDelay(0.5f);
        headPhone.DOFade(0, 2f).SetDelay(4).OnComplete(() =>
        {
            unityAction?.Invoke();
        });
    }
}
