using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Parallax
{
    [System.Serializable] public class I2Parallax_ViewDirController_Gyroscope : I2Parallax_ViewDirController
    {
        public bool  _AutoCenter = true;
        public float _AutoRecenter_MinMovement  = 0.05f;
        public float _AutoRecenter_Delay        = 2;
        [Range(0,1)]public float _AutoRecenter_Damping      = 0.005f;


        Quaternion mInitialRotation;
        float mLastMovementTime;
        int mNumFramesWithNoMovement;
        Vector2 lastViewPosition;


        public override void Start()
        {
            Input.gyro.enabled = true;
            //Input.gyro.updateInterval = 0.01f;
            base.Start();
            Reset();
        }


        public override void Update()
        {
            var currentRot = GyroToUnity(Input.gyro.attitude);
            var delta = Quaternion.Inverse(mInitialRotation) * currentRot;

            // Remove Roll
            var euler = delta.eulerAngles;
            delta = Quaternion.Euler(euler.x, euler.y, 0);

            ViewPosition = delta * new Vector3(0, 0, 1);

            ViewPosition.x = Mathf.Clamp(ViewPosition.x, -1, 1);
            ViewPosition.y = Mathf.Clamp(ViewPosition.y, -1, 1);

            if (_AutoCenter)
                AutoRecenter(currentRot);
        }

        void AutoRecenter( Quaternion currentRot )
        {
            if (ShouldAutoCenter())
            {
                mInitialRotation = Quaternion.Slerp(mInitialRotation, currentRot, _AutoRecenter_Damping);
            }
        }

        bool ShouldAutoCenter()
        {
            float deltaMovement = Vector2.Distance(lastViewPosition, ViewPosition);
            lastViewPosition = ViewPosition;

            // Detect When there is No Movement, but discard random spikes/noise
            if (deltaMovement < _AutoRecenter_MinMovement)
                mNumFramesWithNoMovement++;
            else
                mNumFramesWithNoMovement--;


            // There has to be 4 continuous frames without movement to be considered (no-moving)
            bool recenter = false;
            if (mNumFramesWithNoMovement >= 4)
            {
                mNumFramesWithNoMovement = 4;
                recenter = (Time.time - mLastMovementTime) > _AutoRecenter_Delay;
            }
            else
            if (mNumFramesWithNoMovement <= 0)
            {
                mNumFramesWithNoMovement = 0;
                mLastMovementTime = Time.time;
            }

            return recenter;
        }


        public override void Reset()
        {
            base.Reset();
            mInitialRotation = GyroToUnity(Input.gyro.attitude);
            lastViewPosition = Vector2.zero;
        }

        static Quaternion GyroToUnity(Quaternion q)
        {
            return new Quaternion(q.x, q.y, -q.z, -q.w);
        }
    }
}