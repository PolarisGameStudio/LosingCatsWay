using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Lean.Touch;
using System;
using DG.Tweening;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using Coffee.UIExtensions;

public class FlowTaskSystem : MvcBehaviour
{
    [Title("UI")]
    public UIView bgMask;
    public UIView uIView;

    [Title("Focus")]
    public UIView focusMaskView;
    public Unmask unmask;
    public Button focusButton;

    [Title("GameObject")]
    public GameObject CloudBg;

    [Title("Flags")] public int flowState = -1; //0=新手教學

    [Title("FlowTasks")]
    public List<FlowTask> flowTasks;

    private int checkpoint;

    private Camera cam;

    public Callback OnClose;

    public void Init()
    {
        cam = Camera.main;
        FocusMaskClose();
        CloudBg.SetActive(false);

        if (flowState >= 0) //新手已過
        {
            CloudBg.SetActive(false);
            ActiveDragCamera(true);
            ActivePinchCamera(true);
            Close();
        }
        else
        {
            CloudBg.SetActive(true);
            Open();
            StartTask();
        }
    }

    public void Open()
    {
        uIView.Show();
    }

    public void Close()
    {
        uIView.Hide();
        OnClose?.Invoke();
    }

    #region Sequence

    private void StartTask()
    {
        // todo 綁定結束要開的東西
        // todo if (task == 0) 當第一新手教學才需要
        OnClose += App.controller.entrance.Open; 
        
        checkpoint = 0;
        NextTask();
    }

    public void NextTask()
    {
        if (checkpoint >= flowTasks.Count)
        {
            print("結束教學");
            ActiveDragCamera(true);
            ActivePinchCamera(true);

            //教學State結束
            flowState++;

            Close();

            // todo 綁定結束要開的東西
            // todo if (task == 0) 當第一新手教學才需要
            OnClose -= App.controller.entrance.Open;

            return;
        }

        DOVirtual.DelayedCall(0f, () => 
        {
            flowTasks[checkpoint].Enter();
            checkpoint++;
        });
    }

    #endregion

    #region Method

    public void ActiveDragCamera(bool active)
    {
        cam.GetComponent<LeanDragCamera>().enabled = active;
    }

    public void ActivePinchCamera(bool active)
    {
        cam.GetComponent<LeanPinchCamera>().enabled = active;
    }

    public void FocusMaskOpen(RectTransform rectTransform, UnityAction OnClick = null)
    {
        unmask.fitTarget = rectTransform;
        
        focusButton.onClick.RemoveAllListeners();
        focusButton.onClick.AddListener(OnClick);

        focusMaskView.Show();
    }

    public void FocusMaskClose()
    {
        focusButton.onClick.RemoveAllListeners();

        focusMaskView.InstantHide();
    }

    public void ActiveBgMask(bool active)
    {
        if (active)
        {
            bgMask.Show();
            return;
        }

        bgMask.InstantHide();
    }

    #endregion
}
