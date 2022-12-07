using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Parallax
{
    [System.Serializable] public class I2Parallax_ViewDirController_Mouse : I2Parallax_ViewDirController
    {
        public Vector2 _Sensitivity = Vector2.one;
        public override void Update()
        {
            ViewPosition.x = _Sensitivity.x * (Mathf.Clamp01(Input.mousePosition.x / Screen.width) *2 - 1);
            ViewPosition.y = _Sensitivity.y * (Mathf.Clamp01(Input.mousePosition.y / Screen.height)*2 - 1);
        }
    }
}