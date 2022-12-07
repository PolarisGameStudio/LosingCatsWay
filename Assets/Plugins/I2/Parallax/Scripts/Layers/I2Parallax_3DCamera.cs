using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Parallax
{
    [AddComponentMenu("I2/Parallax/I2Parallax 3D Camera")]
    [RequireComponent(typeof(Camera))]
    public class I2Parallax_3DCamera : MonoBehaviour, IParallaxLayer
    {
        public float   _Depth;
        public Vector2 _Extends;

        Transform mTransform;
        Quaternion mCameraInitialRot;
        Vector3 mCameraInitialPos;
        float mAspectRatio;


        public void OnEnable()
        {
            mTransform = transform;
            mCameraInitialRot = mTransform.localRotation;
            mCameraInitialPos = mTransform.localPosition;
            mAspectRatio = GetComponent<Camera>().aspect;

            I2Parallax_Manager.RegisterLayer(this);
        }

        public void OnDisable()
        {
            I2Parallax_Manager.UnregisterLayer(this);
        }

        public void UpdateLayer( Vector2 viewPos )
        {
            if (mTransform == null)
                mTransform = transform;
            
            Vector3 dir = new Vector3(viewPos.x * _Extends.x * mAspectRatio, viewPos.y * _Extends.y, -_Depth);
            dir = mCameraInitialRot * dir;

            mTransform.localPosition = mCameraInitialPos + _Depth * (mCameraInitialRot * Vector3.forward) + dir;
            mTransform.rotation      = Quaternion.LookRotation(-dir);
        }
    }
}