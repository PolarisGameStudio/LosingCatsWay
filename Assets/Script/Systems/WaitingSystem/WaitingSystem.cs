using System.Collections;
using System.Collections.Generic;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class WaitingSystem : MonoBehaviour
{
    [SerializeField] private UIView view;
    [SerializeField] private Animator animator;
    
    [Button]
    public void Open()
    {
        animator.enabled = true;
        view.Show();
    }
    
    [Button]
    public void Close()
    {
        animator.enabled = false;
        view.InstantHide();
    }
}
