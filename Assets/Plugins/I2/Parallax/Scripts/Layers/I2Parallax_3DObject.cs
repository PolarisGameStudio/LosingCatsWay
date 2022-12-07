using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Parallax
{
    [AddComponentMenu("I2/Parallax/I2Parallax 3D Object")]
    public class I2Parallax_3DObject : MonoBehaviour, IParallaxLayer
    {
        public float _Depth, _DepthFactor;

        Transform mTransform_Child, mTransform;

        public virtual void OnEnable()
        {
            if (transform.childCount != 1)
            {
                Debug.LogFormat("Layer {0} needs to have 1 child", name);
                return;
            }

            mTransform = transform;
            mTransform_Child = transform.GetChild(0);

            Initialize();
            I2Parallax_Manager.RegisterLayer(this);
        }

        public virtual void OnDisable()
        {
            I2Parallax_Manager.UnregisterLayer(this);
        }

        public void UpdateLayer(Vector2 dir)
        {
            if (mTransform == null)
                return;

            Camera cam = Camera.main;
            Vector3 camPos = cam.transform.position;
            float speed = Screen.dpi / 100.0f;

            // 3d points into screen
            var nearWs = mTransform.position;
            var farWs = mTransform.position + (nearWs - camPos).normalized;

            var nearScr = cam.WorldToScreenPoint(nearWs);
            var farScr = cam.WorldToScreenPoint(farWs);

            // move Screen points
            nearScr += (Vector3)dir * speed * _Depth;
            farScr += (Vector3)dir * speed * (_Depth + _DepthFactor);

            // screen to world
            var newNearWs = cam.ScreenToWorldPoint(nearScr);
            var newFarWs = cam.ScreenToWorldPoint(farScr);


            mTransform_Child.position = newNearWs;

            var rotOld = Quaternion.LookRotation(farWs - nearWs, Vector3.up);
            var rotNew = Quaternion.LookRotation(newFarWs - newNearWs, Vector3.up);

            var fwd = (Quaternion.Inverse(rotOld) * rotNew) * mTransform.forward;
            mTransform_Child.rotation = Quaternion.LookRotation(fwd, mTransform.up);
        }

        void Initialize()
        {
        }
    }
}