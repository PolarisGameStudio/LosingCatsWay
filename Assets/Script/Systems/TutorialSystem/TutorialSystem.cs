using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using Lean.Touch;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSystem : MvcBehaviour
{
    [SerializeField] private UIView uiView;
    
    [Title("Directors")] [SerializeField] private List<TutorialDirector> directors;
    [ReadOnly] public TutorialDirector currentDirector;
    
    [Title("Setup")]
    [SerializeField] private LeanDragCamera dragCamera;
    [SerializeField] private LeanPinchCamera pinchCamera;
    [SerializeField] private UIView blackBg;
    [SerializeField] private Image bgImage;

    private int directorIndex = -1;
    [ReadOnly] public bool isTutorial;

    public void Init()
    {
        for (int i = 0; i < directors.Count; i++)
            directors[i].gameObject.SetActive(false);
        
        if (directorIndex == -1) // TODO 新手教學結束Index記錄
            NextDirector();
    }

    private void SetDirector(int index)
    {
        if (index < 0 || index >= directors.Count)
        {
            Debug.LogWarning("Director not found.");
            Close();
            SetCameraDrag(true);
            SetCameraPinch(true);
            SetBlackBg(false);
            return;
        }

        Open();
        currentDirector = directors[index];
        currentDirector.gameObject.SetActive(true);
        currentDirector.Init();
        currentDirector.NextAction();
        isTutorial = true;
    }

    public void NextDirector()
    {
        if (directorIndex >= 0)
            directors[directorIndex].gameObject.SetActive(false);
        SetDirector(directorIndex + 1);
    }

    public void Open()
    {
        uiView.InstantShow();
    }
    
    public void Close()
    {
        directorIndex++;
        uiView.InstantHide();
    }
    
    public void SetCameraDrag(bool value)
    {
        dragCamera.enabled = value;
    }

    public void SetCameraPinch(bool value)
    {
        pinchCamera.enabled = value;
    }

    public void SetBlackBg(bool value)
    {
        if (value)
            blackBg.Show();
        else
            blackBg.InstantHide();
    }

    public void SetBg(Sprite sprite)
    {
        if (sprite == null)
        {
            bgImage.gameObject.SetActive(false);
            return;
        }

        bgImage.gameObject.SetActive(true);
        bgImage.sprite = sprite;
        bgImage.SetNativeSize();
        RectTransform rect = bgImage.transform as RectTransform;
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
    }

    [Button]
    private void RefreshDirectors()
    {
        directors.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            TutorialDirector director = transform.GetChild(i).GetComponent<TutorialDirector>();
            if (director != null)
                directors.Add(director);
        }
    }
}
