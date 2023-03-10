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
    
    [Title("Directors")]
    [SerializeField] private List<TutorialDirector> directors;
    [SerializeField] private TutorialDirector startDirector;
    [SerializeField] private TutorialDirector shelterDirector; // todo 還沒做
    [ReadOnly] public TutorialDirector currentDirector;
    [ReadOnly] public TutorialDirector nextDirector;
    
    [Title("Setup")]
    [SerializeField] private LeanDragCamera dragCamera;
    [SerializeField] private LeanPinchCamera pinchCamera;
    [SerializeField] private UIView blackBg;
    [SerializeField] private Image bgImage;

    [Title("Value")]
    // [ReadOnly] public int directorIndex = -1;
    [ReadOnly] public bool isTutorial;
    // public int startTutorialEndPoint; // 新手教學結束點 // Director (4) 就寫 4
    public bool startTutorialEnd;
    public bool shelterTutorialEnd;

    [Title("Debug")] [SerializeField] private bool skipTutorial;
    
    public void Init()
    {
        for (int i = 0; i < directors.Count; i++)
            directors[i].gameObject.SetActive(false);

        if (skipTutorial)
        {
            startTutorialEnd = true;
            shelterTutorialEnd = true;
            return;
        }

        // if (directorIndex < startTutorialEndPoint)
        if (!startTutorialEnd)
        {
            // directorIndex = -1;
            App.system.openFlow.AddAction(ActionStartTutorial);
        }
    }

    // private void SetDirector(int index)
    // {
    //     if (index < 0 || index >= directors.Count)
    //     {
    //         Debug.LogWarning("Director not found.");
    //         Close();
    //         SetCameraDrag(true);
    //         SetCameraPinch(true);
    //         SetBlackBg(false);
    //         return;
    //     }
    //
    //     Open();
    //     currentDirector = directors[index];
    //     currentDirector.gameObject.SetActive(true);
    //     currentDirector.Init();
    //     currentDirector.NextAction();
    //     isTutorial = true;
    // }

    private void SetDirector(TutorialDirector director)
    {
        if (director == null)
        {
            Debug.LogWarning("Director not found.");
            Close();
            SetCameraDrag(true);
            SetCameraPinch(true);
            SetBlackBg(false);
            return;
        }
        
        Open();

        if (currentDirector != null)
            currentDirector.gameObject.SetActive(false);
        
        currentDirector = director;
        currentDirector.gameObject.SetActive(true);
        currentDirector.Init();
        currentDirector.NextAction();
        isTutorial = true;
    }

    public void Next()
    {
        // if (directorIndex >= 0)
        //     directors[directorIndex].gameObject.SetActive(false);
        // SetDirector(directorIndex + 1);
        SetDirector(nextDirector);
    }

    public void Open()
    {
        uiView.InstantShow();
    }
    
    public void Close()
    {
        // directorIndex++;
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
    
    private void ActionStartTutorial()
    {
        nextDirector = startDirector;
        Next();
    }

    public void ActionShelterTutorial()
    {
        nextDirector = shelterDirector;
        Next();
    }

    public void EndStartTutorial()
    {
        startTutorialEnd = true;
        App.SaveData();
        App.system.unlockGrid.RefreshEffect();
    }

    public void EndShelterTutorial()
    {
        shelterTutorialEnd = true;
        App.SaveData();
    }
}
