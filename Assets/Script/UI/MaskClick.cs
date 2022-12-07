using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class MaskClick : MonoBehaviour
{
    public UnityEvent OnMaskClick;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();

        button.transition = Selectable.Transition.None;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        {
            //App.system.audioSystem.GetSoundPlay(SoundEffectTable.Button_Cancel);
            OnMaskClick?.Invoke();
        });
    }
}
