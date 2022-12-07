using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace LosingCatWay.TextTool
{
    public class TyperTMPro : MonoBehaviour
    {
        public TextMeshProUGUI text;

        public float typingInterval;
        public float typingDelay;

        public bool useRealTime = false;
        public bool autoStart = false;
        public bool repeat = false;
        [ShowIf("repeat", true)] public float repeatFlexTime;

        private string content = "";
        private Coroutine typeRoutine;

        private void Start()
        {
            text.enabled = false;
            content = text.text;

            if (!autoStart) return;
            if (repeat)
            {
                float typingTime = (content.Length * typingInterval) + repeatFlexTime;
                InvokeRepeating("StartTyping", repeatFlexTime, typingTime);
            }
            else
            {
                StartTyping();
            }
        }

        public void StartTyping()
        {
            typeRoutine = StartCoroutine(startTyping());
        }

        public void StopTyping()
        {
            StartCoroutine(stopTyping());
        }

        #region Coroutine

        IEnumerator startTyping()
        {
            text.text = "";

            text.enabled = true;

            if (useRealTime) yield return new WaitForSecondsRealtime(typingDelay);
            else yield return new WaitForSeconds(typingDelay);

            for (int i = 0; i < content.Length; i++)
            {
                text.text += content[i];

                if (useRealTime) yield return new WaitForSecondsRealtime(typingInterval);
                else yield return new WaitForSeconds(typingInterval);
            }
        }

        IEnumerator stopTyping()
        {
            if (typeRoutine != null) StopCoroutine(typeRoutine);
            text.text = "";
            text.text = content;

            if (useRealTime) yield return new WaitForSecondsRealtime(.2f);
            else yield return new WaitForSeconds(.2f);

            text.enabled = false;
        }

        #endregion
    }
}