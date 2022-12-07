using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Parallax
{
    public interface IParallaxLayer
    {
        void UpdateLayer(Vector2 dir);
    }

    public class I2Parallax_Manager : SingletonMonoBehaviour<I2Parallax_Manager>
    {
        //---[ Runtime ]-------------------
        I2Parallax_ViewDirController mController;
        I2ParallaxSettings mSettings;
        List<I2Parallax_3DCamera> mCameras = new List<I2Parallax_3DCamera>();
        List<IParallaxLayer> mLayers   = new List<IParallaxLayer>();

        public static void RegisterLayer( IParallaxLayer layer )
        {
            if (!singleton.mLayers.Contains(layer))
                singleton.mLayers.Add(layer);
        }

        public static void UnregisterLayer( IParallaxLayer layer )
        {
            if (ApplicationIsQuitting || !Application.isPlaying)
                return;
            singleton.mLayers.Remove(layer);
            //singleton.DestroyIfEmpty();
        }

        public static void RegisterCamera(I2Parallax_3DCamera camera)
        {
            if (!singleton.mCameras.Contains(camera))
                singleton.mCameras.Add(camera);
        }

        public static void UnregisterCamera(I2Parallax_3DCamera camera)
        {
            if (ApplicationIsQuitting || !Application.isPlaying)
                return;
            singleton.mCameras.Remove(camera);
        }


        void DestroyIfEmpty()
        {
            if (mLayers.Count > 0 || mCameras.Count > 0)
                return;

            if (Application.isPlaying)
                Destroy(gameObject);
            else
                DestroyImmediate(gameObject);
        }

        public void Update()
        {
            if (mSettings == null)
            {
                mController = null;
                mSettings = I2ParallaxSettings.GetSettings();
            }
            if (mSettings == null)
                mSettings = new I2ParallaxSettings();

            if (mController == null)
                SelectController();
            if (mController == null)
                return;

            mController.Update();

            for (int i = 0, imax = mCameras.Count; i < imax; ++i)
                mCameras[i].UpdateLayer( mController.ViewPosition );

            for (int i = 0, imax = mLayers.Count; i < imax; ++i)
                mLayers[i].UpdateLayer( mController.ViewPosition );

            DestroyIfEmpty();
        }

        public Vector3 GetViewPosition()
        {
            if (mController != null)
                return mController.ViewPosition;

            return Vector3.zero;
        }


        public void SelectController()
        {
            #if UNITY_EDITOR
                bool isRemoteConnected = UnityEditor.EditorApplication.isRemoteConnected;
            #else
                bool isRemoteConnected = false;
            #endif

            if (mSettings._UseGyro && (SystemInfo.supportsGyroscope || SystemInfo.supportsAccelerometer || isRemoteConnected))
                SelectController_Gyro();
            else
            if (mSettings._UseMouse && Input.mousePresent)
                SelectController_Mouse();
            else
                mController = null;
        }

        public void SelectController_None()
        {
            if (mController!=null) mController.Stop();
            mController = null;
        }
        public void SelectController_Gyro()
        {
            mController = SystemInfo.supportsGyroscope ? (I2Parallax_ViewDirController)mSettings._Controller_Gyro : (I2Parallax_ViewDirController)mSettings._Controller_Accelerometer;
            mController.Start();
        }
        public void SelectController_Mouse()
        {
            mController = mSettings._Controller_Mouse;
            mController.Start();
        }


        public void Recenter()
        {
            if (mController!=null)
                mController.Reset();
        }
    }
}