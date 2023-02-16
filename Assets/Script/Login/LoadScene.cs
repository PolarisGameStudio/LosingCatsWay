using System;
using System.Collections;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [Title("Scene")] public string mainSceneName;

    [Title("UI")] public UIView loadSceneView;
    public CanvasGroup loadSceneBg;
    public Image barValueImage;
    public TextMeshProUGUI barValueText;

    public Transform cat;

    private float minX;
    private float maxX;
    private float block;
    
    private AsyncOperation asyncLoad;

    private void Start()
    {
        var rect = transform.GetComponent<RectTransform>();

        var localScaleX = rect.localScale.x;
        
        minX = 80 * localScaleX;
        maxX = rect.rect.width * localScaleX - 80 * localScaleX;
        block = maxX - minX;
        
        cat.DOMoveX(minX, 0);
    }

    public void LoginGame()
    {
        barValueImage.fillAmount = 0;
        barValueText.text = "0%";
        loadSceneView.InstantShow();
        loadSceneBg.DOFade(1, 0.5f).SetEase(Ease.OutExpo).From(0);

        DOVirtual.DelayedCall(1f, () => 
        {
            StartCoroutine(LoadGameScene());
        });
    }

    public void Close()
    {
        loadSceneBg.DOFade(0, 0.5f).SetEase(Ease.OutExpo).From(1).OnComplete(() =>
        {
            Destroy(gameObject);
        }).SetDelay(2);
    }

    IEnumerator LoadGameScene()
    {
        asyncLoad = SceneManager.LoadSceneAsync(mainSceneName, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            var progress = asyncLoad.progress / 0.9f;
            barValueImage.fillAmount = progress;
            barValueText.text = (int)(progress * 100) + "%";

            cat.DOMoveX(minX + block * progress,0);
            
            if (progress >= 1)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}