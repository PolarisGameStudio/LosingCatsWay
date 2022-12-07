using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace SimpleUI.Components
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Image))]
    public class SimpleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Enum

        public enum Transition
        {
            None,
            ColorTint,
            SpriteSwap,
            //Animation
        }

        #endregion

        [Header("Basic")]
        public bool Interactable = true;
        public Transition transition = Transition.None;
        public Image targetGraphic;

        [Header("Color Tint")]
        public Color32 normalColor = Color.white;
        public Color32 disableColor = Color.gray;
        public Color32 pressedColor = Color.gray;

        [Header("Sprite Swap")]
        public Sprite pressedSprite;
        Sprite releaseSprite; //Get from image

        [Header("Events")]
        public UnityEvent OnPressed;
        public UnityEvent OnRelease;

        bool pressing = false;

        private void Start()
        {
            targetGraphic = GetComponent<Image>();

            releaseSprite = targetGraphic.sprite;
            ColorCheck();
        }

        private void Update()
        {
            ColorCheck();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) return;

            OnPressed?.Invoke();
            CheckPressedState();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable) return;

            OnRelease?.Invoke();
            CheckReleaseState();
        }

        #region Check

        void ColorCheck()
        {
            if (pressing) return;

            if (interactable)
                targetGraphic.color = normalColor;
            else
                targetGraphic.color = disableColor;
        }

        void CheckPressedState()
        {
            pressing = true;

            if (transition == Transition.None) return;

            targetGraphic.color = pressedColor;

            if (transition != Transition.SpriteSwap) return;
            targetGraphic.sprite = pressedSprite;
        }

        void CheckReleaseState()
        {
            pressing = false;

            if (transition == Transition.None) return;

            targetGraphic.color = normalColor;

            if (transition != Transition.SpriteSwap) return;
            targetGraphic.sprite = releaseSprite;
        }

        #endregion

        #region GetSet

        public Image image
        {
            get => targetGraphic;
        }

        public bool interactable
        {
            get
            {
                if (!Interactable)
                {
                    targetGraphic.color = disableColor;
                    targetGraphic.sprite = releaseSprite;
                    pressing = false;
                }

                return Interactable;
            }
            set
            {
                Interactable = value;
                
                if (!Interactable)
                {
                    targetGraphic.color = disableColor;
                    targetGraphic.sprite = releaseSprite;
                    pressing = false;
                }
            }
        }

        #endregion
    }
}