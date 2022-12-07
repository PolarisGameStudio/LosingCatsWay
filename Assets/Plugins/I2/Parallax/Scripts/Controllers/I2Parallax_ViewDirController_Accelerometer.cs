using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Parallax
{
    [System.Serializable]
    public class I2Parallax_ViewDirController_Accelerometer : I2Parallax_ViewDirController
    {
        public float _Sensitivity = 0.1f;

        public bool _AutoCenter = true;
        public float _AutoRecenter_MinMovement = 0.05f;
        public float _AutoRecenter_Delay = 2;
        public float _AutoRecenter_Damping = 0.005f;


        float mLastMovementTime;
        int mNumFramesWithNoMovement;
        Vector3 mLastAcceleration = Vector3.zero;
        Vector2 mLastRotationRate = Vector2.zero;

        public override void Start()
        {
            Input.gyro.enabled = true;
            Input.gyro.updateInterval = 0.01f;
            mLastAcceleration = Input.acceleration;
            base.Start();
            Reset();
        }


        public override void Update()
        {
            var rawRate = (Vector2)(Input.acceleration - mLastAcceleration);
            rawRate = Vector2.Lerp(mLastRotationRate, rawRate, 0.1f);
            rawRate = Vector2.ClampMagnitude(rawRate, 1);

            mLastRotationRate = rawRate;
            mLastAcceleration = Input.acceleration;

            var rate = rawRate * _Sensitivity;

            ViewPosition.x -= rate.x;
            ViewPosition.y -= rate.y;
            ViewPosition.x = Mathf.Clamp(ViewPosition.x, -1f, 1f);
            ViewPosition.y = Mathf.Clamp(ViewPosition.y, -1f, 1f);

            float deltaMovement = rate.SqrMagnitude();

            if (_AutoCenter)
                AutoRecenter(deltaMovement);
        }

        void AutoRecenter(float deltaMovement)
        {
            if (ShouldAutoCenter(deltaMovement))
            {
                ViewPosition = Vector2.Lerp(ViewPosition, Vector2.zero, _AutoRecenter_Damping);
            }
        }

        bool ShouldAutoCenter( float deltaMovement )
        {
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
            ViewPosition = Vector2.zero;
            mLastAcceleration = Vector3.zero;
            mLastRotationRate = Vector2.zero;
        }

    }
}