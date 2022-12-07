using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Parallax
{
	public class I2ParallaxSettings : ScriptableObject
    {
        //---[ Controllers ]-------------------
        public bool _UseGyro = true;
        public bool _UseMouse = true;

        public I2Parallax_ViewDirController_Gyroscope     _Controller_Gyro              = new I2Parallax_ViewDirController_Gyroscope();
        public I2Parallax_ViewDirController_Accelerometer _Controller_Accelerometer     = new I2Parallax_ViewDirController_Accelerometer();
        public I2Parallax_ViewDirController_Mouse         _Controller_Mouse             = new I2Parallax_ViewDirController_Mouse();

		public const string SETTINGS_NAME = "I2ParallaxSettings";
		static public I2ParallaxSettings GetSettings()
		{
			return Resources.Load<I2ParallaxSettings> (SETTINGS_NAME);
		}


		#if UNITY_EDITOR

		[UnityEditor.MenuItem("Assets/Create/I2 Parallax/Parallax Settings")]
		public static void CreateAsset()
		{
			var asset = GetSettings();
			if (asset == null) 
			{
				if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources"))
					UnityEditor.AssetDatabase.CreateFolder ("Assets", "Resources");
				
				asset = ScriptableObject.CreateInstance<I2ParallaxSettings> ();
				UnityEditor.AssetDatabase.CreateAsset (asset, "Assets/Resources/" + SETTINGS_NAME + ".asset");
				UnityEditor.AssetDatabase.SaveAssets ();
			}

			UnityEditor.EditorUtility.FocusProjectWindow();
			UnityEditor.Selection.activeObject = asset;
		}

		#endif
    }
}